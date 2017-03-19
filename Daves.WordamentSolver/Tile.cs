using Daves.WordamentSolver.Tiles;
using System;
using System.Collections.Generic;

namespace Daves.WordamentSolver
{
    public abstract class Tile
    {
        protected readonly IReadOnlyDictionary<char, int> _basicTilesValues;

        protected Tile(int row, int column, int position, string @string, int? points,
            IReadOnlyDictionary<char, int> basicTileValues = null)
        {
            Row = row;
            Column = column;
            Position = position;
            String = @string;
            Points = points;
            _basicTilesValues = basicTileValues ?? Board.BasicTileValues;
        }

        public int Row { get; }
        public int Column { get; }
        public int Position { get; }
        public string String { get; }
        public virtual int? Points { get; set; }

        public abstract void GuessPoints();

        // Provided for convenience so doing a contains check against the path built so far isn't necessary.
        public bool IsTaken { get; set; }

        public virtual bool CanExtend(IReadOnlyList<Tile> tiles)
        {
            if (IsTaken) return false;
            if (tiles.Count == 0) return true;

            Tile previousTile = tiles[tiles.Count - 1];
            return Math.Abs(previousTile.Row - Row) <= 1
                && Math.Abs(previousTile.Column - Column) <= 1
                && !(previousTile is SuffixTile);
        }

        // Assuming it's been verified this tile can extend the path yielding the given string.
        public abstract IEnumerable<string> Extend(string @string);

        public override string ToString()
            => $"[{Row}, {Column}, {String}]";
    }
}
