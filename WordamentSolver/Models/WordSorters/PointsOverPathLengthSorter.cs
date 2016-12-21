using System;
using System.Collections.Generic;

namespace WordamentSolver.Models.WordSorters
{
    public sealed class PointsOverPathLengthSorter : WordSorter, IComparer<Word>
    {
        public override string Name => "points / path length";

        public int Compare(Word x, Word y)
            => x.PointsOverPathLength != y.PointsOverPathLength
            ? y.PointsOverPathLength.CompareTo(x.PointsOverPathLength)
            : WordSorter.Alphabet.Compare(x, y);

        public override void Sort(Word[] words)
            => Array.Sort(words, comparer: this);
    }
}
