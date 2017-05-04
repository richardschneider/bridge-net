using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlackHen.Bridge;

namespace BlackHen.Bridge.Scoring
{
   [TestClass]
   public class MatchpointsTest
   {
      Matchpoints scorer = new Matchpoints();

      [TestMethod]
      public void Simple()
      {
         var board = new Board() { Number = 1, Vulnerability = Vulnerability.None };
         List<Game> games = new List<Game>();
         games.Add(new Game(board) { Contract = new Contract(3, Denomination.NoTrumps, Risk.None, Seat.North), Made = 3 });
         games.Add(new Game(board) { Contract = new Contract(3, Denomination.NoTrumps, Risk.None, Seat.North), Made = 4 });
         games.Add(new Game(board) { Contract = new Contract(2, Denomination.NoTrumps, Risk.None, Seat.North), Made = 3 });
         games.Add(new Game(board) { Contract = new Contract(3, Denomination.NoTrumps, Risk.None, Seat.North), Made = -1 });

         scorer.Calculate(games);
         Assert.AreEqual(400, games[0].PartnershipScore(Partnership.NorthSouth));
         Assert.AreEqual(430, games[1].PartnershipScore(Partnership.NorthSouth));
         Assert.AreEqual(150, games[2].PartnershipScore(Partnership.NorthSouth));
         Assert.AreEqual(-50, games[3].PartnershipScore(Partnership.NorthSouth));

         Assert.AreEqual(-400, games[0].PartnershipScore(Partnership.EastWest));
         Assert.AreEqual(-430, games[1].PartnershipScore(Partnership.EastWest));
         Assert.AreEqual(-150, games[2].PartnershipScore(Partnership.EastWest));
         Assert.AreEqual(50, games[3].PartnershipScore(Partnership.EastWest));

         Assert.AreEqual(4, games[0].MatchpointsNorthSouth);
         Assert.AreEqual(2, games[0].MatchpointsEastWest);
         Assert.AreEqual(6, games[1].MatchpointsNorthSouth);
         Assert.AreEqual(0, games[1].MatchpointsEastWest);
         Assert.AreEqual(2, games[2].MatchpointsNorthSouth);
         Assert.AreEqual(4, games[2].MatchpointsEastWest);
         Assert.AreEqual(0, games[3].MatchpointsNorthSouth);
         Assert.AreEqual(6, games[3].MatchpointsEastWest);
      }

      [TestMethod]
      public void Ties2()
      {
         var board = new Board() { Number = 2, Vulnerability = Vulnerability.None };
         List<Game> games = new List<Game>();
         games.Add(new Game(board) { Contract = new Contract(2, Denomination.Spades, Risk.None, Seat.West), Made = 2 });
         games.Add(new Game(board) { Contract = new Contract(2, Denomination.Spades, Risk.None, Seat.West), Made = 3 });
         games.Add(new Game(board) { Contract = new Contract(2, Denomination.Spades, Risk.None, Seat.West), Made = 3 });
         games.Add(new Game(board) { Contract = new Contract(1, Denomination.NoTrumps, Risk.None, Seat.West), Made = 2 });

         scorer.Calculate(games);
         Assert.AreEqual(110, games[0].PartnershipScore(Partnership.EastWest));
         Assert.AreEqual(140, games[1].PartnershipScore(Partnership.EastWest));
         Assert.AreEqual(140, games[2].PartnershipScore(Partnership.EastWest));
         Assert.AreEqual(120, games[3].PartnershipScore(Partnership.EastWest));

         Assert.AreEqual(6, games[0].MatchpointsNorthSouth);
         Assert.AreEqual(0, games[0].MatchpointsEastWest);
         Assert.AreEqual(1, games[1].MatchpointsNorthSouth);
         Assert.AreEqual(5, games[1].MatchpointsEastWest);
         Assert.AreEqual(1, games[2].MatchpointsNorthSouth);
         Assert.AreEqual(5, games[2].MatchpointsEastWest);
         Assert.AreEqual(4, games[3].MatchpointsNorthSouth);
         Assert.AreEqual(2, games[3].MatchpointsEastWest);
      }

      [TestMethod]
      public void Ties3()
      {
         var board = new Board() { Number = 3, Vulnerability = Vulnerability.None };
         List<Game> games = new List<Game>();
         games.Add(new Game(board) { Contract = new Contract(1, Denomination.NoTrumps, Risk.None, Seat.West), Made = -1 });
         games.Add(new Game(board) { Contract = new Contract(2, Denomination.Clubs, Risk.None, Seat.West), Made = 2 });
         games.Add(new Game(board) { Contract = new Contract(2, Denomination.Clubs, Risk.None, Seat.West), Made = 2 });
         games.Add(new Game(board) { Contract = new Contract(2, Denomination.Clubs, Risk.None, Seat.West), Made = 2 });
         Assert.AreEqual(-50, games[0].PartnershipScore(Partnership.EastWest));

         scorer.Calculate(games);
         Assert.AreEqual(-50, games[0].PartnershipScore(Partnership.EastWest));
         Assert.AreEqual(90, games[1].PartnershipScore(Partnership.EastWest));
         Assert.AreEqual(90, games[2].PartnershipScore(Partnership.EastWest));
         Assert.AreEqual(90, games[3].PartnershipScore(Partnership.EastWest));

         Assert.AreEqual(6, games[0].MatchpointsNorthSouth);
         Assert.AreEqual(0, games[0].MatchpointsEastWest);
         Assert.AreEqual(2, games[1].MatchpointsNorthSouth);
         Assert.AreEqual(4, games[1].MatchpointsEastWest);
         Assert.AreEqual(2, games[2].MatchpointsNorthSouth);
         Assert.AreEqual(4, games[2].MatchpointsEastWest);
         Assert.AreEqual(2, games[3].MatchpointsNorthSouth);
         Assert.AreEqual(4, games[3].MatchpointsEastWest);
      }

   }
}
