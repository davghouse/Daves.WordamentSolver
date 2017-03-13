using System;
using System.Collections.Generic;

namespace Daves.WordamentSolver.WordSorters
{
    public class StartPositionByWordLengthAscendingSorter : WordSorter, IComparer<Word>
    {
        public override string Name => "start position by: word length ascending";

        public virtual int Compare(Word x, Word y)
            => x.BestPathStartPosition != y.BestPathStartPosition
            ? x.BestPathStartPosition.CompareTo(y.BestPathStartPosition)
            : WordSorter.WordLengthAscending.Compare(x, y);

        public override void Sort(Word[] words)
            => Array.Sort(words, comparer: this);
    }
}
