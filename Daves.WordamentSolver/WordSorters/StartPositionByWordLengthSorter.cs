using System;
using System.Collections.Generic;

namespace Daves.WordamentSolver.WordSorters
{
    public class StartPositionByWordLengthSorter : WordSorter, IComparer<Word>
    {
        public override string Name => "start position by: word length";

        public virtual int Compare(Word x, Word y)
            => x.BestPathStartPosition != y.BestPathStartPosition
            ? x.BestPathStartPosition.CompareTo(y.BestPathStartPosition)
            : WordSorter.WordLength.Compare(x, y);

        public override void Sort(Word[] words)
            => Array.Sort(words, comparer: this);
    }
}
