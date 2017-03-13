using System;
using System.Collections.Generic;

namespace Daves.WordamentSolver.WordSorters
{
    public class StartPositionByPointsOverWordLengthSorter : WordSorter, IComparer<Word>
    {
        public override string Name => "start position by: points / word length";

        public virtual int Compare(Word x, Word y)
            => x.BestPathStartPosition != y.BestPathStartPosition
            ? x.BestPathStartPosition.CompareTo(y.BestPathStartPosition)
            : WordSorter.PointsOverWordLength.Compare(x, y);

        public override void Sort(Word[] words)
            => Array.Sort(words, comparer: this);
    }
}
