namespace WordamentSolver.Models.WordComparers
{
    public sealed class PathLengthComparer : WordComparer
    {
        public override string Name => "path length (p.l.)";

        public override int Compare(Word x, Word y)
            => x.PathLength != y.PathLength
            ? y.PathLength.CompareTo(x.PathLength)
            : WordComparer.Alphabet.Compare(x, y);
    }
}
