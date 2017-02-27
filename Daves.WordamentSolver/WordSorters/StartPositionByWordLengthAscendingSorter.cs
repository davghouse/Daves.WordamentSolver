using System;
using System.Collections.Generic;

namespace Daves.WordamentSolver.WordSorters
{
    public sealed class StartPositionByWordLengthAscendingSorter : WordSorter, IComparer<Word>
    {
        public override string Name => "start position by: word length ascending";

        public int Compare(Word x, Word y)
            => x.StartPosition != y.StartPosition
            ? x.StartPosition.CompareTo(y.StartPosition)
            : WordSorter.WordLengthAscending.Compare(x, y);

        public override void Sort(Word[] words)
            => Array.Sort(words, comparer: this);
    }
}
