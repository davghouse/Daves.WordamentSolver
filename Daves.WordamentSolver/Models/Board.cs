using System;
using System.Collections.Generic;
using System.Linq;

namespace Daves.WordamentSolver.Models
{
    public sealed class Board
    {
        public Board(int boardWidth, int boardHeight)
            : this(boardWidth, boardHeight, p => null, p => null)
        { }

        public Board(int boardWidth, int boardHeight,
            Func<int, string> stringSelector,
            Func<int, int?> pointsSelector)
        {
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            Tiles = Enumerable.Range(0, BoardSize)
                .Select(p => Tile.Create(
                    row: p / BoardWidth,
                    column: p % BoardWidth,
                    position: p,
                    @string: stringSelector(p),
                    points: pointsSelector(p)))
                .ToArray();
        }

        public int BoardWidth { get; }
        public int BoardHeight { get; }
        public int BoardSize => BoardWidth * BoardHeight;
        public IReadOnlyList<Tile> Tiles { get; }

        public void GuessTilePoints()
        {
            foreach (Tile tile in Tiles)
            {
                tile.GuessPoints();
            }
        }

        public void GuessEmptyTilePoints()
        {
            foreach (Tile tile in Tiles.Where(t => !t.Points.HasValue))
            {
                tile.GuessPoints();
            }
        }

        // So I could name this "GetAdjacentTiles", but you can see how different game modes could allow different moves.
        // The name as is reflects the core game concept, not the specific manifestation of it we see in standard Wordament.
        public IEnumerable<Tile> GetTilesNoMoreThanOneMoveAway(Tile tile)
        {
            for (int r = tile.Row - 1; r <= tile.Row + 1; ++r)
            {
                if (r < 0 || r >= BoardHeight) continue;

                for (int c = tile.Column - 1; c <= tile.Column + 1; ++c)
                {
                    if (c < 0 || c >= BoardWidth) continue;

                    yield return Tiles[r * BoardWidth + c];
                }
            }
        }
    }
}
