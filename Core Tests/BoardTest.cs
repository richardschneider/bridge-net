using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackHen.Bridge
{
   [TestClass]
   public class BoardTest
   {
      [TestMethod]
      public void Properties()
      {
         var board = new Board();
         board.Vulnerability = Vulnerability.EastWest;
         board.Dealer = Seat.South;

         Assert.AreEqual(Vulnerability.EastWest, board.Vulnerability);
         Assert.AreEqual(Seat.South, board.Dealer);
      }

      [TestMethod]
      public void Stringing()
      {
         var board = new Board();
         board.Vulnerability = Vulnerability.EastWest;
         board.Dealer = Seat.South;
         new Deck()
            .Shuffle()
            .Deal(board);
         Console.WriteLine(board);
      }

      [TestMethod]
      public void CreateSet()
      {
         var boards = Board.CreateSet(16);
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

      [TestMethod]
      public void Vulnerbility()
      {
         Board board = new Board();

         board.Vulnerability = Vulnerability.None;
         Assert.AreEqual(false, board.IsVulnerable(Seat.East));
         Assert.AreEqual(false, board.IsVulnerable(Seat.South));
         Assert.AreEqual(false, board.IsVulnerable(Seat.West));
         Assert.AreEqual(false, board.IsVulnerable(Seat.North));

         board.Vulnerability = Vulnerability.All;
         Assert.AreEqual(true, board.IsVulnerable(Seat.East));
         Assert.AreEqual(true, board.IsVulnerable(Seat.South));
         Assert.AreEqual(true, board.IsVulnerable(Seat.West));
         Assert.AreEqual(true, board.IsVulnerable(Seat.North));

         board.Vulnerability = Vulnerability.EastWest;
         Assert.AreEqual(true, board.IsVulnerable(Seat.East));
         Assert.AreEqual(false, board.IsVulnerable(Seat.South));
         Assert.AreEqual(true, board.IsVulnerable(Seat.West));
         Assert.AreEqual(false, board.IsVulnerable(Seat.North));

         board.Vulnerability = Vulnerability.NorthSouth;
         Assert.AreEqual(false, board.IsVulnerable(Seat.East));
         Assert.AreEqual(true, board.IsVulnerable(Seat.South));
         Assert.AreEqual(false, board.IsVulnerable(Seat.West));
         Assert.AreEqual(true, board.IsVulnerable(Seat.North));
      }

   }
}
