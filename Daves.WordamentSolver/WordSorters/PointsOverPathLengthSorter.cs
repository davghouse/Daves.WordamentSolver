using System;
using System.Collections.Generic;

namespace Daves.WordamentSolver.WordSorters
{
    public class PointsOverPathLengthSorter : WordSorter, IComparer<Word>
    {
        public override string Name => "points / path length";

        public virtual int Compare(Word x, Word y)
            => x.PointsOverPathLength != y.PointsOverPathLength
            ? y.PointsOverPathLength.CompareTo(x.PointsOverPathLength)
            : WordSorter.Alphabet.Compare(x, y);

        public override void Sort(Word[] words)
            => Array.Sort(words, comparer: this);
    }
}
