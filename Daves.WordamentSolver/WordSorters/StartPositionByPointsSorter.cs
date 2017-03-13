using System;
using System.Collections.Generic;

namespace Daves.WordamentSolver.WordSorters
{
    public class StartPositionByPointsSorter : WordSorter, IComparer<Word>
    {
        public override string Name => "start position by: points";

        public virtual int Compare(Word x, Word y)
            => x.BestPathStartPosition != y.BestPathStartPosition
            ? x.BestPathStartPosition.CompareTo(y.BestPathStartPosition)
            : WordSorter.Points.Compare(x, y);

        public override void Sort(Word[] words)
            => Array.Sort(words, comparer: this);
    }
}
