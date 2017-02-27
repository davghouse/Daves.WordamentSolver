using System;
using System.Collections.Generic;
using System.Linq;

namespace Daves.WordamentSolver.Models
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

        // TODO: Maintain this if it becomes possible to make a move to tiles beyond the adjacent ones.
        private double ComputePathLength()
        {
            double pathLength = 0;
            for (int i = 1; i < Path.Count; ++i)
            {
                Tile currentTile = Path[i - 1];
                Tile nextTile = Path[i];

                if (currentTile.Row == nextTile.Row
                    || currentTile.Column == nextTile.Column)
                {
                    pathLength += 1; // Next is right, left, down, or up from current.
                }
                else
                {
                    pathLength += Math.Sqrt(2); // Next is diagonal from current.
                }
            }

            return pathLength;
        }

        public override bool Equals(object other)
            => Equals(other as Word);

        public bool Equals(Word other)
            => String.Equals(other?.String, StringComparison.Ordinal);

        public override int GetHashCode()
            => StringComparer.Ordinal.GetHashCode(String);
    }
}
