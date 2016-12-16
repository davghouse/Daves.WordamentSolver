using System;
using System.Collections.Generic;
using System.Linq;

namespace WordamentSolver.Models
{
    public sealed class Word : IEquatable<Word>
    {
        public Word(string @string, IEnumerable<Tile> path)
        {
            String = @string;
            Path = path.ToArray();
            Points = ComputePoints();
        }

        public string String { get; }
        public IReadOnlyList<Tile> Path { get; }

        public int Points { get; }
        private int ComputePoints()
        {
            int points = Path.Sum(t => t.Points) ?? 0;

            if (WordLength >= 8) return (int)(2.5 * points);
            if (WordLength >= 6) return 2 * points;
            if (WordLength >= 5) return (int)(1.5 * points);
            return points;
        }

        public int WordLength => String.Length;
        public double PointsOverWordLength => Points / WordLength;
        public double PointsOverPathLength => Points / PathLength;
        public int StartPosition => Path[0].Position;
        public char StartLetter => String[0];

        private double? _pathLength;
        public double PathLength
        {
            get
            {
                if (!_pathLength.HasValue)
                {
                    _pathLength = ComputePathLength();
                }

                return _pathLength.Value;
            }
        }

        private double ComputePathLength()
        {
            double pathLength = 0;
            for (int i = 1; i < Path.Count; ++i)
            {
                int currentPosition = Path[i - 1].Position;
                int nextPosition = Path[i].Position;

                if (currentPosition + 1 == nextPosition
                    || currentPosition - 1 == nextPosition
                    || currentPosition + 4 == nextPosition
                    || currentPosition - 4 == nextPosition)
                {
                    pathLength += 1; // Next is right, left, down, or up from current.
                }
                else
                {
                    pathLength += 1.41421356237; // Next is diagonal from current.
                }
            }

            return pathLength;
        }

        public override bool Equals(object other)
            => Equals(other as Word);

        public bool Equals(Word other)
            => String.Equals(other?.String, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode()
            => StringComparer.OrdinalIgnoreCase.GetHashCode(String);
    }
}
