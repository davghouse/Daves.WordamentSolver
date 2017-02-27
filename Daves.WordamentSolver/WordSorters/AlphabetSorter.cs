using System;
using System.Collections.Generic;

namespace Daves.WordamentSolver.WordSorters
{
    public class AlphabetSorter : WordSorter, IComparer<Word>
    {
        public override string Name => "alphabet";

        public virtual int Compare(Word x, Word y)
            => x.String.CompareTo(y.String);

        public override void Sort(Word[] words)
            => Array.Sort(words, comparer: this);
    }
}
