using System;
using System.Collections.Generic;

namespace Daves.WordamentSolver.WordSorters
{
    public class WordLengthSorter : WordSorter, IComparer<Word>
    {
        public override string Name => "word length";

        public virtual int Compare(Word x, Word y)
            => x.Length != y.Length
            ? y.Length.CompareTo(x.Length)
            : WordSorter.Alphabet.Compare(x, y);

        public override void Sort(Word[] words)
            => Array.Sort(words, comparer: this);
    }
}
