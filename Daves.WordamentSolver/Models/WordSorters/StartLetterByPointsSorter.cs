using System;
using System.Collections.Generic;

namespace Daves.WordamentSolver.Models.WordSorters
{
    public sealed class StartLetterByPointsSorter : WordSorter, IComparer<Word>
    {
        public override string Name => "start letter by: points";

        public int Compare(Word x, Word y)
            => x.StartLetter != y.StartLetter
            ? x.StartLetter.CompareTo(y.StartLetter)
            : WordSorter.Points.Compare(x, y);

        public override void Sort(Word[] words)
            => Array.Sort(words, comparer: this);
    }
}
