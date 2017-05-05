using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Makaretu.Bridge
{
    /// <summary>
    /// Summary description for GameTest
    /// </summary>
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void Scoring()
        {
            var board = new Board() { Vulnerability = Vulnerability.EastWest };
            var game = new Game(board)
            {
                Contract = new Contract(6, Denomination.NoTrumps, Risk.Redoubled, Seat.East),
                Made = 7
            };
            Assert.AreEqual(2510, game.Score);
            Assert.AreEqual(2510, game.PartnershipScore(Partnership.EastWest));
            Assert.AreEqual(-2510, game.PartnershipScore(Partnership.NorthSouth));

            game = new Game(board)
            {
                Contract = new Contract(7, Denomination.NoTrumps, Risk.Redoubled, Seat.East),
                Made = -1
            };
            Assert.AreEqual(-400, game.Score);
            Assert.AreEqual(-400, game.PartnershipScore(Partnership.EastWest));
            Assert.AreEqual(400, game.PartnershipScore(Partnership.NorthSouth));
        }

        [TestMethod]
        public void Scoring_PassedIn()
        {
            var board = new Board() { Vulnerability = Vulnerability.EastWest, Dealer = Seat.South };
            var game = new Game(board);
            game.Bids.Add(Bid.Pass);
            game.Bids.Add(Bid.Pass);
            game.Bids.Add(Bid.Pass);
            game.Bids.Add(Bid.Pass);
            game.Contract = game.Bids.FinalContract();
            Assert.AreEqual(Contract.PassedIn, game.Contract);
            Assert.AreEqual(0, game.Score);
        }

        [TestMethod]
        public void PairScoring()
        {
            var ew = new Pair() { Number = 1, Partnership = Partnership.EastWest };
            var ns = new Pair() { Number = 1, Partnership = Partnership.NorthSouth };
            var board = new Board() { Vulnerability = Vulnerability.EastWest };
            var game = new Game(board)
            {
                Contract = new Contract(6, Denomination.NoTrumps, Risk.Redoubled, Seat.East),
                Made = 7
            };
            Assert.AreEqual(2510, game.PairScore(ew));
            Assert.AreEqual(-2510, game.PairScore(ns));

            game = new Game(board)
            {
                Contract = new Contract(7, Denomination.NoTrumps, Risk.Redoubled, Seat.East),
                Made = -1
            };
            Assert.AreEqual(-400, game.PairScore(ew));
            Assert.AreEqual(400, game.PairScore(ns));
        }
    }
}
