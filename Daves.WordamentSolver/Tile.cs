using Daves.WordamentSolver.Tiles;
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
            _basicTilesValues = basicTileValues ?? Board.EnglishBasicTileValues;
        }

        public int Row { get; }
        public int Column { get; }
        public int Position { get; }
        public string String { get; }
        public virtual int? Points { get; set; }

        public abstract void GuessPoints();

        // Provided for convenience so doing a contains check against the path built so far isn't necessary.
        public bool IsTaken { get; set; }

        // Assuming that the rules governing board movement allow this tile to be reached in one move from the last tile.
        public virtual bool CanExtend(IReadOnlyList<Tile> path)
            => !IsTaken && !(path[path.Count - 1] is SuffixTile);

        // Assuming it's been verified this tile can extend the path yielding the given string.
        public abstract IEnumerable<string> Extend(string @string);

        public override string ToString()
            => $"[{Row}, {Column}, {String}]";
    }
}
