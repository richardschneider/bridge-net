using BlackHen.Bridge.Scoring;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlackHen.Bridge;

namespace BlackHen.Bridge.Scoring
{
    [TestClass]
    public class ContractScorerTest
    {
        ContractScorer scorer = new ContractScorer();

        [TestMethod]
        public void Contracts()
        {
            var contract = new Contract(2, Denomination.Hearts, Risk.None, Seat.South);
            Assert.AreEqual(110, scorer.Calculate(contract, true, 2));

            contract = new Contract(2, Denomination.Hearts, Risk.Doubled, Seat.South);
            Assert.AreEqual(470, scorer.Calculate(contract, false, 2));
        }

        [TestMethod]
        public void Undertricks()
        {
            var contract = new Contract(4, Denomination.Diamonds, Risk.None, Seat.South);
            Assert.AreEqual(-150, scorer.Calculate(contract, false, -3));

            contract = new Contract(4, Denomination.Diamonds, Risk.Doubled, Seat.South);
            Assert.AreEqual(-500, scorer.Calculate(contract, false, -3));

            contract = new Contract(4, Denomination.Diamonds, Risk.Doubled, Seat.South);
            Assert.AreEqual(-800, scorer.Calculate(contract, true, -3));
        }

        [TestMethod]
        public void Overtricks()
        {
            var contract = new Contract(3, Denomination.NoTrumps, Risk.None, Seat.South);
            Assert.AreEqual(660, scorer.Calculate(contract, true, 5));

            contract = new Contract(5, Denomination.Spades, Risk.Redoubled, Seat.South);
            Assert.AreEqual(1600, scorer.Calculate(contract, true, 6));

            contract = new Contract(6, Denomination.NoTrumps, Risk.None, Seat.South);
            Assert.AreEqual(1020, scorer.Calculate(contract, false, 7));
        }
    }
}
