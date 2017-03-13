using System.Collections.Generic;

namespace Daves.WordamentSolver.Tiles
{
    public class BasicTile : Tile
    {
        protected BasicTile(int row, int column, int position, string @string, int? points,
            IReadOnlyDictionary<char, int> basicTileValues = null)
            : base(row, column, position, @string, points, basicTileValues)
        { }

        public static BasicTile TryCreate(int row, int column, int position, string @string, int? points,
            IReadOnlyDictionary<char, int> basicTileValues = null)
        {
            basicTileValues = basicTileValues ?? Board.EnglishBasicTileValues;

            return @string != null
                && @string.Length == 1
                && basicTileValues.ContainsKey(@string[0])
                ? new BasicTile(row, column, position, @string, points, basicTileValues) : null;
        }

        public char Letter => String[0];

        public override void GuessPoints()
            => Points = _basicTilesValues[Letter];

        public override IEnumerable<string> Extend(string @string)
        {
            yield return @string + Letter;
        }
    }
}
