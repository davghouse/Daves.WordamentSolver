using System.Collections.Generic;
using System.Linq;

namespace Daves.WordamentSolver.Tiles
{
    public sealed class DigramTile : Tile
    {
        private DigramTile(int row, int column, int position, string @string, int? points)
            : base(row, column, position, @string, points)
        { }

        public static DigramTile TryCreate(int row, int column, int position, string @string, int? points)
            => @string != null
            && @string.Length == 2
            && @string.All(c => char.IsUpper(c))
            ? new DigramTile(row, column, position, @string, points) : null;

        public char FirstLetter => String[0];
        public char SecondLetter => String[1];

        public override void GuessPoints()
            => Points = BasicTileValues[FirstLetter] + BasicTileValues[SecondLetter] + 5;

        public override IEnumerable<string> Extend(string @string)
        {
            yield return @string + FirstLetter + SecondLetter;
        }
    }
}
