using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackHen.Bridge.Facts
{
   [TestClass]
   public class HcpTest
   {
      [TestMethod]
      public void Constructors()
      {
         var fact = new Hcp(2);
         Assert.AreEqual(2, fact.Minimum);
         Assert.AreEqual(2, fact.Maximum);

         fact = new Hcp(2, 4);
         Assert.AreEqual(2, fact.Minimum);
         Assert.AreEqual(4, fact.Maximum);
      }

      [TestMethod]
      public void Stringing()
      {
         var fact = new Hcp(2);
         Assert.AreEqual("Hcp(2)", fact.ToString());

         fact = new Hcp(2, 4);
         Assert.AreEqual("Hcp(2, 4)", fact.ToString());
      }

      [TestMethod]
      public void Hcp()
      {
         var hand = new Hand();
         hand.Add(Card.Parse("KC"));
         hand.Add(Card.Parse("AC"));
         hand.Add(Card.Parse("6C"));
         hand.Add(Card.Parse("2H"));
         hand.Add(Card.Parse("QD"));
         hand.Add(Card.Parse("AD"));

         Assert.AreEqual(true,  new Hcp(13).IsTrue(hand));
         Assert.AreEqual(true, new Hcp(13, 15).IsTrue(hand));
         Assert.AreEqual(true, new Hcp(10, 13).IsTrue(hand));
         Assert.AreEqual(true, new Hcp(10, 15).IsTrue(hand));

         Assert.AreEqual(false, new Hcp(10).IsTrue(hand));
         Assert.AreEqual(false, new Hcp(10, 12).IsTrue(hand));
         Assert.AreEqual(false, new Hcp(14, 15).IsTrue(hand));
      }

      [TestMethod]
      public void HandEvaluation()
      {
         var hand = new Hand();
         hand.Add(Card.Parse("KC"));
         hand.Add(Card.Parse("AC"));
         hand.Add(Card.Parse("6C"));
         hand.Add(Card.Parse("2H"));
         hand.Add(Card.Parse("QD"));
         hand.Add(Card.Parse("AD"));

         var facts = new HcpEvaluator().Evaluate(hand);
         Assert.AreEqual(1, facts.Count);
         Assert.AreEqual(new Hcp(13), facts[0]);
      }

      [TestMethod]
      public void SuitHcp()
      {
         var hand = new Hand();
         hand.Add(Card.Parse("KC"));
         hand.Add(Card.Parse("AC"));
         hand.Add(Card.Parse("6C"));
         hand.Add(Card.Parse("2H"));
         hand.Add(Card.Parse("JH"));
         hand.Add(Card.Parse("QD"));
         hand.Add(Card.Parse("AD"));

         Assert.AreEqual(0, HcpEvaluator.GetHcp(hand, Suit.Spades));
         Assert.AreEqual(1, HcpEvaluator.GetHcp(hand, Suit.Hearts));
         Assert.AreEqual(6, HcpEvaluator.GetHcp(hand, Suit.Diamonds));
         Assert.AreEqual(7, HcpEvaluator.GetHcp(hand, Suit.Clubs));
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
         var fact = new Hcp(23);
         Assert.IsTrue(fact.MakeGood(myHand, otherHands));
         Console.WriteLine(board);
         Assert.AreEqual(23, HcpEvaluator.GetHcp(myHand));

         new Deck()
            .Shuffle()
            .Deal(hands);
         fact = new Hcp(12, 14);
         Assert.IsTrue(fact.MakeGood(myHand, otherHands));
         Console.WriteLine(board);
         int hcp = HcpEvaluator.GetHcp(myHand);
         Assert.IsTrue(12 <= hcp && hcp <= 14);

         new Deck()
            .Shuffle()
            .Deal(hands);
         fact = new Hcp(42);
         Assert.IsFalse(fact.MakeGood(myHand, otherHands));
      }


   }
}
