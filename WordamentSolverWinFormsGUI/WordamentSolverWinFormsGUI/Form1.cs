﻿// This is my first C# program... I was just here to learn... Be careful...

﻿using System;
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
    public Form1()
    {
      InitializeComponent();
      comboBox1.SelectedIndex = 1;
    }

    // Allow for faster tabbing via space and clicking via enter.
    #region Remapping keys

    // 'Tile Strings' and 'Tile Points' textBoxes.
    private void textBox_KeyDown(object sender, KeyEventArgs e)
    {
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

    // 'Words' listBox.
    private void ListBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Space)
      {
        e.SuppressKeyPress = true;
        SendKeys.Send("{TAB}");
      }
    }

    // 'Order by:' comboBox.
    private void comboBox1_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Space)
      {
        e.SuppressKeyPress = true;
        SendKeys.Send("{TAB}");
      }
    }

    // 'Go w/ TP guess' button.
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

    // 'Go' button.
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

    // 'Clear' button.
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
    
    #endregion

    // Clear the boards to get ready for a new game.
    #region Clearing boards

    // 'Clear Boards' button.
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

    // 'Clear Path' button.
    private void button4_Click(object sender, EventArgs e)
    {
      ClearPathFormatting();
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

    #endregion

    // Go buttons.
    #region Going

    // 'Go w/ TP guess' button.
    private void button1_Click(object sender, EventArgs e)
    {
      ClearPathFormatting();
      // A zip operation could be used here; would like to traverse tableLayoutPanel1's and tableLayoutPanel2's controls simultaneously.
      int i = 0;
      foreach (Control c in tableLayoutPanel1.Controls)
      {
        int j = 0;
        foreach (Control d in tableLayoutPanel2.Controls)
        {
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
            // Special tile type.
            if (temp.Count() > 1)
            {
              tileValue += 5;
            }
            // Either/or tile.
            if (temp.Count() == 3 && (temp[1] == '\\' || temp[1] == '/'))
            {
              tileValue = 20;
            }
            d.Text = tileValue.ToString();
            break;
          }
          ++j;
        }
        ++i;
      }
      // Go after guessing at Tile Points values.
      button2_Click(sender, e);
      SendKeys.Send("{RIGHT}");
    }

    // 'Go' button.
    private void button2_Click(object sender, EventArgs e)
    {
      // Clean up.
      comboBox1.TabStop = false;
      ClearPathFormatting();
      listBox1.Items.Clear();
      words.Clear();
      paths.Clear();
      points.Clear();
      wordPointPaths.Clear();
      string[] stringBoard = new string[dim * dim];
      int boardPos = 0;
      foreach (Control c in tableLayoutPanel1.Controls)
      {
        stringBoard[boardPos++] = c.Text;
      }
      // Solve, and find point values.
      try
      {
        words = WordamentRecursiveOOSolver.Solver.RunSolver(stringBoard, paths);
        ComputePoints(words, paths, points);
      }
      catch
      {
        // If there's a problem, it's hopefully with what was input by the user. Let them figure it out.
      }
      // Fill this out for use in sorting.
      for (int i = 0; i < words.Count(); ++i)
      {
        for (int j = 0; j < words[i].Count(); ++j)
        {
          wordPointPaths.Add(new WordPointPath(words[i][j], points[i][j], paths[i][j]));
        }
      }
      // Sort.
      try
      {
        // Good place to use reflection?
        switch (comboBox1.SelectedIndex)
        {
          case 1:
            wordPointPaths.Sort(new WordPointPathComparer1());
            break;
          case 2:
            wordPointPaths.Sort(new WordPointPathComparer2());
            break;
          case 3:
            wordPointPaths.Sort(new WordPointPathComparer3());
            break;
          case 4:
            wordPointPaths.Sort(new WordPointPathComparer4());
            break;
          case 5:
            wordPointPaths.Sort(new WordPointPathComparer5());
            break;
          case 6:
            wordPointPaths.Sort(new WordPointPathComparer6());
            break;
          case 7:
            wordPointPaths.Sort(new WordPointPathComparer7());
            break;
          case 8:
            wordPointPaths.Sort(new WordPointPathComparer8());
            break;
          case 9:
            wordPointPaths.Sort(new WordPointPathComparer9());
            break;
          case 10:
            wordPointPaths.Sort(new WordPointPathComparer10());
            break;
          case 11:
            wordPointPaths.Sort(new WordPointPathComparer11());
            break;
          case 12:
            wordPointPaths.Sort(new WordPointPathComparer12());
            break;
          case 13:
            wordPointPaths.Sort(new WordPointPathComparer13());
            break;
          case 14:
            wordPointPaths.Sort(new WordPointPathComparer14());
            break;
          case 15:
            wordPointPaths.Sort(new WordPointPathComparer15());
            break;
          default:
            break;
        }
        Display(wordPointPaths);
        return;
      }
      finally
      {
        // After pressing Go, make sure the first item in ListBox1 is selected.
        SendKeys.Send("{RIGHT}");
        SendKeys.Send("{RIGHT}");
      }
    }
    
    #endregion

    // WordPointPath class and IComparers to help with the different sorts.
    #region Ordering

    // Necessary as a way to make sorting easier.
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

    // Points.
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

    // Word length.
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

    // Path length.
    private class WordPointPathComparer3 : IComparer<WordPointPath>
    {
      public int Compare(WordPointPath lhs, WordPointPath rhs)
      {
        double a = PhysicalPathLength(lhs.path);
        double b = PhysicalPathLength(rhs.path);
        if (a != b)
        {
          return b.CompareTo(a);
        }
        return lhs.word.CompareTo(rhs.word);
      }
    }

    // Points / word length.
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

    // Points / path length.
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

    // Paths are stored backwards, hence looking at the final element in the following IComparers.
    // Start position by: points.
    private class WordPointPathComparer6 : IComparer<WordPointPath>
    {
      public int Compare(WordPointPath lhs, WordPointPath rhs)
      {
        int a = lhs.path[lhs.path.Count() - 1];
        int b = rhs.path[rhs.path.Count() - 1];
        if (a != b)
        {
          return a.CompareTo(b);
        }
        return (new WordPointPathComparer1()).Compare(lhs, rhs);
      }
    }

    // Start position by: word length.
    private class WordPointPathComparer7 : IComparer<WordPointPath>
    {
      public int Compare(WordPointPath lhs, WordPointPath rhs)
      {
        int a = lhs.path[lhs.path.Count() - 1];
        int b = rhs.path[rhs.path.Count() - 1];
        if (a != b)
        {
          return a.CompareTo(b);
        }
        return (new WordPointPathComparer2()).Compare(lhs, rhs);
      }
    }

    // Start position by: points / word length.
    private class WordPointPathComparer8 : IComparer<WordPointPath>
    {
      public int Compare(WordPointPath lhs, WordPointPath rhs)
      {
        int a = lhs.path[lhs.path.Count() - 1];
        int b = rhs.path[rhs.path.Count() - 1];
        if (a != b)
        {
          return a.CompareTo(b);
        }
        return (new WordPointPathComparer4()).Compare(lhs, rhs);
      }
    }

    // Start position by: points / path length.
    private class WordPointPathComparer9 : IComparer<WordPointPath>
    {
      public int Compare(WordPointPath lhs, WordPointPath rhs)
      {
        int a = lhs.path[lhs.path.Count() - 1];
        int b = rhs.path[rhs.path.Count() - 1];
        if (a != b)
        {
          return a.CompareTo(b);
        }
        return (new WordPointPathComparer5()).Compare(lhs, rhs);
      }
    }

    // A, B, ... by: points.
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
        return (new WordPointPathComparer1()).Compare(lhs, rhs);
      }
    }

    // A, B, ... by: word length.
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
        return (new WordPointPathComparer2()).Compare(lhs, rhs);
      }
    }

    // A, B, ... by: points / word length.
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
        return (new WordPointPathComparer4()).Compare(lhs, rhs);
      }
    }

    // A, B, ... by: points / path length.
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
        return (new WordPointPathComparer5()).Compare(lhs, rhs);
      }
    }

    // Speed round: word length ascending.
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

    // Speed round: start position by word length ascending.
    private class WordPointPathComparer15 : IComparer<WordPointPath>
    {
      public int Compare(WordPointPath lhs, WordPointPath rhs)
      {
        int a = lhs.path[lhs.path.Count() - 1];
        int b = rhs.path[rhs.path.Count() - 1];
        if (a != b)
        {
          return a.CompareTo(b);
        }
        return (new WordPointPathComparer14()).Compare(lhs, rhs);
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

    // Compute points.
    #region Computing points

    private void ComputePoints(List<List<string>> words, List<List<List<int>>> paths, List<List<int>> points)
    {
      int[] pointsBoard = new int[dim * dim];
      int boardPos = 0;
      foreach (Control c in tableLayoutPanel2.Controls)
      {
        try
        {
          pointsBoard[boardPos] = Convert.ToInt32(c.Text);
        }
        catch
        {
          pointsBoard[boardPos] = 0;
        }
        finally
        {
          ++boardPos;
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

    private int ComputePointsHelper(int wordLength, List<int> path, int[] pointsBoard)
    {
      int ret = 0;
      for (int i = 0; i < path.Count(); ++i)
      {
        ret += pointsBoard[path[i]];
      }
      if (wordLength >= 8)
      {
        return (int)(2.5 * ret);
      }
      if (wordLength >= 6)
      {
        return 2 * ret;
      }
      if (wordLength >= 5)
      {
        return (int)(1.5 * ret);
      }
      return ret;
    }

    #endregion

    // Display boards and paths.
    #region Displaying

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

    // Displaying paths.
    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      ClearPathFormatting();
      string word = wordPointPaths[listBox1.SelectedIndex].word;
      label9.Text = "[" + word + "]";
      List<int> path = wordPointPaths[listBox1.SelectedIndex].path;
      List<Color> colorGradient = CreateColorGradient(Color.LightGreen, Color.Tomato, path.Count());
      for (int i = path.Count() - 1; i >= 0; --i)
      {
        int boxIndex = path[i];
        int trialBoxIndex = 0;
        foreach (Control c in tableLayoutPanel1.Controls)
        {
          if (boxIndex == trialBoxIndex)
          {
            c.Font = new Font(c.Font, FontStyle.Bold);
            c.BackColor = colorGradient[i];
            c.Text = (path.Count() - i) + "|" + c.Text;
            break;
          }
          ++trialBoxIndex;
        }
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
    
    #endregion

    // Save and load boards.
    #region Saving and loading

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
          int counter = 0;
          foreach (Control c in tableLayoutPanel1.Controls)
          {
            stringBoard[counter++] = c.Text;
          }
          for (int i = 0; i < dim * dim; ++i)
          {
            writer.WriteLine(stringBoard[i]);
          }
          counter = 0;
          foreach (Control c in tableLayoutPanel2.Controls)
          {
            pointsBoard[counter++] = c.Text;
          }
          for (int i = 0; i < dim * dim; ++i)
          {
            writer.WriteLine(pointsBoard[i]);
          }
        }
      }
    }

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
          int counter = 0;
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
            c.Text = stringBoard[counter++];
          }
          counter = 0;
          foreach (Control c in tableLayoutPanel2.Controls)
          {
            c.Text = pointBoard[counter++];
          }
        }
      }
    }

    #endregion

    // Fields.
    #region Fields

    private const int dim = 4;

    // List<string>s within the List<List<string>> contain strings beginning with the same letter.
    private List<List<string>> words = new List<List<string>>();

    // Each string has a corresponding List<int>; its path through the board.
    private List<List<List<int>>> paths = new List<List<List<int>>>();

    // Each string has a corresponding int; its point value.
    private List<List<int>> points = new List<List<int>>();

    // Necessary as a way to make sorting easier. 
    private List<WordPointPath> wordPointPaths = new List<WordPointPath>();

    // May or may not be correct.
    private static Dictionary<char, int> basicTileValues = new Dictionary<char, int>
                                               {{'A', 2}, {'B', 5}, {'C', 3}, {'D', 3}, {'E', 1}, {'F', 5}, {'G', 4}, {'H', 4}, {'I', 2}, 
                                               {'J', 10}, {'K', 6}, {'L', 3}, {'M', 4}, {'N', 2}, {'O', 2}, {'P', 4}, {'Q', 8}, 
                                               {'R', 2}, {'S', 2}, {'T', 2}, {'U', 4}, {'V', 6}, {'W', 6}, {'X', 9}, {'Y', 5}, {'Z', 8}};

    #endregion
  }
}
