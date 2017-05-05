using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackHen.Bridge.Analysis
{
    [TestClass]
    public class BoHaglundDdsTest
    {
        IDoubleDummy Solver { get { return new BoHaglundDds(); } }

        [TestMethod]
        public void DoubleDummy()
        {
            var board = new Board()
            {
                Dealer = Seat.West,
                Vulnerability = Vulnerability.All,
                Hands = HandCollection.ParsePbn("W:KJ95.T.AT873.T98 76.AQJ9642.KJ.QJ QT8.K87.962.A654 A432.53.Q54.K732")
            };
            var dds = Solver.MakeableContracts(board);
            Assert.AreEqual(0, dds[Seat.North, Denomination.Clubs].Level);
            Assert.AreEqual(0, dds[Seat.North, Denomination.Diamonds].Level);
            Assert.AreEqual(3, dds[Seat.North, Denomination.Hearts].Level);
            Assert.AreEqual(0, dds[Seat.North, Denomination.Spades].Level);
            Assert.AreEqual(1, dds[Seat.North, Denomination.NoTrumps].Level);

            Assert.AreEqual(0, dds[Seat.South, Denomination.Clubs].Level);
            Assert.AreEqual(0, dds[Seat.South, Denomination.Diamonds].Level);
            Assert.AreEqual(3, dds[Seat.South, Denomination.Hearts].Level);
            Assert.AreEqual(0, dds[Seat.South, Denomination.Spades].Level);
            Assert.AreEqual(1, dds[Seat.South, Denomination.NoTrumps].Level);

            Assert.AreEqual(1, dds[Seat.East, Denomination.Clubs].Level);
            Assert.AreEqual(2, dds[Seat.East, Denomination.Diamonds].Level);
            Assert.AreEqual(0, dds[Seat.East, Denomination.Hearts].Level);
            Assert.AreEqual(1, dds[Seat.East, Denomination.Spades].Level);
            Assert.AreEqual(0, dds[Seat.East, Denomination.NoTrumps].Level);

            Assert.AreEqual(1, dds[Seat.West, Denomination.Clubs].Level);
            Assert.AreEqual(2, dds[Seat.West, Denomination.Diamonds].Level);
            Assert.AreEqual(0, dds[Seat.West, Denomination.Hearts].Level);
            Assert.AreEqual(1, dds[Seat.West, Denomination.Spades].Level);
            Assert.AreEqual(0, dds[Seat.West, Denomination.NoTrumps].Level);
        }

    }
}
