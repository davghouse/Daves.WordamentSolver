using System.Collections.Generic;

namespace Daves.WordamentSolver.Tiles
{
    public sealed class EitherOrTile : Tile
    {
        private EitherOrTile(int row, int column, int position, string @string, int? points)
            : base(row, column, position, @string, points)
        { }

        public static EitherOrTile TryCreate(int row, int column, int position, string @string, int? points)
            => @string != null
            && @string.Length == 3
            && char.IsUpper(@string[0])
            && char.IsUpper(@string[2])
            && (@string[1] == '/' || @string[1] == '\\')
            ? new EitherOrTile(row, column, position, @string, points) : null;

        public char FirstLetter => String[0];
        public char SecondLetter => String[2];

        public override void GuessPoints()
            => Points = 20;

        public override IEnumerable<string> Extend(string @string)
        {
            yield return @string + FirstLetter;
            yield return @string + SecondLetter;
        }
    }
}
