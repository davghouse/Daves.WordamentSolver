using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Daves.WordamentSolver.UnitTests
{
    [TestClass]
    public class BoardTests
    {
        [TestMethod]
        public void BoardConstruction1()
        {
            var tileStrings = new[]
            {
                "A", "B", "E", "F",
                "G", "K", "U", "O",
                "L", "E", "M", "D"
            };

            var tilePoints = new[]
            {
                1, (int?)null, 2, 3,
                6, 9, 8, 4,
                1, 1, 0, 11
            };

            var board = new Board(3, 4, tileStrings, tilePoints);
            Assert.AreEqual(3, board.BoardHeight);
            Assert.AreEqual(4, board.BoardWidth);
            Assert.AreEqual(12, board.BoardSize);
            for (int i = 0; i < 12; ++i)
            {
                Assert.AreEqual(i, board.Tiles[i].Position);
                Assert.AreEqual(tileStrings[i], board.Tiles[i].String);
                Assert.AreEqual(tilePoints[i], board.Tiles[i].Points);
            }
        }

        [TestMethod]
        public void BoardConstruction2()
        {
            var tileStrings = new[]
            {
                "A", "B", "E", "F",
                "G", "K", "U", "O"
            };

            var tilePoints = new[]
            {
                1, (int?)null, 2, 3,
                6, 9, 8, 4
            };

            var board = new Board(2, 4, p => tileStrings[p], p => tilePoints[p]);
            Assert.AreEqual(2, board.BoardHeight);
            Assert.AreEqual(4, board.BoardWidth);
            Assert.AreEqual(8, board.BoardSize);
            for (int i = 0; i < 8; ++i)
            {
                Assert.AreEqual(i, board.Tiles[i].Position);
                Assert.AreEqual(tileStrings[i], board.Tiles[i].String);
                Assert.AreEqual(tilePoints[i], board.Tiles[i].Points);
            }
        }

        [TestMethod]
        public void BoardConstruction3()
        {
            var board = new Board(19, 6, p => null, p => null);
            Assert.AreEqual(19, board.BoardHeight);
            Assert.AreEqual(6, board.BoardWidth);
            Assert.AreEqual(6 * 19, board.BoardSize);
            for (int i = 0; i < 6 * 19; ++i)
            {
                Assert.AreEqual(i, board.Tiles[i].Position);
                Assert.AreEqual(null, board.Tiles[i].String);
                Assert.AreEqual(null, board.Tiles[i].Points);
            }
        }

        [TestMethod]
        public void BoardConstruction4()
        {
            var tileStrings = new string[3, 4]
            {
                {"A", "B", "E", "F" },
                {"G", "K", "U", "O" },
                {"L", "E", "M", "D" }
            };

            var tilePoints = new int?[3, 4]
            {
                { 1, (int?)null, 2, 3 },
                { 6, 9, 8, 4 },
                { 1, 1, 0, 11 }
            };

            var board = new Board(tileStrings, tilePoints);
            Assert.AreEqual(3, board.BoardHeight);
            Assert.AreEqual(4, board.BoardWidth);
            Assert.AreEqual(12, board.BoardSize);
            for (int i = 0; i < 12; ++i)
            {
                Assert.AreEqual(i, board.Tiles[i].Position);
                Assert.AreEqual(tileStrings[i / 4, i % 4], board.Tiles[i].String);
                Assert.AreEqual(tilePoints[i / 4, i % 4], board.Tiles[i].Points);
            }
        }
    }
}
