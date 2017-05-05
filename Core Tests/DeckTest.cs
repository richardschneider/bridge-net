using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackHen.Bridge
{
    [TestClass]
    public class DeckTest
    {
        [TestMethod]
        public void Cards()
        {
            var deck1 = new Deck();
            var deck2 = new Deck();
            Assert.AreNotSame(deck1.Cards, deck2.Cards);
        }

        [TestMethod]
        public void Shuffling()
        {
            var deck1 = new Deck();
            var deck2 = new Deck();
            CollectionAssert.AreEqual(deck1.Cards, deck2.Cards);

            deck1.Shuffle();
            CollectionAssert.AreNotEqual(deck1.Cards, deck2.Cards);
            deck2.Shuffle();
            CollectionAssert.AreNotEqual(deck1.Cards, deck2.Cards);
        }

        [TestMethod]
        public void Dealing()
        {
            var deck = new Deck();
            var board = new Board();
            board.Dealer = Seat.North;
            deck.Shuffle();
            deck.Deal(board);
            Assert.AreEqual(13, board.Hands[Seat.North].Cards.Count);
            Assert.AreEqual(13, board.Hands[Seat.East].Cards.Count);
            Assert.AreEqual(13, board.Hands[Seat.South].Cards.Count);
            Assert.AreEqual(13, board.Hands[Seat.West].Cards.Count);

            // TODO: Check that cards are not duplicated in the suit.
        }
    }
}
