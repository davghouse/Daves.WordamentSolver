using System;
using System.Collections.Generic;

namespace Daves.WordamentSolver.Models.WordSorters
{
    public sealed class StartPositionByPointsSorter : WordSorter, IComparer<Word>
    {
        public override string Name => "start position by: points";

        public int Compare(Word x, Word y)
            => x.StartPosition != y.StartPosition
            ? x.StartPosition.CompareTo(y.StartPosition)
            : WordSorter.Points.Compare(x, y);

        public override void Sort(Word[] words)
            => Array.Sort(words, comparer: this);
    }
}
