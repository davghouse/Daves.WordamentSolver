using System;
using System.Collections.Generic;

namespace Daves.WordamentSolver.WordSorters
{
    public class PointsSorter : WordSorter, IComparer<Word>
    {
        public override string Name => "points";

        public virtual int Compare(Word x, Word y)
            => x.BestPathPoints != y.BestPathPoints
            ? y.BestPathPoints.CompareTo(x.BestPathPoints)
            : WordSorter.Alphabet.Compare(x, y);

        public override void Sort(Word[] words)
            => Array.Sort(words, comparer: this);
    }
}
