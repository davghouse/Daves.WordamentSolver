using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Daves.WordamentSolver.UnitTests
{
    [TestClass]
    public class SolutionTests
    {
        public static IReadOnlyList<string> BasicTileStrings = new[]
        {
            "A", "B", "E", "F",
            "G", "K", "U", "O",
            "L", "E", "M", "D",
            "R", "A", "F", "O"
        };

        public static IReadOnlyList<string> ComplexTileStrings = new[]
        {
            "-ING", "I/S", "E", "R/P",
            "REA-", "D/E", "Y/U", "FE-",
            "LA", "E/S", "M/O", "-ED",
            "R/T", "A/T", "H", "O/B"
        };

        public static IReadOnlyList<string> PathMultiplicityTileStrings = new[]
        {
            "P/N", "A", "P/n",
            "a",   "a", "a",
            "p/n", "A/q", "p/N",
        };

        [TestMethod]
        public void BasicSolution()
        {
            var board = new Board(4, 4, p => BasicTileStrings[p], p => null);
            board.GuessTilePoints();
            var solution = new Solution(board, WordSorter.Points);

            Assert.AreEqual("AGLEAM", solution.Words[0].String);
            Assert.AreEqual(32, solution.Words[0].BestPathPoints);
            Assert.AreEqual("BAKER", solution.Words[4].String);
            Assert.AreEqual(24, solution.Words[4].BestPathPoints);
            Assert.AreEqual("ERA", solution.Words[109].String);
            Assert.AreEqual(5, solution.Words[109].BestPathPoints);
            Assert.AreEqual(1319, solution.TotalPoints);
            Assert.AreEqual(110, solution.TotalWords);
        }

        [TestMethod]
        public void ComplexSolution()
        {
            var board = new Board(4, 4, p => ComplexTileStrings[p], p => null);
            board.GuessTilePoints();
            var solution = new Solution(board, WordSorter.Points);

            Assert.AreEqual("HOUSESAT", solution.Words[0].String);
            Assert.AreEqual(360, solution.Words[0].BestPathPoints);
            Assert.AreEqual("REHOUSE", solution.Words[4].String);
            Assert.AreEqual(248, solution.Words[4].BestPathPoints);
            Assert.AreEqual("RESUMES", solution.Words[8].String);
            Assert.AreEqual(242, solution.Words[8].BestPathPoints);
            Assert.AreEqual(63353, solution.TotalPoints);
            Assert.AreEqual(618, solution.TotalWords);

            solution.TryGetWord("REHOUSE", out Word word);
            Assert.AreEqual(2, word.Paths.Count);
            Assert.AreEqual(board.Tiles[5], word.BestPath.Last());
            Assert.AreEqual(board.Tiles[2], word.Paths.Single(p => p != word.BestPath).Last());
        }

        [TestMethod]
        public void PathMultiplicitySolution()
        {
            var board = new Board(3, 3, p => PathMultiplicityTileStrings[p], p => null);
            board.GuessTilePoints();
            var solution = new Solution(board, WordSorter.Points);

            solution.TryGetWord("pan", out Word word);
            Assert.AreEqual(20, word.Paths.Count);
            solution.TryGetWord("PAN", out word);
            Assert.AreEqual(20, word.Paths.Count);
            solution.TryGetWord("PaN", out word);
            Assert.AreEqual(20, word.Paths.Count);

            solution.TryGetPath(new[] { board.Tiles[6], board.Tiles[7], board.Tiles[8] }, out Path path);
            Assert.AreEqual(path, word.BestPath);
            Assert.AreEqual(4, path.Words.Count);

            solution.TryGetWord("nap", out word);
            Assert.AreEqual(20, word.Paths.Count);
            solution.TryGetWord("nan", out word);
            Assert.AreEqual(20, word.Paths.Count);
            solution.TryGetWord("PAP", out word);
            Assert.AreEqual(20, word.Paths.Count);

            Assert.IsTrue(solution.ContainsWord("papa"));
            Assert.IsFalse(solution.ContainsWord("pappa"));
            Assert.IsTrue(solution.ContainsPath(new[] { board.Tiles[0], board.Tiles[1], board.Tiles[2] }));
            Assert.IsFalse(solution.ContainsPath(new[] { board.Tiles[3], board.Tiles[4], board.Tiles[5] }));

            foreach (var w in solution.Words)
            {
                Assert.AreEqual(w.BestPathPoints, w.Paths.Select(p => w.GetPoints(p)).Max());
            }

            Assert.AreEqual(530, solution.TotalPoints);
            Assert.AreEqual(9, solution.TotalWords);
        }
    }
}
