using System.Collections.Generic;

namespace Daves.WordamentSolver.Tiles
{
    public class BasicTile : Tile
    {
        protected BasicTile(int row, int column, int position, string @string, int? points)
            : base(row, column, position, @string, points)
        { }

        public static BasicTile TryCreate(int row, int column, int position, string @string, int? points)
            => @string != null
            && @string.Length == 1
            && char.IsUpper(@string[0])
            ? new BasicTile(row, column, position, @string, points) : null;

        public char Letter => String[0];

        public override void GuessPoints()
            => Points = BasicTileValues[Letter];

        public override IEnumerable<string> Extend(string @string)
        {
            yield return @string + Letter;
        }
    }
}
