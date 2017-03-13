using System;
using System.Collections.Generic;
using System.Linq;

namespace Daves.WordamentSolver
{
    // A word corresponds to a string. A word can be created through multiple paths, one being the best (most points).
    public class Word
    {
        protected internal Word(string @string)
            => String = @string;

        public string String { get; }
        public int Length => String.Length;
        public char StartLetter => String[0];

        protected HashSet<Path> _paths = new HashSet<Path>();
        public IReadOnlyCollection<Path> Paths => _paths;
        protected internal virtual bool AddPath(Path path)
        {
            if (_paths.Add(path))
            {
                int points = GetPoints(path);

                if (BestPath == null || points > BestPathPoints)
                {
                    BestPath = path;
                    BestPathPoints = points;
                }

                return true;
            }

            return false;
        }

        // At first I thought to make Points a property of Path, because as you can see, the logic as it stands
        // now is independent of the word's string, as the word length for a tile sequence is fixed. However,
        // normal Wordament supports themes where words have boosted point values, so that wouldn't generalize well.
        public virtual int GetPoints(Path path)
        {
            int points = path.Tiles.Sum(t => t.Points) ?? 0;

            if (Length >= 8) return (int)(2.5 * points);
            if (Length >= 6) return 2 * points;
            if (Length >= 5) return (int)(1.5 * points);
            return points;
        }

        public Path BestPath { get; protected set; }
        public int BestPathPoints { get; protected set; }
        public double BestPathLength => BestPath.PathLength;
        public double BestPathPointsOverWordLength => BestPathPoints / Length;
        public double BestPathPointsOverPathLength => BestPathPoints / BestPathLength;
        public int BestPathStartPosition => BestPath[0].Position;

        public override string ToString()
            => $"{BestPathPoints}\t{String}";
    }
}
