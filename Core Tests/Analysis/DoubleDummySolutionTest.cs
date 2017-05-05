using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Makaretu.Bridge.Analysis
{
    /// <summary>
    /// Summary description for DoubleDummySolution
    /// </summary>
    [TestClass]
    public class DoubleDummySolutionTest
    {
        [TestMethod]
        public void Indexing()
        {
            var dds = new DoubleDummySolution();
            dds.Add(new Contract(3, Denomination.Hearts, Risk.None, Seat.East));
            dds.Add(new Contract(4, Denomination.Hearts, Risk.None, Seat.West));
            Assert.AreEqual("3H by East", dds[Seat.East, Denomination.Hearts].ToString());
            Assert.AreEqual("4H by West", dds[Seat.West, Denomination.Hearts].ToString());
            Assert.AreEqual(0, dds[Seat.North, Denomination.Hearts].Level);
            Assert.AreEqual(Contract.PassedIn, dds[Seat.South, Denomination.NoTrumps]);
        }

    }
}
