using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackHen.Bridge.Facts
{
   [TestClass]
   public class SuitHcpTest
   {
      [TestMethod]
      public void Constructors()
      {
         var fact = new SuitHcp(2, Suit.Clubs);
         Assert.AreEqual(2, fact.Minimum);
         Assert.AreEqual(2, fact.Maximum);
         Assert.AreEqual(Suit.Clubs, fact.Suit);

         fact = new SuitHcp(2, 4, Suit.Clubs);
         Assert.AreEqual(2, fact.Minimum);
         Assert.AreEqual(4, fact.Maximum);
         Assert.AreEqual(Suit.Clubs, fact.Suit);
      }

      [TestMethod]
      public void Stringing()
      {
         var fact = new SuitHcp(2, Suit.Clubs);
         Assert.AreEqual("SuitHcp(2, Clubs)", fact.ToString());

         fact = new SuitHcp(2, 4, Suit.Clubs);
         Assert.AreEqual("SuitHcp(2, 4, Clubs)", fact.ToString());
      }

      [TestMethod]
      public void Truth()
      {
         var hand = new Hand();
         hand.Add(Card.Parse("KC"));
         hand.Add(Card.Parse("AC"));
         hand.Add(Card.Parse("6C"));
         hand.Add(Card.Parse("2H"));
         hand.Add(Card.Parse("JH"));
         hand.Add(Card.Parse("QD"));
         hand.Add(Card.Parse("AD"));

         Assert.AreEqual(true,  new SuitHcp(0, Suit.Spades).IsTrue(hand));
         Assert.AreEqual(true, new SuitHcp(0, 10, Suit.Spades).IsTrue(hand));
         Assert.AreEqual(false, new SuitHcp(1, 10, Suit.Spades).IsTrue(hand));

         Assert.AreEqual(true, new SuitHcp(1, Suit.Hearts).IsTrue(hand));
         Assert.AreEqual(false, new SuitHcp(2, 10, Suit.Hearts).IsTrue(hand));
         Assert.AreEqual(false, new SuitHcp(0, Suit.Hearts).IsTrue(hand));

         Assert.AreEqual(true, new SuitHcp(6, Suit.Diamonds).IsTrue(hand));
         Assert.AreEqual(true, new SuitHcp(0, 10, Suit.Diamonds).IsTrue(hand));
         Assert.AreEqual(false, new SuitHcp(0, 5, Suit.Diamonds).IsTrue(hand));
         Assert.AreEqual(false, new SuitHcp(7, 10, Suit.Diamonds).IsTrue(hand));

         Assert.AreEqual(true, new SuitHcp(7, Suit.Clubs).IsTrue(hand));
         Assert.AreEqual(true, new SuitHcp(0, 10, Suit.Clubs).IsTrue(hand));
         Assert.AreEqual(false, new SuitHcp(0, 6, Suit.Clubs).IsTrue(hand));
         Assert.AreEqual(false, new SuitHcp(8, 10, Suit.Clubs).IsTrue(hand));
      }

      [TestMethod]
      public void HandEvaluation()
      {
         var hand = new Hand();
         hand.Add(Card.Parse("KC"));
         hand.Add(Card.Parse("AC"));
         hand.Add(Card.Parse("6C"));
         hand.Add(Card.Parse("2H"));
         hand.Add(Card.Parse("JH"));
         hand.Add(Card.Parse("QD"));
         hand.Add(Card.Parse("AD"));

         var facts = new SuitHcpEvaluator().Evaluate(hand);
         Assert.AreEqual(4, facts.Count);
         Assert.AreEqual(new SuitHcp(0, Suit.Spades), facts[0]);
         Assert.AreEqual(new SuitHcp(1, Suit.Hearts), facts[1]);
         Assert.AreEqual(new SuitHcp(6, Suit.Diamonds), facts[2]);
         Assert.AreEqual(new SuitHcp(7, Suit.Clubs), facts[3]);
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
         var fact = new SuitHcp(6, Suit.Spades);
         Assert.IsTrue(fact.MakeGood(myHand, otherHands));
         Assert.AreEqual(6, HcpEvaluator.GetHcp(myHand, Suit.Spades));
         Console.WriteLine(board);

         new Deck()
            .Shuffle()
            .Deal(hands);
         fact = new SuitHcp(5, 9, Suit.Clubs);
         Assert.IsTrue(fact.MakeGood(myHand, otherHands));
         Console.WriteLine(board);
         int hcp = HcpEvaluator.GetHcp(myHand, Suit.Clubs);
         Assert.IsTrue(5 <= hcp && hcp <= 9);

         new Deck()
            .Shuffle()
            .Deal(hands);
         fact = new SuitHcp(14, Suit.Clubs);
         Assert.IsFalse(fact.MakeGood(myHand, otherHands));
      }

      [TestMethod]
      public void MakeGoodAllHands()
      {
         var board = new Board();
         var hands = board.Hands;
         hands[Seat.North].Facts.Add(new SuitHcp(6, 7, Suit.Spades));
         hands[Seat.South].Facts.Add(new SuitHcp(3, 4, Suit.Spades));
         hands[Seat.West].Facts.Add(new SuitHcp(2, 8, Suit.Hearts));
         hands[Seat.East].Facts.Add(new SuitHcp(10, Suit.Clubs));

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
