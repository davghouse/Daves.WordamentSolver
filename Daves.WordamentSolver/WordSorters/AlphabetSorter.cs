using System;
using System.Collections.Generic;

namespace Daves.WordamentSolver.WordSorters
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
