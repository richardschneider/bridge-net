using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackHen.Bridge.Facts
{
   [TestClass]
   public class SuitLengthTest
   {
      [TestMethod]
      public void Constructors()
      {
         var fact = new SuitLength(2, Suit.Clubs);
         Assert.AreEqual(2, fact.Minimum);
         Assert.AreEqual(2, fact.Maximum);
         Assert.AreEqual(Suit.Clubs, fact.Suit);

         fact = new SuitLength(2, 4, Suit.Clubs);
         Assert.AreEqual(2, fact.Minimum);
         Assert.AreEqual(4, fact.Maximum);
         Assert.AreEqual(Suit.Clubs, fact.Suit);
      }

      [TestMethod]
      public void Stringing()
      {
         var fact = new SuitLength(2, Suit.Clubs);
         Assert.AreEqual("SuitLength(2, Clubs)", fact.ToString());

         fact = new SuitLength(2, 4, Suit.Clubs);
         Assert.AreEqual("SuitLength(2, 4, Clubs)", fact.ToString());
      }

      [TestMethod]
      public void SuitLength()
      {
         var hand = new Hand();
         hand.Add(Card.Parse("KC"));
         hand.Add(Card.Parse("AC"));
         hand.Add(Card.Parse("6C"));
         hand.Add(Card.Parse("2H"));
         hand.Add(Card.Parse("QD"));
         hand.Add(Card.Parse("AD"));

         Assert.AreEqual(true,  new SuitLength(0, Suit.Spades).IsTrue(hand));
         Assert.AreEqual(false, new SuitLength(1, Suit.Spades).IsTrue(hand));
         Assert.AreEqual(true, new SuitLength(1, Suit.Hearts).IsTrue(hand));
         Assert.AreEqual(false, new SuitLength(2, Suit.Hearts).IsTrue(hand));
         Assert.AreEqual(true, new SuitLength(2, Suit.Diamonds).IsTrue(hand));
         Assert.AreEqual(false, new SuitLength(3, Suit.Diamonds).IsTrue(hand));
         Assert.AreEqual(true, new SuitLength(3, Suit.Clubs).IsTrue(hand));
         Assert.AreEqual(false, new SuitLength(4, Suit.Clubs).IsTrue(hand));
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

         var facts = new SuitLengthEvaluator().Evaluate(hand);
         Assert.AreEqual(4, facts.Count);
         Assert.AreEqual(new SuitLength(0, Suit.Spades), facts[0]);
         Assert.AreEqual(new SuitLength(1, Suit.Hearts), facts[1]);
         Assert.AreEqual(new SuitLength(2, Suit.Diamonds), facts[2]);
         Assert.AreEqual(new SuitLength(3, Suit.Clubs), facts[3]);
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
         var fact = new SuitLength(6, Suit.Spades);
         Assert.IsTrue(fact.MakeGood(myHand, otherHands));
         Assert.AreEqual(6, myHand.SuitLength(Suit.Spades));
         Console.WriteLine(board);

         new Deck()
            .Shuffle()
            .Deal(hands);
         fact = new SuitLength(10, 13, Suit.Clubs);
         Assert.IsTrue(fact.MakeGood(myHand, otherHands));
         Console.WriteLine(board);
         int l = myHand.SuitLength(Suit.Clubs);
         Assert.IsTrue(10 <= l && l <= 13);

         new Deck()
            .Shuffle()
            .Deal(hands);
         fact = new SuitLength(14, Suit.Clubs);
         Assert.IsFalse(fact.MakeGood(myHand, otherHands));
      }

      [TestMethod]
      public void MakeGoodAllHands()
      {
         var board = new Board();
         var hands = board.Hands;
         hands[Seat.North].Facts.Add(new SuitLength(6, Suit.Spades));
         hands[Seat.South].Facts.Add(new SuitLength(2, 4, Suit.Spades));
         hands[Seat.West].Facts.Add(new SuitLength(2, Suit.Hearts));
         hands[Seat.West].Facts.Add(new SuitLength(5, Suit.Diamonds));
         hands[Seat.East].Facts.Add(new SuitLength(2, 4, Suit.Spades));
         hands[Seat.East].Facts.Add(new SuitLength(2, 4, Suit.Hearts));
         hands[Seat.East].Facts.Add(new SuitLength(2, 4, Suit.Diamonds));
         hands[Seat.East].Facts.Add(new SuitLength(2, 4, Suit.Clubs));

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
