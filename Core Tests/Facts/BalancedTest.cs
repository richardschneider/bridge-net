using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackHen.Bridge.Facts
{
    [TestClass]
    public class BalancedTest
    {

        [TestMethod]
        public void Stringing()
        {
            var fact = new Balanced();
            Assert.AreEqual("Balanced", fact.ToString());
        }

        [TestMethod]
        public void BalancedPoints()
        {
            var hand = new Hand();
            hand.Add(Card.Parse("KC"));
            hand.Add(Card.Parse("AC"));
            hand.Add(Card.Parse("6C"));
            hand.Add(Card.Parse("2H"));
            hand.Add(Card.Parse("3H"));
            hand.Add(Card.Parse("QD"));
            hand.Add(Card.Parse("AD"));
            hand.Add(Card.Parse("2S"));
            hand.Add(Card.Parse("3S"));
            Assert.AreEqual(true, new Balanced().IsTrue(hand));

            hand = new Hand();
            hand.Add(Card.Parse("KC"));
            hand.Add(Card.Parse("AC"));
            hand.Add(Card.Parse("6C"));
            hand.Add(Card.Parse("AH"));
            hand.Add(Card.Parse("3H"));
            hand.Add(Card.Parse("QD"));
            hand.Add(Card.Parse("AD"));
            hand.Add(Card.Parse("2S"));
            hand.Add(Card.Parse("3S"));
            Assert.AreEqual(true, new Balanced().IsTrue(hand));
        }

        [TestMethod]
        public void BalancedMajors()
        {
            var hand = new Hand();
            hand.Add(Card.Parse("KC"));
            hand.Add(Card.Parse("AC"));
            hand.Add(Card.Parse("6C"));
            hand.Add(Card.Parse("2H"));
            hand.Add(Card.Parse("3H"));
            hand.Add(Card.Parse("QD"));
            hand.Add(Card.Parse("AD"));
            hand.Add(Card.Parse("2S"));
            hand.Add(Card.Parse("3S"));
            Assert.AreEqual(true, new Balanced().IsTrue(hand));

            hand = new Hand();
            hand.Add(Card.Parse("KC"));
            hand.Add(Card.Parse("AC"));
            hand.Add(Card.Parse("6C"));
            hand.Add(Card.Parse("AH"));
            hand.Add(Card.Parse("3H"));
            hand.Add(Card.Parse("QD"));
            hand.Add(Card.Parse("AD"));
            hand.Add(Card.Parse("2S"));
            hand.Add(Card.Parse("3S"));
            hand.Add(Card.Parse("4S"));
            hand.Add(Card.Parse("5S"));
            hand.Add(Card.Parse("6S"));
            Assert.AreEqual(false, new Balanced().IsTrue(hand));

            hand = new Hand();
            hand.Add(Card.Parse("KC"));
            hand.Add(Card.Parse("AC"));
            hand.Add(Card.Parse("6C"));
            hand.Add(Card.Parse("AH"));
            hand.Add(Card.Parse("3H"));
            hand.Add(Card.Parse("4H"));
            hand.Add(Card.Parse("5H"));
            hand.Add(Card.Parse("6H"));
            hand.Add(Card.Parse("QD"));
            hand.Add(Card.Parse("AD"));
            hand.Add(Card.Parse("2S"));
            hand.Add(Card.Parse("3S"));
            Assert.AreEqual(false, new Balanced().IsTrue(hand));
        }

        [TestMethod]
        public void HandEvaluation()
        {
            var hand = new Hand();
            hand.Add(Card.Parse("KC"));
            hand.Add(Card.Parse("AC"));
            hand.Add(Card.Parse("6C"));
            hand.Add(Card.Parse("2H"));
            hand.Add(Card.Parse("3H"));
            hand.Add(Card.Parse("QD"));
            hand.Add(Card.Parse("AD"));
            hand.Add(Card.Parse("2S"));
            hand.Add(Card.Parse("3S"));

            var facts = new BalancedEvaluator().Evaluate(hand);
            Assert.AreEqual(1, facts.Count);
            Assert.AreEqual(new Balanced(), facts[0]);
        }

        [TestMethod]
        public void MakeGoodOne()
        {
            var board = new Board();
            var hands = board.Hands;
            var myHand = hands[Seat.South];
            var otherHands = hands.OtherHands(Seat.South);

            new Deck()
               .Shuffle()
               .Deal(hands);
            var fact = new Balanced();
            Assert.IsTrue(fact.MakeGood(myHand, otherHands));
            Console.WriteLine(board);
            Assert.AreEqual(true, BalancedEvaluator.IsBalanced(myHand));
        }

        [TestMethod]
        public void MakeGoodAllHands()
        {
            var board = new Board();
            var hands = board.Hands;
            hands[Seat.North].Facts.Add(new Balanced());
            hands[Seat.West].Facts.Add(new Balanced());
            hands[Seat.East].Facts.Add(new Balanced());

            new DealGenerator().Generate(hands);
            Console.WriteLine(board);

            foreach (var hand in hands)
            {
                foreach (var fact in hand.Facts)
                {
                    Assert.IsTrue(fact.IsTrue(hand), fact.ToString());
                }
            }
        }

    }
}
