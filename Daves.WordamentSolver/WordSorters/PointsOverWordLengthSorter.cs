using System;
using System.Collections.Generic;

namespace Daves.WordamentSolver.WordSorters
{
    public class PointsOverWordLengthSorter : WordSorter, IComparer<Word>
    {
        public override string Name => "points / word length";

        public virtual int Compare(Word x, Word y)
            => x.BestPathPointsOverWordLength != y.BestPathPointsOverWordLength
            ? y.BestPathPointsOverWordLength.CompareTo(x.BestPathPointsOverWordLength)
            : WordSorter.Alphabet.Compare(x, y);

        public override void Sort(Word[] words)
            => Array.Sort(words, comparer: this);
    }
}
