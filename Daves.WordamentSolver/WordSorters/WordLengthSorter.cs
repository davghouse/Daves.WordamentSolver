using System;
using System.Collections.Generic;

namespace Daves.WordamentSolver.WordSorters
{
    public class WordLengthSorter : WordSorter, IComparer<Word>
    {
        public override string Name => "word length";

        public virtual int Compare(Word x, Word y)
            => x.WordLength != y.WordLength
            ? y.WordLength.CompareTo(x.WordLength)
            : WordSorter.Alphabet.Compare(x, y);

        public override void Sort(Word[] words)
            => Array.Sort(words, comparer: this);
    }
}
