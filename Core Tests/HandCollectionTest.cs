using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Makaretu.Bridge
{
    [TestClass]
    public class HandCollectionTest
    {
        [TestMethod]
        public void Initing()
        {
            var hands = new HandCollection();
            Assert.AreEqual(4, hands.Count);
            Assert.IsInstanceOfType(hands[Seat.North], typeof(Hand));
            Assert.IsInstanceOfType(hands[Seat.South], typeof(Hand));
            Assert.IsInstanceOfType(hands[Seat.East], typeof(Hand));
            Assert.IsInstanceOfType(hands[Seat.West], typeof(Hand));
        }

        [TestMethod]
        public void Indexing()
        {
            var hands = new HandCollection();
            Assert.AreSame(hands[0], hands[Seat.North]);
            Assert.AreSame(hands[1], hands[Seat.East]);
            Assert.AreSame(hands[2], hands[Seat.South]);
            Assert.AreSame(hands[3], hands[Seat.West]);
        }

        [TestMethod]
        public void OtherHandsSeat()
        {
            var hands = new HandCollection();
            new Deck().Deal(hands);
            var otherHands = hands.OtherHands(Seat.North);
            Assert.IsFalse(otherHands.Contains(hands[Seat.North]));
            Assert.IsTrue(otherHands.Contains(hands[Seat.East]));
            Assert.IsTrue(otherHands.Contains(hands[Seat.South]));
            Assert.IsTrue(otherHands.Contains(hands[Seat.West]));
        }

        [TestMethod]
        public void OtherHandsHand()
        {
            var hands = new HandCollection();
            new Deck().Deal(hands);
            var otherHands = hands.OtherHands(hands[Seat.North]);
            Assert.IsFalse(otherHands.Contains(hands[Seat.North]));
            Assert.IsTrue(otherHands.Contains(hands[Seat.East]));
            Assert.IsTrue(otherHands.Contains(hands[Seat.South]));
            Assert.IsTrue(otherHands.Contains(hands[Seat.West]));
        }

        [TestMethod]
        public void OtherHandsHand2()
        {
            var hands = new HandCollection();
            new Deck().Deal(hands);
            var otherHands = hands.OtherHands(hands[Seat.North], hands[Seat.South]);
            Assert.IsFalse(otherHands.Contains(hands[Seat.North]));
            Assert.IsTrue(otherHands.Contains(hands[Seat.East]));
            Assert.IsFalse(otherHands.Contains(hands[Seat.South]));
            Assert.IsTrue(otherHands.Contains(hands[Seat.West]));
        }

        [TestMethod]
        public void ParsePbn()
        {
            var hands = HandCollection.ParsePbn("W:KQT2.AT.J6542.85 - A8654.KQ5.T.QJT6 -");
            Assert.AreEqual(13, hands[Seat.West].Cards.Count);
            Assert.AreEqual(0, hands[Seat.North].Cards.Count);
            Assert.AreEqual(13, hands[Seat.East].Cards.Count);
            Assert.AreEqual(0, hands[Seat.South].Cards.Count);
            CollectionAssert.Contains(hands[Seat.West].Cards, Card.Parse("KS"));
            CollectionAssert.Contains(hands[Seat.East].Cards, Card.Parse("6C"));
        }
        
        [TestMethod]
        public void ToPbn()
        {
            var pbn = "W:KQT2.AT.J6542.85 - A8654.KQ5.T.QJT6 -";
            var hands = HandCollection.ParsePbn(pbn);
            Assert.AreEqual(pbn, hands.ToPbn(Seat.West));
        }

        [TestMethod]
        public void Issue4()
        {
            var pbn = "N:AKJ763.987.T8.Q8 .AKT3.Q6543.KJ64 QT98.64.A9.97532 542.QJ52.KJ72.AT";
            var hands = HandCollection.ParsePbn(pbn);
            Assert.AreEqual(pbn, hands.ToPbn(Seat.North));
        }

    }
}
