using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WordamentSolverWinFormsGUI
{
  public partial class Form1 : Form
  {
    // There's a List<int> corresponding to each word; its path through the board.
    private List<List<string>> words = new List<List<string>>();
    private List<List<List<int>>> paths = new List<List<List<int>>>();
    private List<List<int>> points = new List<List<int>>();
    List<WordPointPath> wordPointPaths = new List<WordPointPath>();
    private const int dim = 4;

    public Form1()
    {
      InitializeComponent();
      comboBox1.SelectedIndex = 5;
    }

    // Remapping the 32 Tile textboxes' Space and Enter to Tab.
    private void textBox_KeyDown(object sender, KeyEventArgs e)
    {
      comboBox1.TabStop = false;
      if (e.KeyCode == Keys.Space)
      {
        e.SuppressKeyPress = true;
        SendKeys.Send("{TAB}");
      }
      else if (e.KeyCode == Keys.Enter)
      {
        e.SuppressKeyPress = true;
        SendKeys.Send("{TAB}");
      }
    }


    private void ListBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Space)
      {
        e.SuppressKeyPress = true;
        SendKeys.Send("{TAB}");
      }
    }

    private void comboBox1_Enter(object sender, EventArgs e)
    {
      //comboBox1.DroppedDown = true;
    }

    private void comboBox1_DropDownClosed(object sender, EventArgs e)
    {
      this.comboBox1.TabStop = false;
    }

    private void comboBox1_KeyDown(object sener, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Space)
      {
        e.SuppressKeyPress = true;
        SendKeys.Send("{TAB}");
      }

    }

    // Go w/ TP guess button tabbing
    private void button1_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Space)
      {
        e.SuppressKeyPress = true;
        SendKeys.Send("{TAB}");
      }
      else if (e.KeyCode == Keys.Enter)
      {
        button1_Click(sender, e);
      }
    }

    // Go button tabbing
    private void button2_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Space)
      {
        e.SuppressKeyPress = true;
        SendKeys.Send("{TAB}");
      }
      else if (e.KeyCode == Keys.Enter)
      {
        e.SuppressKeyPress = true;
        button2_Click(sender, e);
      }
    }

    // Clear button tabbing
    private void button3_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Space)
      {
        e.SuppressKeyPress = true;
        SendKeys.Send("{TAB}");
      }
      else if (e.KeyCode == Keys.Enter)
      {
        e.SuppressKeyPress = true;
        button3_Click(sender, e);
      }
    }

    // Clear Boards button
    private void button3_Click(object sender, EventArgs e)
    {
      ClearPathFormatting();
      foreach (Control c in tableLayoutPanel1.Controls)
      {
        c.Text = "";
      }
      foreach (Control c in tableLayoutPanel2.Controls)
      {
        c.Text = "";
      }
      listBox1.Items.Clear();
      label6.Text = "Total points: ";
      label7.Text = "Words found: ";
    }

    // Clear Path button
    private void button4_Click(object sender, EventArgs e)
    {
      ClearPathFormatting();
    }

    // Go w/ TP guess
    private static Dictionary<char, int> basicTileValues = new Dictionary<char, int>
                                               {{'A', 2}, {'B', 5}, {'C', 3}, {'D', 3}, {'E', 1}, {'F', 5}, {'G', 4}, {'H', 4}, {'I', 2}, 
                                               {'J', 10}, {'K', 6}, {'L', 3}, {'M', 4}, {'N', 2}, {'O', 2}, {'P', 4}, {'Q', 8}, 
                                               {'R', 2}, {'S', 2}, {'T', 2}, {'U', 4}, {'V', 6}, {'W', 6}, {'X', 9}, {'Y', 5}, {'Z', 8}};
    private void button1_Click(object sender, EventArgs e)
    {
      ClearPathFormatting();
      foreach (Control c in tableLayoutPanel1.Controls)
      {
        int i = Convert.ToInt32(c.Name.Substring(7));
        foreach (Control d in tableLayoutPanel2.Controls)
        {
          int j = Convert.ToInt32(d.Name.Substring(7)) - dim * dim;
          if (i == j)
          {
            string temp = c.Text;
            int tileValue = 0;
            for (int k = 0; k < temp.Count(); ++k)
            {
              if (Char.IsLetter(temp[k]))
              {
                tileValue += basicTileValues[temp[k]];
              }
            }
            if (temp.Count() > 1)
            {
              tileValue += 5;
            }
            if (temp.Count() == 3 && (temp[1] == '\\' || temp[1] == '/'))
            {
              tileValue = 20;
            }
            d.Text = tileValue.ToString();
          }
        }
      }
      button2_Click(sender, e);
      SendKeys.Send("{RIGHT}");
    }

    // Go
    private void button2_Click(object sender, EventArgs e)
    {
      ClearPathFormatting();
      listBox1.Items.Clear();
      string[] stringBoard = new string[dim * dim];
      foreach (Control c in tableLayoutPanel1.Controls)
      {
        string temp = c.Name;
        // textBoxXX; number begins at the 7th index.
        temp = temp.Substring(7);
        int i = Convert.ToInt32(temp);
        stringBoard[i - 1] = c.Text;
      }
      words.Clear();
      paths.Clear();
      points.Clear();
      wordPointPaths.Clear();

      try
      {
        words = WordamentRecursiveOOSolver.Solver.RunSolver(stringBoard, paths);
        ComputePoints(words, paths, points);
      }
      catch
      {
      }
      for (int i = 0; i < words.Count(); ++i)
      {
        for (int j = 0; j < words[i].Count(); ++j)
        {
          wordPointPaths.Add(new WordPointPath(words[i][j], points[i][j], paths[i][j]));
        }
      }
      // comboBoxes are zero-indexed with -1 representing no selection.
      //if(comboBox1.SelectedIndex = 0)
      // Default: alphabetical
      if (comboBox1.SelectedIndex == -1 || comboBox1.SelectedIndex == 0)
      {
        Display(wordPointPaths);
        return;
      }
      // points

      // What follows seems kind of disgusting.
      try
      {
        if (comboBox1.SelectedIndex == 1)
        {
          wordPointPaths.Sort(new WordPointPathComparer1());
          Display(wordPointPaths);
          return;
        }
        // word length
        if (comboBox1.SelectedIndex == 2)
        {
          wordPointPaths.Sort(new WordPointPathComparer2());
          Display(wordPointPaths);
          return;
        }
        // physical path length
        if (comboBox1.SelectedIndex == 3)
        {
          wordPointPaths.Sort(new WordPointPathComparer3());
          Display(wordPointPaths);
          return;
        }
        // points / word length
        if (comboBox1.SelectedIndex == 4)
        {
          wordPointPaths.Sort(new WordPointPathComparer4());
          Display(wordPointPaths);
          return;
        }
        // points / path length
        if (comboBox1.SelectedIndex == 5)
        {
          wordPointPaths.Sort(new WordPointPathComparer5());
          Display(wordPointPaths);
          return;
        }
        // start position by: points
        if (comboBox1.SelectedIndex == 6)
        {
          wordPointPaths.Sort(new WordPointPathComparer6());
          Display(wordPointPaths);
          return;
        }
        // start position by: word length
        if (comboBox1.SelectedIndex == 7)
        {
          wordPointPaths.Sort(new WordPointPathComparer7());
          Display(wordPointPaths);
          return;
        }
        // start position by: points / word length
        if (comboBox1.SelectedIndex == 8)
        {
          wordPointPaths.Sort(new WordPointPathComparer8());
          Display(wordPointPaths);
          return;
        }
        // start position by: points / path length
        if (comboBox1.SelectedIndex == 9)
        {
          wordPointPaths.Sort(new WordPointPathComparer9());
          Display(wordPointPaths);
          return;
        }
        // A, B, ... by: points
        if (comboBox1.SelectedIndex == 10)
        {
          wordPointPaths.Sort(new WordPointPathComparer10());
          Display(wordPointPaths);
          return;
        }
        // A, B, ... by: word length
        if (comboBox1.SelectedIndex == 11)
        {
          wordPointPaths.Sort(new WordPointPathComparer11());
          Display(wordPointPaths);
          return;
        }
        // A, B, ... by: points / word length
        if (comboBox1.SelectedIndex == 12)
        {
          wordPointPaths.Sort(new WordPointPathComparer12());
          Display(wordPointPaths);
          return;
        }
        // A, B, ... by: points / path length
        if (comboBox1.SelectedIndex == 13)
        {
          wordPointPaths.Sort(new WordPointPathComparer13());
          Display(wordPointPaths);
          return;
        }
        // speed round: word length ascending
        if (comboBox1.SelectedIndex == 14)
        {
          wordPointPaths.Sort(new WordPointPathComparer14());
          Display(wordPointPaths);
          return;
        }
        // speed round: start position by word length ascending
        if (comboBox1.SelectedIndex == 15)
        {
          wordPointPaths.Sort(new WordPointPathComparer15());
          Display(wordPointPaths);
          return;
        }
      }
      finally
      {
        SendKeys.Send("{RIGHT}");
        SendKeys.Send("{RIGHT}");
      }

    }

    // Ordering stuff
    // What follows seems kind of disgusting.
    #region Ordering stuff
    private class WordPointPath
    {
      public WordPointPath(string word, int points, List<int> path)
      {
        this.word = word;
        this.points = points;
        this.path = path;
      }
      public string word;
      public int points;
      public List<int> path;
    }

    // points
    private class WordPointPathComparer1 : IComparer<WordPointPath>
    {
      public int Compare(WordPointPath lhs, WordPointPath rhs)
      {
        if (lhs.points != rhs.points)
        {
          return rhs.points.CompareTo(lhs.points);
        }
        return lhs.word.CompareTo(rhs.word);
      }
    }

    // word length
    private class WordPointPathComparer2 : IComparer<WordPointPath>
    {
      public int Compare(WordPointPath lhs, WordPointPath rhs)
      {
        if (lhs.word.Count() != rhs.word.Count())
        {
          return rhs.word.Count().CompareTo(lhs.word.Count());
        }
        return lhs.word.CompareTo(rhs.word);
      }
    }

    // path length
    private class WordPointPathComparer3 : IComparer<WordPointPath>
    {
      public int Compare(WordPointPath lhs, WordPointPath rhs)
      {
        double lhsPathLength = PhysicalPathLength(lhs.path);
        double rhsPathLength = PhysicalPathLength(rhs.path);
        if (lhsPathLength != rhsPathLength)
        {
          return rhsPathLength.CompareTo(lhsPathLength);
        }
        return lhs.word.CompareTo(rhs.word);
      }
    }

    // points / word length
    private class WordPointPathComparer4 : IComparer<WordPointPath>
    {
      public int Compare(WordPointPath lhs, WordPointPath rhs)
      {
        double a = lhs.points / lhs.word.Count();
        double b = rhs.points / rhs.word.Count();
        if (a != b)
        {
          return b.CompareTo(a);
        }
        return lhs.word.CompareTo(rhs.word);
      }
    }

    // points / path length
    private class WordPointPathComparer5 : IComparer<WordPointPath>
    {
      public int Compare(WordPointPath lhs, WordPointPath rhs)
      {
        double a = lhs.points / PhysicalPathLength(lhs.path);
        double b = rhs.points / PhysicalPathLength(rhs.path);
        if (a != b)
        {
          return b.CompareTo(a);
        }
        return lhs.word.CompareTo(rhs.word);
      }
    }

    // start position by: points
    private class WordPointPathComparer6 : IComparer<WordPointPath>
    {
      public int Compare(WordPointPath lhs, WordPointPath rhs)
      {
        // Remember that paths are stored backwards
        int a = lhs.path[lhs.path.Count() - 1];
        int b = rhs.path[rhs.path.Count() - 1];
        if (a != b)
        {
          return a.CompareTo(b);
        }
        WordPointPathComparer1 temp = new WordPointPathComparer1();
        return temp.Compare(lhs, rhs);
      }
    }

    // start position by: word length
    private class WordPointPathComparer7 : IComparer<WordPointPath>
    {
      public int Compare(WordPointPath lhs, WordPointPath rhs)
      {
        // Remember that paths are stored backwards
        int a = lhs.path[lhs.path.Count() - 1];
        int b = rhs.path[rhs.path.Count() - 1];
        if (a != b)
        {
          return a.CompareTo(b);
        }
        WordPointPathComparer2 temp = new WordPointPathComparer2();
        return temp.Compare(lhs, rhs);
      }
    }

    // start position by: points / word length
    private class WordPointPathComparer8 : IComparer<WordPointPath>
    {
      public int Compare(WordPointPath lhs, WordPointPath rhs)
      {
        // Remember that paths are stored backwards
        int a = lhs.path[lhs.path.Count() - 1];
        int b = rhs.path[rhs.path.Count() - 1];
        if (a != b)
        {
          return a.CompareTo(b);
        }
        WordPointPathComparer4 temp = new WordPointPathComparer4();
        return temp.Compare(lhs, rhs);
      }
    }

    // start position by: points / path length
    private class WordPointPathComparer9 : IComparer<WordPointPath>
    {
      public int Compare(WordPointPath lhs, WordPointPath rhs)
      {
        // Remember that paths are stored backwards
        int a = lhs.path[lhs.path.Count() - 1];
        int b = rhs.path[rhs.path.Count() - 1];
        if (a != b)
        {
          return a.CompareTo(b);
        }
        WordPointPathComparer5 temp = new WordPointPathComparer5();
        return temp.Compare(lhs, rhs);
      }
    }

    // A, B, ... by: points
    private class WordPointPathComparer10 : IComparer<WordPointPath>
    {
      public int Compare(WordPointPath lhs, WordPointPath rhs)
      {
        int a = lhs.word[0];
        int b = rhs.word[0];
        if (a != b)
        {
          return a.CompareTo(b);
        }
        WordPointPathComparer1 temp = new WordPointPathComparer1();
        return temp.Compare(lhs, rhs);
      }
    }

    // A, B, ... by: word length
    private class WordPointPathComparer11 : IComparer<WordPointPath>
    {
      public int Compare(WordPointPath lhs, WordPointPath rhs)
      {
        int a = lhs.word[0];
        int b = rhs.word[0];
        if (a != b)
        {
          return a.CompareTo(b);
        }
        WordPointPathComparer2 temp = new WordPointPathComparer2();
        return temp.Compare(lhs, rhs);
      }
    }

    // A, B, ... by: points / word length
    private class WordPointPathComparer12 : IComparer<WordPointPath>
    {
      public int Compare(WordPointPath lhs, WordPointPath rhs)
      {
        int a = lhs.word[0];
        int b = rhs.word[0];
        if (a != b)
        {
          return a.CompareTo(b);
        }
        WordPointPathComparer4 temp = new WordPointPathComparer4();
        return temp.Compare(lhs, rhs);
      }
    }

    // A, B, ... by: points / path length
    private class WordPointPathComparer13 : IComparer<WordPointPath>
    {
      public int Compare(WordPointPath lhs, WordPointPath rhs)
      {
        int a = lhs.word[0];
        int b = rhs.word[0];
        if (a != b)
        {
          return a.CompareTo(b);
        }
        WordPointPathComparer5 temp = new WordPointPathComparer5();
        return temp.Compare(lhs, rhs);
      }
    }

    // speed round: word length ascending
    private class WordPointPathComparer14 : IComparer<WordPointPath>
    {
      public int Compare(WordPointPath lhs, WordPointPath rhs)
      {
        if (lhs.word.Count() != rhs.word.Count())
        {
          return lhs.word.Count().CompareTo(rhs.word.Count());
        }
        return lhs.word.CompareTo(rhs.word);
      }
    }

    // speed round: start position by word length ascending
    private class WordPointPathComparer15 : IComparer<WordPointPath>
    {
      public int Compare(WordPointPath lhs, WordPointPath rhs)
      {
        // Remember that paths are stored backwards
        int a = lhs.path[lhs.path.Count() - 1];
        int b = rhs.path[rhs.path.Count() - 1];
        if (a != b)
        {
          return a.CompareTo(b);
        }
        WordPointPathComparer14 temp = new WordPointPathComparer14();
        return temp.Compare(lhs, rhs);
      }
    }


    private static double PhysicalPathLength(List<int> path)
    {
      double pathLength = 0;
      for (int i = 0; i < path.Count() - 1; ++i)
      {
        int a = path[i];
        int b = path[i + 1];
        if (a + 1 == b || a - 1 == b || a + dim == b || a - dim == b)
        {
          pathLength += 1;
        }
        // diagonal
        else
        {
          pathLength += 1.41421356237;
        }
      }
      return pathLength;
    }
    #endregion

    private void ComputePoints(List<List<string>> words, List<List<List<int>>> paths, List<List<int>> points)
    {
      // Generate points array
      int[] pointsBoard = new int[dim * dim];
      foreach (Control c in tableLayoutPanel2.Controls)
      {
        string temp = c.Name;
        temp = temp.Substring(7);
        // In a 4x4 board, textBox17 is the first textBox in the Tile Points tableLayoutPanel.
        // The path itself is zero-indexed, so subtract dim^2 - 1 to adjust.
        int i = Convert.ToInt32(temp) - (dim * dim + 1);
        try
        {
          pointsBoard[i] = Convert.ToInt32(c.Text);
        }
        catch
        {
          pointsBoard[i] = 0;
        }
      }
      for (int i = 0; i < words.Count(); ++i)
      {
        points.Add(new List<int>());
        for (int j = 0; j < words[i].Count(); ++j)
        {
          points[i].Add(ComputePointsHelper(words[i][j].Count(), paths[i][j], pointsBoard));
        }
      }
    }

    private int ComputePointsHelper(int wordLength, List<int> path, int[] pointsArray)
    {
      int ret = 0;
      for (int i = 0; i < path.Count(); ++i)
      {
        ret += pointsArray[path[i]];
      }
      if (wordLength >= 8)
      {
        ret = (int)(2.5 * ret);
      }
      else if (wordLength >= 6)
      {
        ret = 2 * ret;
      }
      else if (wordLength >= 5)
      {
        ret = (int)(1.5 * ret);
      }
      // Only words designated as 'common' get a bonus of 5 points for using the digram tile.
      // No way for me currently to determine what's common.
      /*
      if (WordamentRecursiveOOSolver.Solver.digram && (wordLength > path.Count()))
      {
        ret += 5;
      }
       */
      return ret;
    }

    private void Display(List<WordPointPath> wordPointPairs)
    {
      int totalPoints = 0;
      for (int i = 0; i < wordPointPairs.Count(); ++i)
      {
        totalPoints += wordPointPairs[i].points;
        listBox1.Items.Add(wordPointPairs[i].points + " " + wordPointPairs[i].word);
      }
      label6.Text = "Total points: " + totalPoints;
      label7.Text = "Words found: " + wordPointPairs.Count();
    }

    private void ClearPathFormatting()
    {
      label9.Text = "[]";
      foreach (Control c in tableLayoutPanel1.Controls)
      {
        string[] temp = c.Text.Split('|');
        c.Text = temp[temp.Count() - 1];
        c.Font = new Font(c.Font, FontStyle.Regular);
        c.BackColor = Color.White;
      }
    }

    private List<Color> CreateColorGradient(Color a, Color b, int steps)
    {
      int rMax = a.R;
      int rMin = b.R;
      int gMax = a.G;
      int gMin = b.G;
      int bMax = a.B;
      int bMin = b.B;
      List<Color> colorGradient = new List<Color>();
      int denominator = steps - 1;
      if (denominator == 0)
      {
        denominator = 1;
      }
      for (int i = 0; i < steps; ++i)
      {
        int rAverage = rMin + (int)((rMax - rMin) * i / (denominator));
        int gAverage = gMin + (int)((gMax - gMin) * i / (denominator));
        int bAverage = bMin + (int)((bMax - bMin) * i / (denominator));
        colorGradient.Add(Color.FromArgb(rAverage, gAverage, bAverage));
      }
      return colorGradient;
    }

    // Displaying paths
    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      ClearPathFormatting();
      string word = wordPointPaths[listBox1.SelectedIndex].word;
      label9.Text = "[" + word + "]";
      List<int> path = wordPointPaths[listBox1.SelectedIndex].path;
      // How to do this efficiently? Is it possible? (it's not necessary).
      List<Color> colorGradient = CreateColorGradient(Color.LightGreen, Color.Tomato, path.Count());
      for (int i = path.Count() - 1; i >= 0; --i)
      {
        int boxIndex = path[i];
        foreach (Control c in tableLayoutPanel1.Controls)
        {
          string temp = c.Name;
          temp = temp.Substring(7);
          int trialBoxIndex = Convert.ToInt32(temp) - 1;
          if (boxIndex == trialBoxIndex)
          {
            c.Font = new Font(c.Font, FontStyle.Bold);
            c.BackColor = colorGradient[i];
            c.Text = (path.Count() - i) + "|" + c.Text;
            break;
          }
        }
      }
    }

    // Saving a board...
    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SaveFileDialog dialog = new SaveFileDialog();
      dialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
      dialog.FilterIndex = 1;
      if (dialog.ShowDialog() == DialogResult.OK)
      {
        ClearPathFormatting();
        using (StreamWriter writer = new StreamWriter(dialog.FileName.ToString()))
        {
          string[] stringBoard = new string[dim * dim];
          string[] pointsBoard = new string[dim * dim];
          foreach (Control c in tableLayoutPanel1.Controls)
          {
            string temp = c.Name;
            // textBoxXX; number begins at the 7th index.
            temp = temp.Substring(7);
            int i = Convert.ToInt32(temp);
            stringBoard[i - 1] = c.Text;
          }
          for (int i = 0; i < dim * dim; ++i)
          {
            writer.WriteLine(stringBoard[i]);
          }
          foreach (Control c in tableLayoutPanel2.Controls)
          {
            string temp = c.Name;
            temp = temp.Substring(7);
            // In a 4x4 board, textBox17 is the first textBox in the Tile Points tableLayoutPanel.
            // The path itself is zero-indexed, so subtract dim^2 - 1 to adjust.
            int i = Convert.ToInt32(temp) - (dim * dim + 1);
            pointsBoard[i] = c.Text;
          }
          for (int i = 0; i < dim * dim; ++i)
          {
            writer.WriteLine(pointsBoard[i]);
          }
        }
      }
    }

    // Loading a board...
    private void loadToolStripMenuItem_Click(object sender, EventArgs e)
    {
      OpenFileDialog dialog = new OpenFileDialog();
      dialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
      dialog.FilterIndex = 1;
      if (dialog.ShowDialog() == DialogResult.OK)
      {
        ClearPathFormatting();
        listBox1.Items.Clear();
        using (StreamReader reader = new StreamReader(dialog.FileName.ToString()))
        {
          string[] stringBoard = new string[dim * dim];
          string[] pointBoard = new string[dim * dim];
          for (int i = 0; i < dim * dim; ++i)
          {
            stringBoard[i] = reader.ReadLine();
          }
          for (int i = 0; i < dim * dim; ++i)
          {
            pointBoard[i] = reader.ReadLine();
          }
          foreach (Control c in tableLayoutPanel1.Controls)
          {
            string temp = c.Name;
            // textBoxXX; number begins at the 7th index.
            temp = temp.Substring(7);
            int i = Convert.ToInt32(temp) - 1;
            c.Text = stringBoard[i];
          }
          foreach (Control c in tableLayoutPanel2.Controls)
          {
            string temp = c.Name;
            temp = temp.Substring(7);
            // In a 4x4 board, textBox17 is the first textBox in the Tile Points tableLayoutPanel.
            // The path itself is zero-indexed, so subtract dim^2 - 1 to adjust.
            int i = Convert.ToInt32(temp) - (dim * dim + 1);
            c.Text = pointBoard[i];
          }
        }
      }
    }
  }
}
