using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BlackHen.Bridge.Facts
{
    public struct SuitLength : Fact
    {
        public byte Minimum;
        public byte Maximum;
        public Suit Suit;

        public SuitLength(byte count, Suit suit)
           : this(count, count, suit)
        {
        }

        public SuitLength(byte min, byte max, Suit suit)
        {
            Minimum = min;
            Maximum = max;
            Suit = suit;
        }

        public bool IsTrue(Hand hand)
        {
            var v = hand.SuitLength(Suit);
            return Minimum <= v && v <= Maximum;
        }

        public bool MakeGood(Hand hand, HandCollection otherHands)
        {
            var rng = RandomThreaded.Generator;

            if (this.Minimum > Card.CardsInSuit || this.Maximum > Card.CardsInSuit)
                return false;

            int cardsNeeded = Minimum == Maximum ? Minimum : rng.Next(Minimum, Maximum + 1);
            int extras = cardsNeeded - hand.SuitLength(Suit);
            var thisSuit = Suit;
            while (extras != 0)
            {
                int ci;
                int oi;
                var other = otherHands[rng.Next(0, otherHands.Count)];
                if (extras > 0)
                {
                    ci = hand.Cards.FindIndexRandom(card => thisSuit != card.Suit);
                    if (ci < 0)
                        return false;
                    oi = other.Cards.FindIndexRandom(card => thisSuit == card.Suit);
                    if (oi < 0)
                        continue;
                    --extras;
                }
                else
                {
                    ci = hand.Cards.FindIndexRandom(card => thisSuit == card.Suit);
                    if (ci < 0)
                        return false;
                    oi = other.Cards.FindIndexRandom(card => thisSuit != card.Suit);
                    if (oi < 0)
                        continue;
                    ++extras;
                }
                Card t = hand.Cards[ci];
                hand.Cards[ci] = other.Cards[oi];
                other.Cards[oi] = t;
            }

            return true;
        }

        public override string ToString()
        {
            var s = new StringBuilder("SuitLength(");
            s.Append(Minimum.ToString(CultureInfo.InvariantCulture));
            s.Append(", ");
            if (Minimum != Maximum)
            {
                s.Append(Maximum.ToString(CultureInfo.InvariantCulture));
                s.Append(", ");
            }
            s.Append(Suit.ToString());
            s.Append(")");

            return s.ToString();

        }
    }

    public class SuitLengthEvaluator : IHandEvaluator
    {
        public List<Fact> Evaluate(Hand hand)
        {
            var facts = new List<Fact>(4);
            facts.Add(new SuitLength(hand.SuitLength(Suit.Spades), Suit.Spades));
            facts.Add(new SuitLength(hand.SuitLength(Suit.Hearts), Suit.Hearts));
            facts.Add(new SuitLength(hand.SuitLength(Suit.Diamonds), Suit.Diamonds));
            facts.Add(new SuitLength(hand.SuitLength(Suit.Clubs), Suit.Clubs));
            return facts;
        }
    }
}
