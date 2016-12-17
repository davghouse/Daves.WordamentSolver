using System;
using System.Collections.Generic;
using System.Linq;
using WordamentSolver.Tries;

namespace WordamentSolver.Models
{
    public sealed class Solution
    {
        private static Trie _dictionary;
        private readonly Board _board;
        private readonly HashSet<Word> _wordsSet = new HashSet<Word>();
        private readonly Word[] _words = new Word[0];

        public static void SetDictionary(IEnumerable<string> dictionary)
        {
            _dictionary = new Trie();
            foreach (string word in dictionary)
            {
                if (word.Length > 2)
                {
                    _dictionary.Add(word.ToUpper());
                }
            }
        }

        public Solution()
        { }

        public Solution(Board board, WordComparer wordComparer = null)
        {
            _board = board;

            foreach (Tile tile in _board.Tiles)
            {
                SearchUsing(tile, new List<Tile>(), string.Empty, new TrieSearchResult());
            }

            _words = _wordsSet.ToArray();
            TotalPoints = _words.Sum(w => w.Points);
            WordsFound = _words.Length;
            SortWords(wordComparer);
        }

        // We use the rules governing board movement (not overridable, but may be in the future) to figure out what tiles
        // to try using. For each of those tiles, we check to see if it can extend the path. For example, if a tile is already
        // used in the path then it can't extend it. For each of those tiles, we recurse. In the recursion, we know the tile
        // can extend the path, so we stick it on. In extending the path it produces some number of strings (one, possibly two) to
        // check the trie for. Since these strings are prefixed by the string searched for previously, we use the previous search
        // result to jump to the start location in the trie. If the trie doesn't contain the string as a prefix then we don't recurse,
        // because it'll contain nothing after this point either, since we're prefixing whatever gets built next. Jumping down the
        // trie with the previous search result isn't a big deal, performance-wise--but stopping when we know the string doesn't prefix
        // anything in the dictionary is. A simple HashSet doesn't get us the latter. A sorted list can, but with an extra log(n) factor.
        private void SearchUsing(Tile nextTile, List<Tile> pathBeingBuilt, string stringSoFar, TrieSearchResult searchResultSoFar)
        {
            pathBeingBuilt.Add(nextTile);
            nextTile.IsTaken = true;

            foreach (string @string in nextTile.Extend(stringSoFar))
            {
                TrieSearchResult searchResult = _dictionary.Search(@string, searchResultSoFar.TerminalNode);

                if (searchResult.ContainsWord)
                {
                    _wordsSet.Add(new Word(@string, pathBeingBuilt));
                }

                if (searchResult.ContainsPrefix)
                {
                    foreach (Tile tile in _board.GetTilesNoMoreThanOneMoveAway(nextTile)
                        .Where(t => t.CanExtend(pathBeingBuilt)))
                    {
                        SearchUsing(tile, pathBeingBuilt, @string, searchResult);
                    }
                }
            }

            pathBeingBuilt.RemoveAt(pathBeingBuilt.Count - 1);
            nextTile.IsTaken = false;
        }

        public IReadOnlyList<Word> Words => _words;
        public int? TotalPoints { get; }
        public int? WordsFound { get; }

        public void SortWords(WordComparer wordComparer)
            => Array.Sort(_words, wordComparer ?? WordComparer.Points);
    }
}
