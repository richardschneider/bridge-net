using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BlackHen.Bridge.Facts
{
    /// <summary>
    ///   High Card Points
    /// </summary>
    public struct Hcp : Fact
    {
        public byte Minimum;
        public byte Maximum;

        public Hcp(byte count)
           : this(count, count)
        {
        }

        public Hcp(byte min, byte max)
        {
            Minimum = min;
            Maximum = max;
        }

        public bool IsTrue(Hand hand)
        {
            var hcp = HcpEvaluator.GetHcp(hand);
            return Minimum <= hcp && hcp <= Maximum;
        }

        public bool MakeGood(Hand hand, HandCollection otherHands)
        {
            while (!IsTrue(hand))
            {
                int pointsNeeded = Minimum == Maximum ? Minimum : RandomThreaded.Generator.Next(Minimum, Maximum + 1);
                int delta = pointsNeeded - HcpEvaluator.GetHcp(hand);
                int ci;
                int oi;
                var other = otherHands[RandomThreaded.Generator.Next(0, otherHands.Count)];
                if (delta > 0)
                {
                    ci = hand.Cards.FindIndexRandom(card => !card.IsBiddingHonour);
                    if (ci < 0)
                        return false;
                    oi = other.Cards.FindIndexRandom(card => card.IsBiddingHonour);
                    if (oi < 0)
                        continue;
                }
                else
                {
                    ci = hand.Cards.FindIndexRandom(card => card.IsBiddingHonour); // FindRandomIndex
                    if (ci < 0)
                        return false;
                    oi = other.Cards.FindIndexRandom(card => !card.IsBiddingHonour);
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
            var s = new StringBuilder("Hcp(");
            s.Append(Minimum.ToString(CultureInfo.InvariantCulture));
            if (Minimum != Maximum)
            {
                s.Append(", ");
                s.Append(Maximum.ToString(CultureInfo.InvariantCulture));
            }
            s.Append(")");

            return s.ToString();
        }
    }

    public class HcpEvaluator : IHandEvaluator
    {
        /// <summary>
        ///   Returns the High Card Points for the specified <see cref="Hand"/>.
        /// </summary>
        /// <param name="hand">
        ///   The <see cref="Hand"/> to count.
        /// </param>
        /// <returns>
        ///   The High Card Points (HCP) for the specified <paramref name="hand"/>.
        /// </returns>
        /// <remarks>
        ///   <see cref="Rank.Ace"/> = 4, <see cref="Rank.King"/> = 3, <see cref="Rank.Queen"/> = 2
        ///   and <see cref="Rank.Jack"/> = 1; for a total of 40 points.
        /// </remarks>
        static public byte GetHcp(Hand hand)
        {
            byte hcp = 0;
            foreach (var card in hand.Cards)
            {
                switch (card.Rank)
                {
                    case Rank.Ace: hcp += 4; break;
                    case Rank.King: hcp += 3; break;
                    case Rank.Queen: hcp += 2; break;
                    case Rank.Jack: hcp += 1; break;
                }
            }

            return hcp;
        }

        /// <summary>
        ///   Returns the High Card Points for the specified <see cref="Suit"/> in the <see cref="Hand"/>.
        /// </summary>
        /// <param name="suit">
        ///   The <see cref="Suit"/> to count.
        /// </param>
        /// <param name="hand">
        ///   The <see cref="Hand"/> to count.
        /// </param>
        /// <returns>
        ///   The High Card Points (HCP) for the specified <paramref name="suit"/>
        ///   in the <paramref name="hand"/>.
        /// </returns>
        /// <remarks>
        ///   <see cref="Rank.Ace"/> = 4, <see cref="Rank.King"/> = 3, <see cref="Rank.Queen"/> = 2
        ///   and <see cref="Rank.Jack"/> = 1; for a total of 10 points.
        /// </remarks>
        static public byte GetHcp(Hand hand, Suit suit)
        {
            byte hcp = 0;
            foreach (var card in hand.Cards)
            {
                if (card.Suit == suit)
                {
                    switch (card.Rank)
                    {
                        case Rank.Ace: hcp += 4; break;
                        case Rank.King: hcp += 3; break;
                        case Rank.Queen: hcp += 2; break;
                        case Rank.Jack: hcp += 1; break;
                    }
                }
            }
            return hcp;
        }

        public List<Fact> Evaluate(Hand hand)
        {
            var facts = new List<Fact>(1);
            facts.Add(new Hcp(GetHcp(hand)));
            return facts;
        }
    }
}
