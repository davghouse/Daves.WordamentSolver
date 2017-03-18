using Daves.WordamentSolver.EqualityComparers;
using Daves.WordamentSolver.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Daves.WordamentSolver
{
    public class Board
    {
        protected readonly IReadOnlyDictionary<char, int> _basicTilesValues;

        public Board(int boardHeight, int boardWidth,
            IEnumerable<string> tileStrings,
            IEnumerable<int?> tilePoints,
            IReadOnlyDictionary<char, int> basicTileValues = null)
        {
            BoardHeight = boardHeight;
            BoardWidth = boardWidth;
            _basicTilesValues = basicTileValues ?? BasicTileValues;

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
            Func<int, string> tileStringSelector,
            Func<int, int?> tilePointsSelector,
            IReadOnlyDictionary<char, int> basicTileValues = null)
            : this(boardHeight, boardWidth,
                  Enumerable.Range(0, boardWidth * boardHeight).Select(tileStringSelector),
                  Enumerable.Range(0, boardWidth * boardHeight).Select(tilePointsSelector),
                  basicTileValues)
        { }

        public Board(int boardHeight, int boardWidth,
            IReadOnlyDictionary<char, int> basicTileValues = null)
            : this(boardHeight, boardWidth, p => null, p => null, basicTileValues)
        { }

        public Board(string[,] tileStrings, int?[,] tilePoints,
            IReadOnlyDictionary<char, int> basicTileValues = null)
        {
            BoardHeight = tileStrings.GetLength(0);
            BoardWidth = tileStrings.GetLength(1);
            _basicTilesValues = basicTileValues ?? BasicTileValues;

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

        public int BoardHeight { get; }
        public int BoardWidth { get; }
        public int BoardSize => BoardHeight * BoardWidth;
        public virtual IReadOnlyList<Tile> Tiles { get; protected set; }

        protected virtual Tile CreateTile(int row, int column, int position, string @string, int? points)
            => BasicTile.TryCreate(row, column, position, @string, points, _basicTilesValues)
            ?? DigramTile.TryCreate(row, column, position, @string, points, _basicTilesValues)
            ?? PrefixTile.TryCreate(row, column, position, @string, points, _basicTilesValues)
            ?? SuffixTile.TryCreate(row, column, position, @string, points, _basicTilesValues)
            ?? EitherOrTile.TryCreate(row, column, position, @string, points, _basicTilesValues)
            ?? (Tile)new InvalidTile(row, column, position, @string, points, _basicTilesValues);

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

        public static int? GuessTilePoints(string @string,
            IReadOnlyDictionary<char, int> basicTileValues = null)
        {
            var tile = new Board(0, 0, basicTileValues).CreateTile(0, 0, 0, @string, null);
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

        public static readonly IReadOnlyDictionary<char, int> BasicTileValues = new Dictionary<char, int>(new CaseInsensitiveCharEqualityComparer())
           {{'A', 2}, {'B', 5}, {'C', 3}, {'D', 3}, {'E', 1}, {'F', 5}, {'G', 4}, {'H', 4}, {'I', 2},
            {'J', 10}, {'K', 6}, {'L', 3}, {'M', 4}, {'N', 2}, {'O', 2}, {'P', 4}, {'Q', 8}, {'R', 2},
            {'S', 2}, {'T', 2}, {'U', 4}, {'V', 6}, {'W', 6}, {'X', 9}, {'Y', 5}, {'Z', 8}};
    }
}
