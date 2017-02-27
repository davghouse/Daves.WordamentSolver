using System;
using System.Collections.Generic;

namespace Daves.WordamentSolver.WordSorters
{
    public class StartLetterByPointsOverPathLengthSorter : WordSorter, IComparer<Word>
    {
        public override string Name => "start letter by: points / path length";

        public virtual int Compare(Word x, Word y)
            => x.StartLetter != y.StartLetter
            ? x.StartLetter.CompareTo(y.StartLetter)
            : WordSorter.PointsOverPathLength.Compare(x, y);

        public override void Sort(Word[] words)
            => Array.Sort(words, comparer: this);
    }
}
