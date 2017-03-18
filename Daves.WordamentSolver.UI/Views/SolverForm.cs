using Daves.WordamentSolver.UI.Contracts;
using Daves.WordamentSolver.UI.Helpers;
using Daves.WordamentSolver.UI.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Daves.WordamentSolver.UI.Views
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
            Icon = Resources.BigWIcon;

            _tileStringTextBoxes = tileStringsTableLayoutPanel
                .Controls.Cast<TextBox>()
                .ToArray();
            _tilePointsTextBoxes = tilePointsTableLayoutPanel
                .Controls.Cast<TextBox>()
                .ToArray();
        }

        public int BoardHeight => _boardHeight;
        public int BoardWidth => _boardWidth;
        public int BoardSize => BoardWidth * BoardHeight;

        public event Action<int?> SortBySelectionChanged;
        public event Action SolveWithTilePointsGuess;
        public event Action Solve;
        public event Action<int?> WordSelectionChanged;
        public event Action ClearPath;
        public event Action ClearBoard;
        public event Action<string> SaveToFile;
        public event Action<string> LoadFromFile;

        public void DisplaySortByOptions(IReadOnlyList<WordSorter> wordSorters, int selecteIndex)
        {
            sortByComboBox.Items.Clear();
            sortByComboBox.Items.AddRange(wordSorters
                .Select(wc => wc.Name)
                .ToArray());
            sortByComboBox.SelectedIndex = selecteIndex;
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
            => new Board(BoardHeight, BoardWidth, GetTileString, GetTilePoints); 

        private string GetTileString(int tileStringTextBoxIndex)
            => _tileStringTextBoxes[tileStringTextBoxIndex].Text?.Split('|').LastOrDefault();

        private int? GetTilePoints(int tilePointsTextBoxIndex)
        {
            if (int.TryParse(_tilePointsTextBoxes[tilePointsTextBoxIndex].Text, out int points))
                return points;

            return null;
        }

        public void DisplaySolution(Solution solution)
        {
            int longestPointsStringLength = solution.Words
                .Select(w => w.BestPathPoints.ToString().Length)
                .DefaultIfEmpty()
                .Max();

            wordsListBox.Items.Clear();
            wordsListBox.Items.AddRange(solution.Words
                .Select(w => new { PointsString = w.BestPathPoints.ToString(), WordString = w.String })
                // Pad with whitespaces to right-align the word strings.
                .Select(w => $"{w.PointsString}{new string(' ', longestPointsStringLength - w.PointsString.Length)} {w.WordString}")
                .ToArray());

            totalPointsLabel.Text = $"Total points: {solution.TotalPoints}";
            wordsFoundLabel.Text = $"Words found: {solution.TotalWords}";
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
                    .GetColorGradient(Color.LightGreen, Color.Tomato, word.BestPath.Length)
                    .ToArray();
                for (int i = 0; i < word.BestPath.Length; ++i)
                {
                    TextBox textBox = _tileStringTextBoxes[word.BestPath[i].Position];
                    textBox.Text = $"{i + 1}|{textBox.Text}";
                    textBox.Font = new Font(textBox.Font, FontStyle.Bold);
                    textBox.BackColor = colorGradient[i];
                }

                tileStringsWordLabel.Text = $"[{word.String}]";
            }
            else
            {
                tileStringsWordLabel.Text = null;
            }
        }

        private void SortByComboBox_SelectionChangeCommitted(object sender, EventArgs e)
            => SortBySelectionChanged(sortByComboBox.SelectedIndex != -1 ? sortByComboBox.SelectedIndex : (int?)null);

        private void SolveWithTilePointsGuessButton_Click(object sender, EventArgs e)
        {
            SolveWithTilePointsGuess();

            // Try to make the first word selected. TODO: More direct way to do this?
            SendKeys.Send("{RIGHT}");
            SendKeys.Send("{RIGHT}");
            SendKeys.Send("{RIGHT}");
        }

        private void SolveButton_Click(object sender, EventArgs e)
        {
            Solve();

            // Try to make the first word selected. TODO: More direct way to do this?
            SendKeys.Send("{RIGHT}");
            SendKeys.Send("{RIGHT}");
        }

        private void WordsListBox_SelectedIndexChanged(object sender, EventArgs e)
            => WordSelectionChanged(wordsListBox.SelectedIndex != -1 ? wordsListBox.SelectedIndex : (int?)null);

        private void ClearPathButton_Click(object sender, EventArgs e)
            => ClearPath();

        private void ClearBoardButton_Click(object sender, EventArgs e)
            => ClearBoard();

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SaveToFile(dialog.FileName);
            }
        }

        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
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

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
            => MapAllowedKeyDownsToTab(e, Keys.Space, Keys.Enter);

        private void WordsListBox_KeyDown(object sender, KeyEventArgs e)
            => MapAllowedKeyDownsToTab(e, Keys.Space);

        private void SortByComboBox_KeyDown(object sender, KeyEventArgs e)
            => MapAllowedKeyDownsToTab(e, Keys.Space);

        private void SolveWithTilePointsGuessButton_KeyDown(object sender, KeyEventArgs e)
        {
            MapAllowedKeyDownsToTab(e, Keys.Space);
            MapEnterKeyDownToButtonClick(e, () => SolveWithTilePointsGuessButton_Click(sender, e));
        }

        private void SolveButton_KeyDown(object sender, KeyEventArgs e)
        {
            MapAllowedKeyDownsToTab(e, Keys.Space);
            MapEnterKeyDownToButtonClick(e, () => SolveButton_Click(sender, e));
        }

        private void ClearPathButton_KeyDown(object sender, KeyEventArgs e)
        {
            MapAllowedKeyDownsToTab(e, Keys.Space);
            MapEnterKeyDownToButtonClick(e, () => ClearPathButton_Click(sender, e));
        }

        private void ClearBoardButton_KeyDown(object sender, KeyEventArgs e)
        {
            MapAllowedKeyDownsToTab(e, Keys.Space);
            MapEnterKeyDownToButtonClick(e, () => ClearBoardButton_Click(sender, e));
        }
    }
}
