using System;
using System.Collections.Generic;
using System.Linq;
using WordamentSolver.Contracts;
using WordamentSolver.Helpers;
using WordamentSolver.Models;

namespace WordamentSolver.Presenters
{
    public sealed class SolverPresenter
    {
        private readonly ISolverView _view;
        private Board _board;
        private Solution _solution;
        private WordComparer _selectedWordComparer;

        public SolverPresenter(ISolverView view)
        {
            _view = view;
            _board = new Board(BoardWidth, BoardHeight);
            _solution = new Solution();

            _view.DisplayOrderByOptions(WordComparer.All, selectedIndex: 1);

            view.OrderByOptionSelectionChanged += View_OrderByOptionSelectionChanged;
            view.SolveWithTilePointsGuess += View_SolveWithTilePointsGuess;
            view.Solve += View_Solve;
            view.WordSelectionChanged += View_WordSelectionChanged;
            view.ClearPath += View_ClearPath;
            view.ClearBoard += View_ClearBoard;
            view.SaveToFile += View_SaveToFile;
            view.LoadFromFile += View_LoadFromFile;
        }

        private int BoardWidth => _view.BoardWidth;
        private int BoardHeight => _view.BoardHeight;
        private int BoardSize => BoardWidth * BoardHeight;

        private void View_OrderByOptionSelectionChanged(int? selectedIndex)
        {
            if (selectedIndex.HasValue)
            {
                _selectedWordComparer = WordComparer.All[selectedIndex.Value];
                _solution.SortWords(_selectedWordComparer);

                _view.DisplaySolution(_solution);
                _view.DisplayPath(null);
            }
            else
            {
                _selectedWordComparer = null;
            }
        }

        private void View_SolveWithTilePointsGuess()
        {
            _board = _view.GetBoard();
            _board.GuessTilePoints();
            _solution = new Solution(_board, _selectedWordComparer);

            _view.DisplayBoard(_board);
            _view.DisplaySolution(_solution);
            _view.DisplayPath(null);
        }

        private void View_Solve()
        {
            _board = _view.GetBoard();
            _board.GuessEmptyTilePoints();
            _solution = new Solution(_board, _selectedWordComparer);

            _view.DisplayBoard(_board);
            _view.DisplaySolution(_solution);
            _view.DisplayPath(null);
        }

        private void View_WordSelectionChanged(int? selectedIndex)
            => _view.DisplayPath(selectedIndex.HasValue ? _solution.Words[selectedIndex.Value] : null);

        private void View_ClearBoard()
        {
            _board = new Board(BoardWidth, BoardHeight);
            _solution = new Solution();

            _view.DisplayBoard(_board);
            _view.DisplaySolution(_solution);
            _view.DisplayPath(null);
        }

        private void View_ClearPath()
            => _view.DisplayPath(null);

        private void View_SaveToFile(string filePath)
        {
            IReadOnlyList<Tile> currentTiles = _view.GetBoard().Tiles;
            FileHelper.WriteAllLines(filePath, currentTiles.Select(t => t.String)
                .Concat(currentTiles.Select(t => t.Points?.ToString())));
        }

        private void View_LoadFromFile(string filePath)
        {
            string[] lines = FileHelper.ReadAllLines(filePath);
            if (lines.Length < BoardSize * 2)
                throw new FormatException($"{filePath} doesn't correctly define a board.");

            string[] tileStrings = new string[BoardSize];
            for (int i = 0; i < BoardSize; ++i)
            {
                tileStrings[i] = lines[i];
            }

            int?[] tilePoints = new int?[BoardSize];
            for (int i = 0; i < BoardSize; ++i)
            {
                int points;
                if (int.TryParse(lines[i + BoardSize], out points))
                {
                    tilePoints[i] = points;
                }
            }

            _board = new Board(BoardWidth, BoardHeight, p => tileStrings[p], p => tilePoints[p]);
            _solution = new Solution();

            _view.DisplayBoard(_board);
            _view.DisplaySolution(_solution);
            _view.DisplayPath(null);
        }
    }
}
