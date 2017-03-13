using System.Collections.Generic;
using System.Linq;

namespace Daves.WordamentSolver.EqualityComparers
{
    public sealed class TilesEqualityComparer : IEqualityComparer<IReadOnlyList<Tile>>
    {
        public bool Equals(IReadOnlyList<Tile> x, IReadOnlyList<Tile> y)
            => x.Count == y.Count
            && x.SequenceEqual(y);

        public int GetHashCode(IReadOnlyList<Tile> tiles)
        {
            unchecked
            {
                int hash = 19;
                foreach (var tile in tiles)
                {
                    hash = hash * 31 + tile.GetHashCode();
                }

                return hash;
            }
        }
    }
}
