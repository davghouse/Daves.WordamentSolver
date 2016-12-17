using System.Collections.Generic;
using System.Linq;

namespace WordamentSolver.Models.Tiles
{
    public sealed class SuffixTile : Tile
    {
        private SuffixTile(int row, int column, int position, string @string, int? points)
            : base(row, column, position, @string, points)
        { }

        public static SuffixTile TryCreate(int row, int column, int position, string @string, int? points)
            => @string != null
            && @string.Length >= 3
            && @string.Skip(1).All(c => char.IsUpper(c))
            && @string[0] == '-'
            ? new SuffixTile(row, column, position, @string, points) : null;

        public string Letters => String.Substring(1, String.Length - 1);

        public override void GuessPoints()
            => Points = Letters.Sum(c => BasicTileValues[c]) + 5;

        public override IEnumerable<string> Extend(string @string)
        {
            yield return @string + Letters;
        }
    }
}
