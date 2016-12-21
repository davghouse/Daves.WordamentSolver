using System;
using System.Collections.Generic;

namespace WordamentSolver.Models.WordSorters
{
    public sealed class AlphabetSorter : WordSorter, IComparer<Word>
    {
        public override string Name => "alphabet";

        public int Compare(Word x, Word y)
            => x.String.CompareTo(y.String);

        public override void Sort(Word[] words)
            => Array.Sort(words, comparer: this);
    }
}
