using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackHen.Bridge
{
   [TestClass]
   public class HandTest
   {
      [TestMethod]
      public void DefaultProperties()
      {
         var hand = new Hand();
         Assert.AreEqual(0, hand.Facts.Count);
         Assert.AreEqual(0, hand.Cards.Count);
      }

      [TestMethod]
      public void Stringing()
      {
         var hand = new Hand();
         hand.Add(Card.Parse("KC"));
         hand.Add(Card.Parse("AC"));
         hand.Add(Card.Parse("6C"));
         hand.Add(Card.Parse("2H"));
         hand.Add(Card.Parse("QD"));
         hand.Add(Card.Parse("AD"));

         string n = Environment.NewLine;
         Assert.AreEqual("\u2660; \u2665 2; \u2666 AQ; \u2663 AK6", hand.ToString("G"));
         Assert.AreEqual("\u2660" + n + "\u2665 2" + n + "\u2666 AQ" + n + "\u2663 AK6", hand.ToString("N"));
      }

      [TestMethod]
      public void ToPbnString()
      {
         var hand = new Hand();
         hand.Add(Card.Parse("KS"));
         hand.Add(Card.Parse("QS"));
         hand.Add(Card.Parse("TS"));
         hand.Add(Card.Parse("2S"));
         hand.Add(Card.Parse("AH"));
         hand.Add(Card.Parse("TH"));
         hand.Add(Card.Parse("JD"));
         hand.Add(Card.Parse("6D"));
         hand.Add(Card.Parse("5D"));
         hand.Add(Card.Parse("4D"));
         hand.Add(Card.Parse("2D"));
         hand.Add(Card.Parse("8C"));
         hand.Add(Card.Parse("5C"));

         Assert.AreEqual("KQT2.AT.J6542.85", hand.ToString("P"));
      }

      [TestMethod]
      public void ParsePbn()
      {
         var hand = Hand.ParsePbn("KQT2.AT.J6542.85");

         CollectionAssert.Contains(hand.Cards, Card.Parse("KS"));
         CollectionAssert.Contains(hand.Cards, Card.Parse("QS"));
         CollectionAssert.Contains(hand.Cards, Card.Parse("TS"));
         CollectionAssert.Contains(hand.Cards, Card.Parse("2S"));
         CollectionAssert.Contains(hand.Cards, Card.Parse("AH"));
         CollectionAssert.Contains(hand.Cards, Card.Parse("TH"));
         CollectionAssert.Contains(hand.Cards, Card.Parse("JD"));
         CollectionAssert.Contains(hand.Cards, Card.Parse("6D"));
         CollectionAssert.Contains(hand.Cards, Card.Parse("5D"));
         CollectionAssert.Contains(hand.Cards, Card.Parse("4D"));
         CollectionAssert.Contains(hand.Cards, Card.Parse("2D"));
         CollectionAssert.Contains(hand.Cards, Card.Parse("8C"));
         CollectionAssert.Contains(hand.Cards, Card.Parse("5C"));
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

         Assert.AreEqual(0, hand.SuitLength(Suit.Spades));
         Assert.AreEqual(1, hand.SuitLength(Suit.Hearts));
         Assert.AreEqual(2, hand.SuitLength(Suit.Diamonds));
         Assert.AreEqual(3, hand.SuitLength(Suit.Clubs));
      }

   }
}
