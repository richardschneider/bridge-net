using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Makaretu.Bridge
{
    public class DealGenerator
    {
        public void Generate(HandCollection hands)
        {
            var deck = new Deck();
            deck.Shuffle();
            deck.Deal(hands);

            restart:
            foreach (var hand in hands)
            {
                foreach (var fact in hand.Facts)
                {
                    if (fact.IsTrue(hand))
                        continue;
                    if (!fact.MakeGood(hand, hands.OtherHands(hand)))
                        throw new Exception(string.Format("Can not make the hand {0} contain {1}.", hand, fact));
                    goto restart;
                }
            }
        }

        public void Generate(HandCollection hands, params Hand[] fixedHands)
        {
            var deck = new Deck();
            deck.Shuffle();
            deck.Deal(hands, Seat.West, fixedHands);

            restart:
            foreach (var hand in hands)
            {
                if (fixedHands.Contains<Hand>(hand))
                    continue;

                foreach (var fact in hand.Facts)
                {
                    if (fact.IsTrue(hand))
                        continue;
                    var ignore = new Hand[fixedHands.Length + 1];
                    ignore[0] = hand;
                    fixedHands.CopyTo(ignore, 1);
                    if (!fact.MakeGood(hand, hands.OtherHands(ignore)))
                        throw new Exception(string.Format("Can not make the hand {0} contain {1}.", hand, fact));
                    goto restart;
                }
            }
        }
    }
}
