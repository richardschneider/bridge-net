using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackHen.Bridge
{
    [TestClass]
    public class TournamentTest
    {
        [TestMethod]
        public void Properties()
        {
            var t = new Tournament();
            Assert.IsNotNull(t.Boards);

            var boards = Board.CreateSet(2);
            t.Boards = boards;
            Assert.AreSame(boards, t.Boards);
        }

        [TestMethod]
        public void GenerateBoards()
        {
            var tournament = new Tournament();
            tournament.GenerateBoards(16);
            var boards = tournament.Boards;
            Assert.IsNotNull(boards);
            Assert.AreEqual(16, boards.Count);
            for (int i = 0; i < 16; ++i)
                Assert.AreEqual(i + 1, boards[i].Number);

            Assert.AreEqual(Seat.North, boards[0].Dealer);
            Assert.AreEqual(Seat.East, boards[1].Dealer);
            Assert.AreEqual(Seat.South, boards[2].Dealer);
            Assert.AreEqual(Seat.West, boards[3].Dealer);
            Assert.AreEqual(Seat.North, boards[4].Dealer);
            Assert.AreEqual(Seat.East, boards[5].Dealer);
            Assert.AreEqual(Seat.South, boards[6].Dealer);
            Assert.AreEqual(Seat.West, boards[7].Dealer);
            Assert.AreEqual(Seat.North, boards[8].Dealer);
            Assert.AreEqual(Seat.East, boards[9].Dealer);
            Assert.AreEqual(Seat.South, boards[10].Dealer);
            Assert.AreEqual(Seat.West, boards[11].Dealer);
            Assert.AreEqual(Seat.North, boards[12].Dealer);
            Assert.AreEqual(Seat.East, boards[13].Dealer);
            Assert.AreEqual(Seat.South, boards[14].Dealer);
            Assert.AreEqual(Seat.West, boards[15].Dealer);

            Assert.AreEqual(Vulnerability.None, boards[0].Vulnerability);
            Assert.AreEqual(Vulnerability.NorthSouth, boards[1].Vulnerability);
            Assert.AreEqual(Vulnerability.EastWest, boards[2].Vulnerability);
            Assert.AreEqual(Vulnerability.All, boards[3].Vulnerability);
            Assert.AreEqual(Vulnerability.NorthSouth, boards[4].Vulnerability);
            Assert.AreEqual(Vulnerability.EastWest, boards[5].Vulnerability);
            Assert.AreEqual(Vulnerability.All, boards[6].Vulnerability);
            Assert.AreEqual(Vulnerability.None, boards[7].Vulnerability);
            Assert.AreEqual(Vulnerability.EastWest, boards[8].Vulnerability);
            Assert.AreEqual(Vulnerability.All, boards[9].Vulnerability);
            Assert.AreEqual(Vulnerability.None, boards[10].Vulnerability);
            Assert.AreEqual(Vulnerability.NorthSouth, boards[11].Vulnerability);
            Assert.AreEqual(Vulnerability.All, boards[12].Vulnerability);
            Assert.AreEqual(Vulnerability.None, boards[13].Vulnerability);
            Assert.AreEqual(Vulnerability.NorthSouth, boards[14].Vulnerability);
            Assert.AreEqual(Vulnerability.EastWest, boards[15].Vulnerability);
        }

    }
}
