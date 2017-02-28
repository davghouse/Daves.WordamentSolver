using System;
using System.Collections.Generic;
using System.Linq;
using Daves.WordamentSolver.UI.Contracts;
using Daves.WordamentSolver.UI.Helpers;

namespace Daves.WordamentSolver.UI.Presenters
{
    public class SolverPresenter
    {
        private readonly ISolverView _view;
        private Board _board;
        private Solution _solution;
        private WordSorter _selectedWordSorter = WordSorter.All[0];

        public SolverPresenter(ISolverView view)
        {
            _view = view;
            _board = new Board(BoardHeight, BoardWidth);
            _solution = new Solution();

            _view.DisplaySortByOptions(WordSorter.All, selectedIndex: 0);

            view.SortBySelectionChanged += View_SortBySelectionChanged;
            view.SolveWithTilePointsGuess += View_SolveWithTilePointsGuess;
            view.Solve += View_Solve;
            view.WordSelectionChanged += View_WordSelectionChanged;
            view.ClearPath += View_ClearPath;
            view.ClearBoard += View_ClearBoard;
            view.SaveToFile += View_SaveToFile;
            view.LoadFromFile += View_LoadFromFile;
        }

        private int BoardHeight => _view.BoardHeight;
        private int BoardWidth => _view.BoardWidth;
        private int BoardSize => BoardWidth * BoardHeight;

        private void View_SortBySelectionChanged(int? selectedIndex)
        {
            if (selectedIndex.HasValue)
            {
                _selectedWordSorter = WordSorter.All[selectedIndex.Value];
                _solution.SortWords(_selectedWordSorter);

                _view.DisplaySolution(_solution);
                _view.DisplayPath(null);
            }
            else
            {
                _selectedWordSorter = null;
            }
        }

        private void View_SolveWithTilePointsGuess()
        {
            _board = _view.GetBoard();
            _board.GuessTilePoints();
            _solution = new Solution(_board, _selectedWordSorter);

            _view.DisplayBoard(_board);
            _view.DisplaySolution(_solution);
            _view.DisplayPath(null);
        }

        private void View_Solve()
        {
            _board = _view.GetBoard();
            _board.GuessEmptyTilePoints();
            _solution = new Solution(_board, _selectedWordSorter);

            _view.DisplayBoard(_board);
            _view.DisplaySolution(_solution);
            _view.DisplayPath(null);
        }

        private void View_WordSelectionChanged(int? selectedIndex)
            => _view.DisplayPath(selectedIndex.HasValue ? _solution.Words[selectedIndex.Value] : null);

        private void View_ClearBoard()
        {
            _board = new Board(BoardHeight, BoardWidth);
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

            _board = new Board(BoardHeight, BoardWidth, p => tileStrings[p], p => tilePoints[p]);
            _solution = new Solution();

            _view.DisplayBoard(_board);
            _view.DisplaySolution(_solution);
            _view.DisplayPath(null);
        }
    }
}
