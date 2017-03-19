using System;
using System.Collections.Generic;
using System.Linq;

namespace Daves.WordamentSolver.Tiles
{
    public class PrefixTile : Tile
    {
        protected PrefixTile(int row, int column, int position, string @string, int? points,
            IReadOnlyDictionary<char, int> basicTileValues = null)
            : base(row, column, position, @string, points, basicTileValues)
        { }

        public static PrefixTile TryCreate(int row, int column, int position, string @string, int? points,
            IReadOnlyDictionary<char, int> basicTileValues = null)
        {
            basicTileValues = basicTileValues ?? Board.BasicTileValues;

            return @string != null
                && @string.Length >= 3
                && @string.Take(@string.Length - 1).All(basicTileValues.ContainsKey)
                && @string[@string.Length - 1] == '-'
                ? new PrefixTile(row, column, position, @string, points, basicTileValues) : null;
        }

        public string Letters => String.Substring(0, String.Length - 1);

        public override void GuessPoints()
            => Points = Letters.Sum(c => _basicTilesValues[c]) + 5;

        public override bool CanExtend(IReadOnlyList<Tile> tiles)
            => tiles.Count == 0;

        public override IEnumerable<string> Extend(string @string)
        {
            yield return Letters;
        }
    }
}
