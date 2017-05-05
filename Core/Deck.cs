using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackHen.Bridge
{
    /// <summary>
    ///   A distribution of 52 <see cref="Card">cards </see>.
    /// </summary>
    public class Deck
    {
        /// <summary>
        ///   The number of <see cref="Card">cards</see> in a <see cref="Deck"/>.
        /// </summary>
        public const int CardCount = Card.SuitCount * Card.CardsInSuit;

        public Deck()
        {
            var allCards = Card.All;
            Cards = new Card[allCards.Length];
            allCards.CopyTo(Cards, 0);
        }

        public Card[] Cards { get; set; }

        /// <summary>
        ///   Randomises the <see cref="Cards"/>.
        /// </summary>
        /// <remarks>
        ///   Uses the Fisher–Yates shuffle algorithm.
        /// </remarks>
        public Deck ShuffleFisherYates()
        {
            var n = Cards.Length;                  // The number of items left to shuffle (loop invariant).
            while (n > 1)
            {
                var k = RandomThreaded.Generator.Next(n); // 0 <= k < n.
                n--;                                // n is now the last pertinent index;
                var t = Cards[n];                   // swap elements n and k
                Cards[n] = Cards[k];
                Cards[k] = t;
            }

            return this;
        }

        /// <summary>
        ///   Randomises the <see cref="Cards"/>.
        /// </summary>
        /// <returns>
        ///   A shuffled <see cref="Deck"/>.
        /// </returns>
        public Deck Shuffle()
        {
            int[] keys = new int[Cards.Length];
            for (int i = keys.Length - 1; 0 <= i; --i)
            {
                keys[i] = RandomThreaded.Generator.Next();
            }

            Array.Sort<int, Card>(keys, Cards);
            return this;
        }

        public Deck Deal(Board board)
        {
            return Deal(board.Hands, board.Dealer);
        }

        public Deck Deal(HandCollection hands)
        {
            return Deal(hands, Seat.West);
        }

        public Deck Deal(HandCollection hands, Seat dealer)
        {
            var seat = dealer;
            foreach (var hand in hands)
                hand.Cards.Clear();
            for (int i = 0; i < Cards.Length; ++i)
            {
                seat = HandCollection.NextSeat(seat);
                hands[seat].Add(Cards[i]);
            }

            return this;
        }

        public Deck Deal(HandCollection hands, Seat dealer, params Hand[] fixedHands)
        {
            var seat = dealer;
            foreach (var hand in hands)
            {
                if (!fixedHands.Contains<Hand>(hand))
                    hand.Cards.Clear();
            }
            for (int i = 0; i < Cards.Length; ++i)
            {
                seat = HandCollection.NextSeat(seat);
                if (fixedHands.Contains<Hand>(hands[seat]))
                    continue;
                var card = Cards[i];
                bool fixedCard = false;
                foreach (var fixedHand in fixedHands)
                {
                    if (fixedHand.Cards.Contains(card))
                    {
                        fixedCard = true;
                        break;
                    }
                }
                if (!fixedCard)
                    hands[seat].Add(Cards[i]);
            }

            return this;
        }

    }
}
