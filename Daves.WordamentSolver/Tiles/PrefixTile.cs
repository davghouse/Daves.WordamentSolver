using System;
using System.Collections.Generic;
using System.Linq;

namespace Daves.WordamentSolver.Tiles
{
    public class PrefixTile : Tile
    {
        protected PrefixTile(int row, int column, int position, string @string, int? points)
            : base(row, column, position, @string, points)
        { }

        public static PrefixTile TryCreate(int row, int column, int position, string @string, int? points)
            => @string != null
            && @string.Length >= 3
            && @string.Take(@string.Length - 1).All(c => char.IsUpper(c))
            && @string[@string.Length - 1] == '-'
            ? new PrefixTile(row, column, position, @string, points) : null;

        public string Letters => String.Substring(0, String.Length - 1);

        public override void GuessPoints()
            => Points = Letters.Sum(c => BasicTileValues[c]) + 5;

        public override bool CanExtend(IReadOnlyList<Tile> path)
            => path.Count == 0;

        public override IEnumerable<string> Extend(string @string)
        {
            yield return Letters;
        }
    }
}
