using System.Collections.Generic;

namespace Daves.WordamentSolver.Tiles
{
    public class InvalidTile : Tile
    {
        public InvalidTile(int row, int column, int position, string @string, int? points,
            IReadOnlyDictionary<char, int> basicTileValues = null)
            : base(row, column, position, @string, points, basicTileValues)
        { }

        public override void GuessPoints()
            => Points = null;

        public override bool CanExtend(IReadOnlyList<Tile> path)
            => false;

        public override IEnumerable<string> Extend(string @string)
        {
            yield break;
        }
    }
}
