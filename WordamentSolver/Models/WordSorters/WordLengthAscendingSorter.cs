using System;
using System.Collections.Generic;

namespace WordamentSolver.Models.WordSorters
{
    public sealed class WordLengthAscendingSorter : WordSorter, IComparer<Word>
    {
        public override string Name => "word length ascending";

        public int Compare(Word x, Word y)
            => x.WordLength != y.WordLength
            ? x.WordLength.CompareTo(y.WordLength)
            : WordSorter.Alphabet.Compare(x, y);

        public override void Sort(Word[] words)
            => Array.Sort(words, comparer: this);
    }
}
