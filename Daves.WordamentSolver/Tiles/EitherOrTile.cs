using System.Collections.Generic;

namespace Daves.WordamentSolver.Tiles
{
    public class EitherOrTile : Tile
    {
        protected EitherOrTile(int row, int column, int position, string @string, int? points,
            IReadOnlyDictionary<char, int> basicTileValues = null)
            : base(row, column, position, @string, points, basicTileValues)
        { }

        public static EitherOrTile TryCreate(int row, int column, int position, string @string, int? points,
            IReadOnlyDictionary<char, int> basicTileValues = null)
        {
            basicTileValues = basicTileValues ?? Board.BasicTileValues;

            return @string != null
                && @string.Length == 3
                && basicTileValues.ContainsKey(@string[0])
                && basicTileValues.ContainsKey(@string[2])
                && (@string[1] == '/' || @string[1] == '\\')
                ? new EitherOrTile(row, column, position, @string, points) : null;
        }

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
