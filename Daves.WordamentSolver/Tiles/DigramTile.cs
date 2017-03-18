using System.Collections.Generic;
using System.Linq;

namespace Daves.WordamentSolver.Tiles
{
    public class DigramTile : Tile
    {
        protected DigramTile(int row, int column, int position, string @string, int? points,
            IReadOnlyDictionary<char, int> basicTileValues = null)
            : base(row, column, position, @string, points, basicTileValues)
        { }

        public static DigramTile TryCreate(int row, int column, int position, string @string, int? points,
            IReadOnlyDictionary<char, int> basicTileValues = null)
        {
            basicTileValues = basicTileValues ?? Board.BasicTileValues;

            return @string != null
                && @string.Length == 2
                && @string.All(basicTileValues.ContainsKey)
                ? new DigramTile(row, column, position, @string, points, basicTileValues) : null;
        }

        public char FirstLetter => String[0];
        public char SecondLetter => String[1];

        public override void GuessPoints()
            => Points = _basicTilesValues[FirstLetter] + _basicTilesValues[SecondLetter] + 5;

        public override IEnumerable<string> Extend(string @string)
        {
            yield return @string + FirstLetter + SecondLetter;
        }
    }
}
