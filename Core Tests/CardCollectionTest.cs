using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackHen.Bridge
{
    [TestClass]
    public class CardCollectionTest
    {
        [TestMethod]
        public void IndexOf()
        {
            var cards = new CardCollection();
            cards.Add(Card.Parse("KC"));
            cards.Add(Card.Parse("AC"));
            cards.Add(Card.Parse("6C"));
            cards.Add(Card.Parse("2H"));
            cards.Add(Card.Parse("QD"));
            cards.Add(Card.Parse("AD"));

            foreach (var card in cards)
            {
                int index = cards.IndexOf(card);
                Assert.IsTrue(index >= 0);
                Assert.AreEqual(card, cards[index]);
            }

            Assert.AreEqual(-1, cards.IndexOf(Card.Parse("2C")));
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
    }
}
