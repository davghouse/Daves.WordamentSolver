using Daves.WordamentSolver.Tries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Daves.WordamentSolver
{
    // This is a stripped down version of Solution used to get a word count. The normal Solution has some overhead
    // now that it maintains word/path relationships, and sometimes all I want is a word count. Specifically, to
    // generate a decent board to play on I randomly generate a lot and then choose the one with the most words.
    public class SimpleSolution
    {
        protected readonly Board _board;
        protected readonly HashSet<string> _words = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public SimpleSolution(Board board)
        {
            if (Solution.Dictionary == null) { Solution.SetDictionary(); }

            _board = board;

            foreach (Tile tile in _board.Tiles)
            {
                SearchUsing(tile, new List<Tile>(), string.Empty, new TrieSearchResult());
            }

            Words = _words;
            TotalWords = _words.Count;
        }

        protected virtual void SearchUsing(Tile nextTile, List<Tile> tilesSoFar, string wordSoFar, TrieSearchResult searchResultSoFar)
        {
            tilesSoFar.Add(nextTile);
            nextTile.IsTaken = true;

            foreach (string word in nextTile.Extend(wordSoFar))
            {
                TrieSearchResult searchResult = Solution.Dictionary.Search(word, searchResultSoFar.TerminalNode);

                if (searchResult.ContainsWord)
                {
                    _words.Add(word);
                }

                if (searchResult.ContainsPrefix)
                {
                    foreach (Tile tile in _board.Tiles.Where(t => t.CanExtend(tilesSoFar)))
                    {
                        SearchUsing(nextTile: tile, tilesSoFar: tilesSoFar, wordSoFar: word, searchResultSoFar: searchResult);
                    }
                }
            }

            tilesSoFar.RemoveAt(tilesSoFar.Count - 1);
            nextTile.IsTaken = false;
        }

        public IReadOnlyCollection<string> Words { get; }
        public int TotalWords { get; }

        public bool ContainsWord(string word)
            => _words.Contains(word);
    }
}
