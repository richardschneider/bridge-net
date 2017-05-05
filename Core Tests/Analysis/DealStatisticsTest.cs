using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Makaretu.Bridge.Analysis
{
    [TestClass]
    public class DealStatisticsTest
    {
        [TestMethod]
        public void AverageHcp()
        {
            var tournament = new Tournament();
            tournament.GenerateBoards(2);
            Console.WriteLine("Board 1: {0}", tournament.Boards[0].Hands);
            Console.WriteLine("Board 2: {0}", tournament.Boards[1].Hands);
            Console.WriteLine("Avg HCP: {0}", DealStatistics.AverageHcp(tournament.Boards));
        }

        [TestMethod]
        public void Voids()
        {
            var boards = new List<Board>();
            var board = new Board();
            boards.Add(board);
            board.Hands[Seat.North].Add(Card.Parse("2S"));
            board.Hands[Seat.North].Add(Card.Parse("2H"));
            board.Hands[Seat.North].Add(Card.Parse("2D"));
            board.Hands[Seat.North].Add(Card.Parse("2C"));
            board.Hands[Seat.East].Add(Card.Parse("3S"));
            board.Hands[Seat.East].Add(Card.Parse("3H"));
            board.Hands[Seat.East].Add(Card.Parse("3D"));
            board.Hands[Seat.East].Add(Card.Parse("3C"));
            board.Hands[Seat.South].Add(Card.Parse("4S"));
            board.Hands[Seat.South].Add(Card.Parse("4H"));
            board.Hands[Seat.South].Add(Card.Parse("4D"));
            board.Hands[Seat.South].Add(Card.Parse("4C"));
            board.Hands[Seat.West].Add(Card.Parse("5S"));
            board.Hands[Seat.West].Add(Card.Parse("5H"));
            board.Hands[Seat.West].Add(Card.Parse("5D"));
            board.Hands[Seat.West].Add(Card.Parse("5C"));
            var voids = DealStatistics.Voids(boards);
            Assert.AreEqual(0, voids.North);
            Assert.AreEqual(0, voids.South);
            Assert.AreEqual(0, voids.East);
            Assert.AreEqual(0, voids.West);

            board = new Board();
            boards.Add(board);
            board.Hands[Seat.North].Add(Card.Parse("2H"));
            board.Hands[Seat.North].Add(Card.Parse("2D"));
            board.Hands[Seat.North].Add(Card.Parse("2C"));
            board.Hands[Seat.East].Add(Card.Parse("3S"));
            board.Hands[Seat.East].Add(Card.Parse("3H"));
            board.Hands[Seat.East].Add(Card.Parse("3D"));
            board.Hands[Seat.East].Add(Card.Parse("3C"));
            board.Hands[Seat.South].Add(Card.Parse("4S"));
            board.Hands[Seat.South].Add(Card.Parse("4H"));
            board.Hands[Seat.South].Add(Card.Parse("4D"));
            board.Hands[Seat.South].Add(Card.Parse("4C"));
            board.Hands[Seat.West].Add(Card.Parse("5S"));
            board.Hands[Seat.West].Add(Card.Parse("5H"));
            board.Hands[Seat.West].Add(Card.Parse("5D"));
            board.Hands[Seat.West].Add(Card.Parse("5C"));
            voids = DealStatistics.Voids(boards);
            Assert.AreEqual(1, voids.North);
            Assert.AreEqual(0, voids.South);
            Assert.AreEqual(0, voids.East);
            Assert.AreEqual(0, voids.West);

            board = new Board();
            boards.Add(board);
            board.Hands[Seat.North].Add(Card.Parse("2H"));
            board.Hands[Seat.North].Add(Card.Parse("2D"));
            board.Hands[Seat.North].Add(Card.Parse("2C"));
            board.Hands[Seat.East].Add(Card.Parse("3S"));
            board.Hands[Seat.East].Add(Card.Parse("3H"));
            board.Hands[Seat.East].Add(Card.Parse("3D"));
            board.Hands[Seat.South].Add(Card.Parse("4S"));
            board.Hands[Seat.South].Add(Card.Parse("4H"));
            board.Hands[Seat.South].Add(Card.Parse("4D"));
            board.Hands[Seat.South].Add(Card.Parse("4C"));
            board.Hands[Seat.West].Add(Card.Parse("5S"));
            board.Hands[Seat.West].Add(Card.Parse("5H"));
            board.Hands[Seat.West].Add(Card.Parse("5D"));
            board.Hands[Seat.West].Add(Card.Parse("5C"));
            voids = DealStatistics.Voids(boards);
            Assert.AreEqual(2, voids.North);
            Assert.AreEqual(0, voids.South);
            Assert.AreEqual(1, voids.East);
            Assert.AreEqual(0, voids.West);

            board = new Board();
            boards.Add(board);
            board.Hands[Seat.North].Add(Card.Parse("2H"));
            board.Hands[Seat.North].Add(Card.Parse("2D"));
            board.Hands[Seat.North].Add(Card.Parse("2C"));
            board.Hands[Seat.East].Add(Card.Parse("3S"));
            board.Hands[Seat.East].Add(Card.Parse("3H"));
            board.Hands[Seat.East].Add(Card.Parse("3D"));
            voids = DealStatistics.Voids(boards);
            Assert.AreEqual(3, voids.North);
            Assert.AreEqual(4, voids.South);
            Assert.AreEqual(2, voids.East);
            Assert.AreEqual(4, voids.West);
        }

    }
}
