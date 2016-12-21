using System;
using System.Collections.Generic;

namespace WordamentSolver.Models.WordSorters
{
    public sealed class PointsOverWordLengthSorter : WordSorter, IComparer<Word>
    {
        public override string Name => "points / word length";

        public int Compare(Word x, Word y)
            => x.PointsOverWordLength != y.PointsOverWordLength
            ? y.PointsOverWordLength.CompareTo(x.PointsOverWordLength)
            : WordSorter.Alphabet.Compare(x, y);

        public override void Sort(Word[] words)
            => Array.Sort(words, comparer: this);
    }
}
