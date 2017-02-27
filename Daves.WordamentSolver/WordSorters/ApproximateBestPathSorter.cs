using System;
using System.Collections.Generic;

namespace Daves.WordamentSolver.WordSorters
{
    // We're trying to minimize distance spent travelling between words. Words can be thought of as vertices in an asymmetric
    // graph. The edge length from vertex A to vertex B is the length of A's word plus the distance from the final tile of
    // A's word to the first tile of B's word. The edge length from B to A is the reverse of that. But yeah, travelling salesman
    // problem so I'm just going to use a nearest neighbor approximation.
    public class ApproximateBestPathSorter : WordSorter
    {
        public override string Name
            => "approximate best path";

        public override void Sort(Word[] words)
        {
            if (words.Length == 0) return;

            // Doing this sort in-place.
            // Loop invariant: the i words from [0 ... i - 1] are sorted/taken/in their final position. words.Length - i
            // words remain to be sorted.  In each iteration, the goal is to find the word that starts closest to the
            // previous word's end. Stop an index early, since when there's only one to choose from it's not going to move.
            for (int i = 1; i < words.Length - 1; ++i)
            {
                Word previousWord = words[i - 1];
                int indexOfRemainingClosestWord = -1;
                double distanceToRemainingClosestWord = double.MaxValue;

                for (int j = i; j < words.Length; ++j)
                {
                    double distance = GetDistanceBetween(previousWord, words[j]);
                    if (distance < distanceToRemainingClosestWord)
                    {
                        distanceToRemainingClosestWord = distance;
                        indexOfRemainingClosestWord = j;
                    }
                }

                Word temp = words[i];
                words[i] = words[indexOfRemainingClosestWord];
                words[indexOfRemainingClosestWord] = temp;
            }
        }

        public static double GetDistanceBetween(Word firstWord, Word secondWord)
            => GetDistanceBetween(firstWord.Path[firstWord.Path.Count - 1], secondWord.Path[0]);

        // This is the Euclidean (straight-line) distance, and in the future if Board becomes a base class that can be
        // overridden to support different metrics, this would need a redesign (we'd need a Board reference).
        public static double GetDistanceBetween(Tile firstTile, Tile secondTile)
            => Math.Sqrt(Math.Pow(firstTile.Row - secondTile.Row, 2) + Math.Pow(firstTile.Column - secondTile.Column, 2));

        public static double CalculateTotalDistanceBetweenWords(IReadOnlyList<Word> words)
        {
            double totalPathLengthBetweenWords = 0;
            for (int i = 1; i < words.Count; ++i)
            {
                totalPathLengthBetweenWords += GetDistanceBetween(words[i - 1], words[i]);
            }

            return totalPathLengthBetweenWords;
        }
    }
}
