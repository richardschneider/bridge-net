using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BlackHen.Bridge.Facts
{
    public struct SuitHcp : Fact
    {
        public byte Minimum;
        public byte Maximum;
        public Suit Suit;

        public SuitHcp(byte count, Suit suit)
           : this(count, count, suit)
        {
        }

        public SuitHcp(byte min, byte max, Suit suit)
        {
            Minimum = min;
            Maximum = max;
            Suit = suit;
        }

        public bool IsTrue(Hand hand)
        {
            var v = HcpEvaluator.GetHcp(hand, Suit);
            return Minimum <= v && v <= Maximum;
        }

        public bool MakeGood(Hand hand, HandCollection otherHands)
        {
            var rng = RandomThreaded.Generator;

            if (this.Minimum > 10 || this.Maximum > 10)
                return false;

            Suit thisSuit = Suit;
            while (!IsTrue(hand))
            {
                int pointsNeeded = Minimum == Maximum ? Minimum : rng.Next(Minimum, Maximum + 1);
                int delta = pointsNeeded - HcpEvaluator.GetHcp(hand, thisSuit);
                int ci;
                int oi;
                var other = otherHands[rng.Next(0, otherHands.Count)];
                if (delta > 0)
                {
                    ci = hand.Cards.FindIndexRandom(card => !(card.Suit == thisSuit && card.IsBiddingHonour));
                    if (ci < 0)
                        return false;
                    oi = other.Cards.FindIndexRandom(card => card.Suit == thisSuit && card.IsBiddingHonour);
                    if (oi < 0)
                        continue;
                }
                else
                {
                    ci = hand.Cards.FindIndexRandom(card => card.Suit == thisSuit && card.IsBiddingHonour);
                    if (ci < 0)
                        return false;
                    oi = other.Cards.FindIndexRandom(card => !(card.Suit == thisSuit && card.IsBiddingHonour));
                    if (oi < 0)
                        continue;
                }
                Card t = hand.Cards[ci];
                hand.Cards[ci] = other.Cards[oi];
                other.Cards[oi] = t;
            }

            return true;
        }

        public override string ToString()
        {
            var s = new StringBuilder("SuitHcp(");
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

    public class SuitHcpEvaluator : IHandEvaluator
    {
        public List<Fact> Evaluate(Hand hand)
        {
            var facts = new List<Fact>(4);
            facts.Add(new SuitHcp(HcpEvaluator.GetHcp(hand, Suit.Spades), Suit.Spades));
            facts.Add(new SuitHcp(HcpEvaluator.GetHcp(hand, Suit.Hearts), Suit.Hearts));
            facts.Add(new SuitHcp(HcpEvaluator.GetHcp(hand, Suit.Diamonds), Suit.Diamonds));
            facts.Add(new SuitHcp(HcpEvaluator.GetHcp(hand, Suit.Clubs), Suit.Clubs));
            return facts;
        }
    }
}
