using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackHen.Bridge
{
    /// <summary>
    /// Summary description for BiddingHistoryTest
    /// </summary>
    [TestClass]
    public class AuctionTest
    {

        [TestMethod]
        public void FinalContract_OneBid()
        {
            var bidding = new Auction(Seat.North);
            bidding.Add(new Bid(1, Denomination.Clubs));
            bidding.Add(Bid.Pass);
            bidding.Add(Bid.Pass);
            bidding.Add(Bid.Pass);
            Assert.AreEqual("1C by North", bidding.FinalContract().ToString());

            bidding = new Auction(Seat.South);
            bidding.Add(Bid.Pass);
            bidding.Add(Bid.Pass);
            bidding.Add(Bid.Pass);
            bidding.Add(new Bid(1, Denomination.Clubs));
            bidding.Add(Bid.Pass);
            bidding.Add(new Bid(2, Denomination.Clubs));
            bidding.Add(Bid.Pass);
            bidding.Add(Bid.Pass);
            bidding.Add(Bid.Pass);
            Assert.AreEqual("2C by East", bidding.FinalContract().ToString());
        }

        [TestMethod]
        public void FinalContract_Overcall1()
        {
            var bidding = new Auction(Seat.East);
            bidding.Add(new Bid(1, Denomination.Clubs));
            bidding.Add(new Bid(1, Denomination.Spades));
            bidding.Add(Bid.Pass);
            bidding.Add(Bid.Pass);
            bidding.Add(Bid.Pass);
            Assert.AreEqual("1S by South", bidding.FinalContract().ToString());
        }

        [TestMethod]
        public void FinalContract_Overcall2()
        {
            var bidding = new Auction(Seat.East);
            bidding.Add(new Bid(1, Denomination.Clubs));
            bidding.Add(new Bid(1, Denomination.Spades));
            bidding.Add(Bid.Pass);
            bidding.Add(Bid.Pass);
            bidding.Add(new Bid(1, Denomination.NoTrumps));
            bidding.Add(Bid.Pass);
            bidding.Add(Bid.Pass);
            bidding.Add(Bid.Pass);
            Assert.AreEqual("1NT by East", bidding.FinalContract().ToString());
        }

        [TestMethod]
        public void FinalContract_Overcall3()
        {
            var bidding = new Auction(Seat.East);
            bidding.Add(new Bid(1, Denomination.Clubs));
            bidding.Add(new Bid(1, Denomination.Spades));
            bidding.Add(Bid.Pass);
            bidding.Add(Bid.Pass);
            bidding.Add(Bid.Pass);
            bidding.Add(new Bid(2, Denomination.Clubs));
            bidding.Add(Bid.Pass);
            bidding.Add(Bid.Pass);
            bidding.Add(Bid.Pass);
            Assert.AreEqual("2C by South", bidding.FinalContract().ToString());
        }
        [TestMethod]
        public void FinalContract_Doubled()
        {
            var bidding = new Auction(Seat.West);
            bidding.Add(new Bid(1, Denomination.Hearts));
            bidding.Add(Bid.Pass);
            bidding.Add(new Bid(4, Denomination.Hearts));
            bidding.Add(Bid.Double);
            bidding.Add(Bid.Pass);
            bidding.Add(Bid.Pass);
            bidding.Add(Bid.Pass);
            Assert.AreEqual("4HX by West", bidding.FinalContract().ToString());
        }

        [TestMethod]
        public void FinalContract_DoubledChanged()
        {
            var bidding = new Auction(Seat.North);
            bidding.Add(new Bid(1, Denomination.NoTrumps));
            bidding.Add(Bid.Pass);
            bidding.Add(new Bid(3, Denomination.NoTrumps));
            bidding.Add(Bid.Double);
            bidding.Add(new Bid(4, Denomination.Spades));
            bidding.Add(Bid.Pass);
            bidding.Add(Bid.Pass);
            bidding.Add(Bid.Pass);
            Assert.AreEqual("4S by North", bidding.FinalContract().ToString());
        }

        [TestMethod]
        public void FinalContract_PassedIn()
        {
            var bidding = new Auction(Seat.West);
            bidding.Add(Bid.Pass);
            bidding.Add(Bid.Pass);
            bidding.Add(Bid.Pass);
            bidding.Add(Bid.Pass);

            Assert.AreEqual(Contract.PassedIn, bidding.FinalContract());
            Assert.AreEqual("-", bidding.FinalContract().ToString());
        }
    }

}
