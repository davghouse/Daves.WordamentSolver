using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WordamentSolver.Contracts;
using WordamentSolver.Helpers;
using WordamentSolver.Models;

namespace WordamentSolver.Views
{
    public sealed partial class SolverForm : Form, ISolverView
    {
        private const int _boardWidth = 4;
        private const int _boardHeight = 4;
        private readonly TextBox[] _tileStringTextBoxes;
        private readonly TextBox[] _tilePointsTextBoxes;

        public SolverForm()
        {
            InitializeComponent();
            _tileStringTextBoxes = tileStringsTableLayoutPanel
                .Controls.Cast<TextBox>()
                .ToArray();
            _tilePointsTextBoxes = tilePointsTableLayoutPanel
                .Controls.Cast<TextBox>()
                .ToArray();
        }

        public int BoardWidth => _boardWidth;
        public int BoardHeight => _boardHeight;
        public int BoardSize => BoardWidth * BoardHeight;

        public event Action<int?> OrderByOptionSelectionChanged;
        public event Action SolveWithTilePointsGuess;
        public event Action Solve;
        public event Action<int?> WordSelectionChanged;
        public event Action ClearPath;
        public event Action ClearBoard;
        public event Action<string> SaveToFile;
        public event Action<string> LoadFromFile;

        public void DisplayOrderByOptions(IReadOnlyList<WordComparer> wordComparers, int selecteIndex)
        {
            orderByComboBox.Items.Clear();
            orderByComboBox.Items.AddRange(wordComparers
                .Select(wc => wc.Name)
                .ToArray());
            orderByComboBox.SelectedIndex = selecteIndex;
        }

        public void DisplayBoard(Board board)
        {
            for (int i = 0; i < BoardSize; ++i)
            {
                _tileStringTextBoxes[i].Text = board.Tiles[i].String;
                _tilePointsTextBoxes[i].Text = board.Tiles[i].Points?.ToString();
            }
        }

        public Board GetBoard()
            => new Board(BoardWidth, BoardHeight, GetTileString, GetTilePoints); 

        private string GetTileString(int tileStringTextBoxIndex)
            => _tileStringTextBoxes[tileStringTextBoxIndex].Text?.Split('|').LastOrDefault();

        private int? GetTilePoints(int tilePointsTextBoxIndex)
        {
            int points;
            if (int.TryParse(_tilePointsTextBoxes[tilePointsTextBoxIndex].Text, out points))
                return points;

            return null;
        }

        public void DisplaySolution(Solution solution)
        {
            wordsListBox.Items.Clear();
            wordsListBox.Items.AddRange(solution.Words
                .Select(w => $"{w.Points} {w.String}")
                .ToArray());

            totalPointsLabel.Text = $"Total points: {solution.TotalPoints}";
            wordsFoundLabel.Text = $"Words found: {solution.WordsFound}";
        }

        public void DisplayPath(Word word)
        {
            // Clean up the styling that might exist from an old path, before establishing the new path.
            foreach (TextBox textBox in _tileStringTextBoxes)
            {
                textBox.Text = textBox.Text?.Split('|').LastOrDefault();
                textBox.Font = new Font(textBox.Font, FontStyle.Regular);
                textBox.BackColor = Color.White;
            }

            if (word != null)
            {
                Color[] colorGradient = ColorHelper
                    .GetColorGradient(Color.LightGreen, Color.Tomato, word.Path.Count)
                    .ToArray();
                for (int i = 0; i < word.Path.Count; ++i)
                {
                    TextBox textBox = _tileStringTextBoxes[word.Path[i].Position];
                    textBox.Text = $"{i + 1}|{textBox.Text}";
                    textBox.Font = new Font(textBox.Font, FontStyle.Bold);
                    textBox.BackColor = colorGradient[i];
                }
            }

            tileStringsWordLabel.Text = $"[{word?.String}]";
        }

        private void orderByComboBox_SelectionChangeCommitted(object sender, EventArgs e)
            => OrderByOptionSelectionChanged(orderByComboBox.SelectedIndex != -1 ? orderByComboBox.SelectedIndex : (int?)null);

        private void solveWithTilePointsGuessButton_Click(object sender, EventArgs e)
        {
            SolveWithTilePointsGuess();

            // Try to make the first word selected. TODO: More direct way to do this?
            SendKeys.Send("{RIGHT}");
            SendKeys.Send("{RIGHT}");
            SendKeys.Send("{RIGHT}");
        }

        private void solveButton_Click(object sender, EventArgs e)
        {
            Solve();

            // Try to make the first word selected. TODO: More direct way to do this?
            SendKeys.Send("{RIGHT}");
            SendKeys.Send("{RIGHT}");
        }

        private void wordsListBox_SelectedIndexChanged(object sender, EventArgs e)
            => WordSelectionChanged(wordsListBox.SelectedIndex != -1 ? wordsListBox.SelectedIndex : (int?)null);

        private void clearPathButton_Click(object sender, EventArgs e)
            => ClearPath();

        private void clearBoardButton_Click(object sender, EventArgs e)
            => ClearBoard();

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 1
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SaveToFile(dialog.FileName);
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 1
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadFromFile(dialog.FileName);
            }
        }

        // Allow for faster tabbing by mapping keys like space or enter to tab.
        private void MapAllowedKeyDownsToTab(KeyEventArgs e, params Keys[] allowedKeys)
        {
            if (allowedKeys.Contains(e.KeyCode))
            {
                e.SuppressKeyPress = true;
                SendKeys.Send("{TAB}");
            }
        }

        // Allow for faster clicking by mapping the enter key to button click.
        private void MapEnterKeyDownToButtonClick(KeyEventArgs e, Action buttonClickHandler)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                buttonClickHandler();
            }
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
            => MapAllowedKeyDownsToTab(e, Keys.Space, Keys.Enter);

        private void wordsListBox_KeyDown(object sender, KeyEventArgs e)
            => MapAllowedKeyDownsToTab(e, Keys.Space);

        private void orderByComboBox_KeyDown(object sender, KeyEventArgs e)
            => MapAllowedKeyDownsToTab(e, Keys.Space);

        private void solveWithTilePointsGuessButton_KeyDown(object sender, KeyEventArgs e)
        {
            MapAllowedKeyDownsToTab(e, Keys.Space);
            MapEnterKeyDownToButtonClick(e, () => solveWithTilePointsGuessButton_Click(sender, e));
        }

        private void solveButton_KeyDown(object sender, KeyEventArgs e)
        {
            MapAllowedKeyDownsToTab(e, Keys.Space);
            MapEnterKeyDownToButtonClick(e, () => solveButton_Click(sender, e));
        }

        private void clearPathButton_KeyDown(object sender, KeyEventArgs e)
        {
            MapAllowedKeyDownsToTab(e, Keys.Space);
            MapEnterKeyDownToButtonClick(e, () => clearPathButton_Click(sender, e));
        }

        private void clearBoardButton_KeyDown(object sender, KeyEventArgs e)
        {
            MapAllowedKeyDownsToTab(e, Keys.Space);
            MapEnterKeyDownToButtonClick(e, () => clearBoardButton_Click(sender, e));
        }
    }
}
