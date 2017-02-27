using System.Collections.Generic;
using Daves.WordamentSolver.Models.Tiles;

namespace Daves.WordamentSolver.Models
{
    public abstract class Tile
    {
        protected Tile(int row, int column, int position, string @string, int? points)
        {
            Row = row;
            Column = column;
            Position = position;
            String = @string;
            Points = points;
        }

        public static Tile Create(int row, int column, int position, string @string, int? points)
            => BasicTile.TryCreate(row, column, position, @string, points)
            ?? DigramTile.TryCreate(row, column, position, @string, points)
            ?? PrefixTile.TryCreate(row, column, position, @string, points)
            ?? SuffixTile.TryCreate(row, column, position, @string, points)
            ?? EitherOrTile.TryCreate(row, column, position, @string, points)
            ?? (Tile)new InvalidTile(row, column, position, @string, points);

        public int Row { get; }
        public int Column { get; }
        public int Position { get; }
        public string String { get; }
        public int? Points { get; set; }

        public abstract void GuessPoints();

        // Provided for convenience so doing a contains check against the path built so far isn't necessary.
        public bool IsTaken { get; set; }

        // Assuming that the rules governing board movement allow this tile to be reached in one move from the last tile.
        public virtual bool CanExtend(IReadOnlyList<Tile> path)
            => !IsTaken && !(path[path.Count - 1] is SuffixTile);

        // Assuming it's been verified this tile can extend the path yielding the given string.
        public abstract IEnumerable<string> Extend(string @string);

        public static readonly IReadOnlyDictionary<char, int> BasicTileValues = new Dictionary<char, int>
           {{'A', 2}, {'B', 5}, {'C', 3}, {'D', 3}, {'E', 1}, {'F', 5}, {'G', 4}, {'H', 4}, {'I', 2},
           {'J', 10}, {'K', 6}, {'L', 3}, {'M', 4}, {'N', 2}, {'O', 2}, {'P', 4}, {'Q', 8},
           {'R', 2}, {'S', 2}, {'T', 2}, {'U', 4}, {'V', 6}, {'W', 6}, {'X', 9}, {'Y', 5}, {'Z', 8}};
    }
}
