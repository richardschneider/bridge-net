using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BlackHen.Bridge.Facts
{
    /// <summary>
    ///   Balanced hand.
    /// </summary>
    public struct Balanced : Fact
    {
        public bool IsTrue(Hand hand)
        {
            return BalancedEvaluator.IsBalanced(hand);
        }

        public bool MakeGood(Hand hand, HandCollection otherHands)
        {
            restart:
            foreach (var rule in BalancedEvaluator.Rules)
            {
                if (rule.IsTrue(hand))
                    continue;
                rule.MakeGood(hand, otherHands);
                goto restart;
            }

            return true;
        }

        public override string ToString()
        {
            return "Balanced";
        }
    }

    public class BalancedEvaluator : IHandEvaluator
    {
        internal static Fact[] Rules = new Fact[]
        {
         //new Hcp(12, 14),
         new SuitLength(2, 4, Suit.Spades),
         new SuitLength(2, 4, Suit.Hearts),
         new SuitLength(2, 5, Suit.Diamonds),
         new SuitLength(2, 5, Suit.Clubs)
        };

        static public bool IsBalanced(Hand hand)
        {
            foreach (Fact f in Rules)
            {
                if (!f.IsTrue(hand))
                    return false;
            }

            return true;
        }

        public List<Fact> Evaluate(Hand hand)
        {
            List<Fact> facts;
            if (IsBalanced(hand))
            {
                facts = new List<Fact>(1);
                facts.Add(new Balanced());
            }
            else
            {
                facts = new List<Fact>(0);
            }

            return facts;
        }
    }
}
