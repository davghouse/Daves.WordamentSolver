namespace WordamentSolver.Models.WordComparers
{
    public sealed class StartLetterByPointsOverPathLengthComparer : WordComparer
    {
        public override string Name => "start letter by: points / p.l.";

        public override int Compare(Word x, Word y)
            => x.StartLetter != y.StartLetter
            ? x.StartLetter.CompareTo(y.StartLetter)
            : WordComparer.PointsOverPathLength.Compare(x, y);
    }
}
