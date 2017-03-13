using System.Collections.Generic;
using System.Linq;

namespace Daves.WordamentSolver.Tiles
{
    public class SuffixTile : Tile
    {
        protected SuffixTile(int row, int column, int position, string @string, int? points,
            IReadOnlyDictionary<char, int> basicTileValues = null)
            : base(row, column, position, @string, points)
        { }

        public static SuffixTile TryCreate(int row, int column, int position, string @string, int? points,
            IReadOnlyDictionary<char, int> basicTileValues = null)
        {
            basicTileValues = basicTileValues ?? Board.EnglishBasicTileValues;

            return @string != null
                && @string.Length >= 3
                && @string.Skip(1).All(basicTileValues.ContainsKey)
                && @string[0] == '-'
                ? new SuffixTile(row, column, position, @string, points) : null;
        }

        public string Letters => String.Substring(1, String.Length - 1);

        public override void GuessPoints()
            => Points = Letters.Sum(c => _basicTilesValues[c]) + 5;

        public override IEnumerable<string> Extend(string @string)
        {
            yield return @string + Letters;
        }
    }
}
