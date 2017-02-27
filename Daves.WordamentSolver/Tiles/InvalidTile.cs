using System.Collections.Generic;

namespace Daves.WordamentSolver.Tiles
{
    public sealed class InvalidTile : Tile
    {
        public InvalidTile(int row, int column, int position, string @string, int? points)
            : base(row, column, position, @string, points)
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
