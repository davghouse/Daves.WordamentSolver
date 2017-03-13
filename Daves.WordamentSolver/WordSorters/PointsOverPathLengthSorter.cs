using System;
using System.Collections.Generic;

namespace Daves.WordamentSolver.WordSorters
{
    public class PointsOverPathLengthSorter : WordSorter, IComparer<Word>
    {
        public override string Name => "points / path length";

        public virtual int Compare(Word x, Word y)
            => x.BestPathPointsOverPathLength != y.BestPathPointsOverPathLength
            ? y.BestPathPointsOverPathLength.CompareTo(x.BestPathPointsOverPathLength)
            : WordSorter.Alphabet.Compare(x, y);

        public override void Sort(Word[] words)
            => Array.Sort(words, comparer: this);
    }
}
