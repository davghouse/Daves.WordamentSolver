using System;
using System.Collections;
using System.Collections.Generic;

namespace Daves.WordamentSolver
{
    // A path corresponds to a tile sequence. A path can yield multiple words, for example, imagine [P] [I/A] [N].
    public class Path : IReadOnlyList<Tile>
    {
        protected internal Path(IReadOnlyList<Tile> tiles)
            => Tiles = tiles;

        public IReadOnlyList<Tile> Tiles { get; }
        public int Length => Tiles.Count;

        protected HashSet<Word> _words = new HashSet<Word>();
        public IReadOnlyCollection<Word> Words => _words;
        protected internal virtual bool AddWord(Word word)
            => _words.Add(word);

        protected double? _pathLength;
        public virtual double PathLength
        {
            get
            {
                if (!_pathLength.HasValue)
                {
                    _pathLength = 0;
                    for (int i = 1; i < Tiles.Count; ++i)
                    {
                        Tile currentTile = Tiles[i - 1];
                        Tile nextTile = Tiles[i];

                        if (currentTile.Row == nextTile.Row
                            || currentTile.Column == nextTile.Column)
                        {
                            _pathLength += 1; // Because next is right, left, down, or up from current.
                        }
                        else
                        {
                            _pathLength += Math.Sqrt(2); // Because next is diagonal from current.
                        }
                    }
                }

                return _pathLength.Value;
            }
        }

        public override string ToString()
            => string.Join(" -> ", Tiles);

        public Tile this[int index] => Tiles[index];
        int IReadOnlyCollection<Tile>.Count => Tiles.Count;
        public IEnumerator<Tile> GetEnumerator() => Tiles.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Tiles).GetEnumerator();
    }
}
