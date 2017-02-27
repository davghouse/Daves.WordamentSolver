using System;
using System.Collections.Generic;

namespace Daves.WordamentSolver.WordSorters
{
    public class PathLengthSorter : WordSorter, IComparer<Word>
    {
        public override string Name => "path length";

        public virtual int Compare(Word x, Word y)
            => x.PathLength != y.PathLength
            ? y.PathLength.CompareTo(x.PathLength)
            : WordSorter.Alphabet.Compare(x, y);

        public override void Sort(Word[] words)
            => Array.Sort(words, comparer: this);
    }
}
