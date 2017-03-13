using Daves.WordamentSolver.EqualityComparers;
using Daves.WordamentSolver.Tries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Daves.WordamentSolver
{
    public class Solution : IReadOnlyList<Word>
    {
        protected internal static Trie Dictionary { get; protected set; }
        public static void SetDictionary(IEnumerable<string> @strings)
            => Dictionary = new Trie(@strings.Where(s => s.Length > 2), new CaseInsensitiveCharEqualityComparer());

        protected readonly Board _board;
        protected readonly Dictionary<string, Word> _stringWordMap = new Dictionary<string, Word>(StringComparer.OrdinalIgnoreCase);
        protected readonly Dictionary<IReadOnlyList<Tile>, Path> _tilesPathMap = new Dictionary<IReadOnlyList<Tile>, Path>(new TilesEqualityComparer());
        protected readonly Word[] _words = new Word[0];

        public Solution()
        { }

        public Solution(Board board, WordSorter wordSorter = null)
        {
            _board = board;

            foreach (Tile tile in _board.Tiles)
            {
                SearchUsing(tile, new List<Tile>(), string.Empty, new TrieSearchResult());
            }

            _words = _stringWordMap.Values.ToArray();
            TotalPoints = _words.Sum(w => w.BestPathPoints);
            SortWords(wordSorter);
        }

        // We use the rules governing board movement to figure out what tiles to try using next. For each of those tiles, we check
        // to see if it can extend the path. For example, if a tile is already used in the path, then it can't extend it. For each
        // of those tiles, we recurse. In the recursion, we know the tile can extend the path, so we stick it on. In extending the
        // path it produces some number of strings (one, possibly two) to check the trie for. Since these strings are prefixed by
        // the string searched for previously, we use the previous search result to jump to the start location in the trie. If the
        // trie doesn't contain the string as a prefix then we don't recurse, because it'll contain nothing after this point either,
        // since we're prefixing whatever would be next. Jumping down the trie with the previous search result isn't a big deal,
        // performance-wise--but stopping when we know the string doesn't prefix anything in the dictionary is. A simple HashSet
        // doesn't get us the latter. A sorted list can, but with an extra log(n) factor.
        protected virtual void SearchUsing(Tile nextTile, List<Tile> tilesSoFar, string stringSoFar, TrieSearchResult searchResultSoFar)
        {
            tilesSoFar.Add(nextTile);
            nextTile.IsTaken = true;

            foreach (string @string in nextTile.Extend(stringSoFar))
            {
                TrieSearchResult searchResult = Dictionary.Search(@string, searchResultSoFar.TerminalNode);

                if (searchResult.ContainsWord)
                {
                    // If we don't have a corresponding Word for the string yet, create one.
                    if (!_stringWordMap.TryGetValue(@string, out Word word))
                    {
                        word = new Word(@string);
                        _stringWordMap[@string] = word;
                    }

                    // If we don't have a corresponding Path for the tiles yet, create one.
                    if (!_tilesPathMap.TryGetValue(tilesSoFar, out Path path))
                    {
                        var tilesCopy = tilesSoFar.ToArray();
                        path = new Path(tilesCopy);
                        _tilesPathMap[tilesCopy] = path;
                    }

                    // Tie the word and the path together.
                    word.AddPath(path);
                    path.AddWord(word);
                }

                if (searchResult.ContainsPrefix)
                {
                    foreach (Tile tile in _board.GetTilesNoMoreThanOneMoveAway(nextTile)
                        .Where(t => t.CanExtend(tilesSoFar)))
                    {
                        SearchUsing(nextTile: tile, tilesSoFar: tilesSoFar, stringSoFar: @string, searchResultSoFar: searchResult);
                    }
                }
            }

            tilesSoFar.RemoveAt(tilesSoFar.Count - 1);
            nextTile.IsTaken = false;
        }

        public IReadOnlyList<Word> Words => _words;
        public int TotalPoints { get; set; }
        public int TotalWords => Words.Count;

        public bool ContainsWord(string @string)
            => _stringWordMap.ContainsKey(@string);

        public bool TryGetWord(string @string, out Word word)
            => _stringWordMap.TryGetValue(@string, out word);

        public bool ContainsPath(IReadOnlyList<Tile> tiles)
            => _tilesPathMap.ContainsKey(tiles);

        public bool TryGetPath(IReadOnlyList<Tile> tiles, out Path path)
            => _tilesPathMap.TryGetValue(tiles, out path);

        public virtual void SortWords(WordSorter wordSorter)
        {
            if (wordSorter != null)
            {
                wordSorter.Sort(_words);
            }
        }

        public override string ToString()
            => $"Total points: {TotalPoints}, Total words: {TotalWords}";

        Word IReadOnlyList<Word>.this[int index] => Words[index];
        int IReadOnlyCollection<Word>.Count => TotalWords;
        IEnumerator<Word> IEnumerable<Word>.GetEnumerator() => Words.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Words).GetEnumerator();
    }
}
