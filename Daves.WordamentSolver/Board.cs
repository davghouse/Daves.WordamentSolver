using Daves.WordamentSolver.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Daves.WordamentSolver
{
    public class Board
    {
        protected Board()
        { }

        public Board(int boardHeight, int boardWidth,
            IEnumerable<string> tileStrings,
            IEnumerable<int?> tilePoints)
        {
            BoardHeight = boardHeight;
            BoardWidth = boardWidth;
            Tiles = tileStrings.Zip(tilePoints, (s, p) => new { @string = s, points = p })
                .Select((a, i) => CreateTile(
                    row: i / BoardWidth,
                    column: i % BoardWidth,
                    position: i,
                    @string: a.@string,
                    points: a.points))
                .ToArray();
        }

        public Board(int boardHeight, int boardWidth,
            Func<int, string> stringSelector,
            Func<int, int?> pointsSelector)
            : this(boardHeight, boardWidth,
                  Enumerable.Range(0, boardWidth * boardHeight).Select(stringSelector),
                  Enumerable.Range(0, boardWidth * boardHeight).Select(pointsSelector))
        { }

        public Board(int boardHeight, int boardWidth)
            : this(boardHeight, boardWidth, p => null, p => null)
        { }

        public Board(string[,] tileStrings, int?[,] tilePoints)
        {
            BoardHeight = tileStrings.GetLength(0);
            BoardWidth = tileStrings.GetLength(1);

            var tiles = new Tile[BoardSize];
            for (int r = 0; r < BoardHeight; ++r)
            {
                for (int c = 0; c < BoardWidth; ++c)
                {
                    int position = r * BoardWidth + c;
                    tiles[position] = CreateTile(
                        row: r,
                        column: c,
                        position: position,
                        @string: tileStrings[r, c],
                        points: tilePoints[r, c]);
                }
            }

            Tiles = tiles;
        }

        public virtual int BoardHeight { get; protected set; }
        public virtual int BoardWidth { get; protected set; }
        public int BoardSize => BoardHeight * BoardWidth;
        public virtual IReadOnlyList<Tile> Tiles { get; protected set; }

        public virtual Tile CreateTile(int row, int column, int position, string @string, int? points)
            => BasicTile.TryCreate(row, column, position, @string, points)
            ?? DigramTile.TryCreate(row, column, position, @string, points)
            ?? PrefixTile.TryCreate(row, column, position, @string, points)
            ?? SuffixTile.TryCreate(row, column, position, @string, points)
            ?? EitherOrTile.TryCreate(row, column, position, @string, points)
            ?? (Tile)new InvalidTile(row, column, position, @string, points);

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

        public static int? GuessTilePoints(string @string)
        {
            var tile = new Board().CreateTile(0, 0, 0, @string, null);
            tile.GuessPoints();

            return tile.Points;
        }

        // So I could name this "GetAdjacentTiles", but you can see how different game modes could allow different moves.
        // The name as is reflects the core game concept, not the specific manifestation of it we see in standard Wordament.
        public virtual IEnumerable<Tile> GetTilesNoMoreThanOneMoveAway(Tile tile)
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
