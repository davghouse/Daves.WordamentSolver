using System;
using System.Collections.Generic;
using System.Linq;

namespace Daves.WordamentSolver.WordSorters
{
    // So this is simulating an ideal solve by a player. When a path is traversed, all the words it produces that haven't been
    // found already are now found. I don't know if that's how Wordament does it, but it's more relevant for us because we support
    // an arbitrary number of either/or tiles, or who knows what else. So paths can give multiple words and obviously words can
    // exist on multiple paths. We don't consider all paths--only a word's sole best path, the one giving the most points, but if
    // there is a path yielding multiple words it's usually the best path for those words, since it incorporates an either/or tile.
    // We're trying to minimize distance spent travelling between paths. Paths can be thought of as vertices in an asymmetric
    // graph. The edge length from vertex A to vertex B is the length of A's path plus the distance from the final tile of
    // A's path to the first tile of B's path. The edge length from B to A is the reverse of that. But yeah, travelling salesman
    // problem so I'm going to use a nearest neighbor approximation. All we really want to do is minimize distance, but we might
    // as well break closeness ties by choosing the path that produces the most points (even though a complete solve is assumed).
    public class ApproximateBestPathSorter : WordSorter
    {
        public override string Name
            => "approximate best path";

        public override void Sort(Word[] words)
        {
            if (words.Length == 0) return;

            var paths = words
                // Order by points first to guarantee the order doesn't depend upon the incoming order of the array,
                // and to start off with the most valuable. We want to break closeness ties, but just this won't be enough.
                .OrderBy(w => w, WordSorter.Points)
                .Select(w => w.BestPath)
                .Distinct() // This is order-preserving.
                .ToArray();

            // Loop invariant: the first i paths are in order. In each iteration, the goal is to find the path that starts closest
            // to the previous path's end. Stop an index early, since when there's only one to choose from it's not going to move.
            for (int i = 1; i < paths.Length - 1; ++i)
            {
                Path previousPath = paths[i - 1];
                int indexOfRemainingClosestPath = -1;
                double distanceToRemainingClosestPath = double.MaxValue;
                double pointsOfRemainingClosestPath = double.MinValue;

                for (int j = i; j < paths.Length; ++j)
                {
                    double distance = GetDistanceBetween(previousPath, paths[j]);
                    double points = paths[j].Words.First().GetPoints(paths[j]); // See the commeont on GetPoints.

                    if (distance < distanceToRemainingClosestPath
                        || (distance == distanceToRemainingClosestPath
                            && points > pointsOfRemainingClosestPath))
                    {
                        indexOfRemainingClosestPath = j;
                        distanceToRemainingClosestPath = distance;
                        pointsOfRemainingClosestPath = points;
                    }
                }

                Path temp = paths[i];
                paths[i] = paths[indexOfRemainingClosestPath];
                paths[indexOfRemainingClosestPath] = temp;
            }

            // Reconstitute the word order from the path order. Loop invariant: the first c words are in order.
            int c = 0;
            foreach (Path path in paths)
            {
                for (int i = c; i < words.Length; ++i)
                {
                    if (words[i].BestPath == path)
                    {
                        Word temp = words[c];
                        words[c++] = words[i];
                        words[i] = temp;
                    }
                }
            }
        }

        public static double GetDistanceBetween(Path firstPath, Path secondPath)
            => GetDistanceBetween(firstPath.Tiles.Last(), secondPath.Tiles.First());

        public static double GetDistanceBetween(Tile firstTile, Tile secondTile)
            => Math.Sqrt(
                  Math.Pow(firstTile.Row - secondTile.Row, 2)
                + Math.Pow(firstTile.Column - secondTile.Column, 2));

        public static double CalculateTotalPathLength(IReadOnlyList<Word> words)
        {
            var paths = words
                .Select(w => w.BestPath)
                .Distinct() // This is order-preserving.
                .ToArray();

            double totalPathLengthFromWords = paths.Sum(p => p.PathLength);
            double totalPathLengthBetweenWords = 0;
            for (int i = 1; i < paths.Length; ++i)
            {
                totalPathLengthBetweenWords += GetDistanceBetween(paths[i - 1], paths[i]);
            }

            return totalPathLengthFromWords + totalPathLengthBetweenWords;
        }
    }
}
