using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackHen.Bridge
{
   /// <summary>
   /// Summary description for UnitTest1
   /// </summary>
   [TestClass]
   public class CardTest
   {
      [TestMethod]
      public void Getting()
      {
         var card = Card.Get(Rank.Two, Suit.Spades);
         Assert.AreEqual(Rank.Two, card.Rank);
         Assert.AreEqual(Suit.Spades, card.Suit);

         card = Card.Get(Rank.Ace, Suit.Hearts);
         Assert.AreEqual(Rank.Ace, card.Rank);
         Assert.AreEqual(Suit.Hearts, card.Suit);

         card = Card.Get(Rank.Ten, Suit.Diamonds);
         Assert.AreEqual(Rank.Ten, card.Rank);
         Assert.AreEqual(Suit.Diamonds, card.Suit);

         card = Card.Get(Rank.Four, Suit.Clubs);
         Assert.AreEqual(Rank.Four, card.Rank);
         Assert.AreEqual(Suit.Clubs, card.Suit);
      }

      [TestMethod]
      public void Majors()
      {
         Assert.IsTrue(Card.Get(Rank.Ace, Suit.Spades).IsMajor);
         Assert.IsTrue(Card.Get(Rank.Ace, Suit.Hearts).IsMajor);
         Assert.IsFalse(Card.Get(Rank.Ace, Suit.Diamonds).IsMajor);
         Assert.IsFalse(Card.Get(Rank.Ace, Suit.Clubs).IsMajor);
      }

      [TestMethod]
      public void Minors()
      {
         Assert.IsFalse(Card.Get(Rank.Ace, Suit.Spades).IsMinor);
         Assert.IsFalse(Card.Get(Rank.Ace, Suit.Hearts).IsMinor);
         Assert.IsTrue(Card.Get(Rank.Ace, Suit.Diamonds).IsMinor);
         Assert.IsTrue(Card.Get(Rank.Ace, Suit.Clubs).IsMinor);
      }

      [TestMethod]
      public void BiddingHonours()
      {
         Assert.AreEqual(false, Card.Get(Rank.Two, Suit.Spades).IsBiddingHonour);
         Assert.AreEqual(false, Card.Get(Rank.Three, Suit.Spades).IsBiddingHonour);
         Assert.AreEqual(false, Card.Get(Rank.Four, Suit.Spades).IsBiddingHonour);
         Assert.AreEqual(false, Card.Get(Rank.Five, Suit.Spades).IsBiddingHonour);
         Assert.AreEqual(false, Card.Get(Rank.Six, Suit.Spades).IsBiddingHonour);
         Assert.AreEqual(false, Card.Get(Rank.Seven, Suit.Spades).IsBiddingHonour);
         Assert.AreEqual(false, Card.Get(Rank.Eight, Suit.Spades).IsBiddingHonour);
         Assert.AreEqual(false, Card.Get(Rank.Nine, Suit.Spades).IsBiddingHonour);
         Assert.AreEqual(false, Card.Get(Rank.Ten, Suit.Spades).IsBiddingHonour);
         Assert.AreEqual(true, Card.Get(Rank.Jack, Suit.Spades).IsBiddingHonour);
         Assert.AreEqual(true, Card.Get(Rank.Queen, Suit.Spades).IsBiddingHonour);
         Assert.AreEqual(true, Card.Get(Rank.King, Suit.Spades).IsBiddingHonour);
         Assert.AreEqual(true, Card.Get(Rank.Ace, Suit.Spades).IsBiddingHonour);
      }

      [TestMethod]
      public void PlayingHonours()
      {
         Assert.AreEqual(false, Card.Get(Rank.Two, Suit.Spades).IsPlayingHonour);
         Assert.AreEqual(false, Card.Get(Rank.Three, Suit.Spades).IsPlayingHonour);
         Assert.AreEqual(false, Card.Get(Rank.Four, Suit.Spades).IsPlayingHonour);
         Assert.AreEqual(false, Card.Get(Rank.Five, Suit.Spades).IsPlayingHonour);
         Assert.AreEqual(false, Card.Get(Rank.Six, Suit.Spades).IsPlayingHonour);
         Assert.AreEqual(false, Card.Get(Rank.Seven, Suit.Spades).IsPlayingHonour);
         Assert.AreEqual(false, Card.Get(Rank.Eight, Suit.Spades).IsPlayingHonour);
         Assert.AreEqual(false, Card.Get(Rank.Nine, Suit.Spades).IsPlayingHonour);
         Assert.AreEqual(true, Card.Get(Rank.Ten, Suit.Spades).IsPlayingHonour);
         Assert.AreEqual(true, Card.Get(Rank.Jack, Suit.Spades).IsPlayingHonour);
         Assert.AreEqual(true, Card.Get(Rank.Queen, Suit.Spades).IsPlayingHonour);
         Assert.AreEqual(true, Card.Get(Rank.King, Suit.Spades).IsPlayingHonour);
         Assert.AreEqual(true, Card.Get(Rank.Ace, Suit.Spades).IsPlayingHonour);
      }

      [TestMethod]
      public void ToStrings()
      {
         Assert.AreEqual("2S", Card.Get(Rank.Two, Suit.Spades).ToString());
         Assert.AreEqual("3S", Card.Get(Rank.Three, Suit.Spades).ToString());
         Assert.AreEqual("4S", Card.Get(Rank.Four, Suit.Spades).ToString());
         Assert.AreEqual("5S", Card.Get(Rank.Five, Suit.Spades).ToString());
         Assert.AreEqual("6S", Card.Get(Rank.Six, Suit.Spades).ToString());
         Assert.AreEqual("7S", Card.Get(Rank.Seven, Suit.Spades).ToString());
         Assert.AreEqual("8S", Card.Get(Rank.Eight, Suit.Spades).ToString());
         Assert.AreEqual("9S", Card.Get(Rank.Nine, Suit.Spades).ToString());
         Assert.AreEqual("10S", Card.Get(Rank.Ten, Suit.Spades).ToString());
         Assert.AreEqual("JS", Card.Get(Rank.Jack, Suit.Spades).ToString());
         Assert.AreEqual("QS", Card.Get(Rank.Queen, Suit.Spades).ToString());
         Assert.AreEqual("KS", Card.Get(Rank.King, Suit.Spades).ToString());
         Assert.AreEqual("AS", Card.Get(Rank.Ace, Suit.Spades).ToString());

         Assert.AreEqual("2H", Card.Get(Rank.Two, Suit.Hearts).ToString());
         Assert.AreEqual("3H", Card.Get(Rank.Three, Suit.Hearts).ToString());
         Assert.AreEqual("4H", Card.Get(Rank.Four, Suit.Hearts).ToString());
         Assert.AreEqual("5H", Card.Get(Rank.Five, Suit.Hearts).ToString());
         Assert.AreEqual("6H", Card.Get(Rank.Six, Suit.Hearts).ToString());
         Assert.AreEqual("7H", Card.Get(Rank.Seven, Suit.Hearts).ToString());
         Assert.AreEqual("8H", Card.Get(Rank.Eight, Suit.Hearts).ToString());
         Assert.AreEqual("9H", Card.Get(Rank.Nine, Suit.Hearts).ToString());
         Assert.AreEqual("10H", Card.Get(Rank.Ten, Suit.Hearts).ToString());
         Assert.AreEqual("JH", Card.Get(Rank.Jack, Suit.Hearts).ToString());
         Assert.AreEqual("QH", Card.Get(Rank.Queen, Suit.Hearts).ToString());
         Assert.AreEqual("KH", Card.Get(Rank.King, Suit.Hearts).ToString());
         Assert.AreEqual("AH", Card.Get(Rank.Ace, Suit.Hearts).ToString());

         Assert.AreEqual("2D", Card.Get(Rank.Two, Suit.Diamonds).ToString());
         Assert.AreEqual("3D", Card.Get(Rank.Three, Suit.Diamonds).ToString());
         Assert.AreEqual("4D", Card.Get(Rank.Four, Suit.Diamonds).ToString());
         Assert.AreEqual("5D", Card.Get(Rank.Five, Suit.Diamonds).ToString());
         Assert.AreEqual("6D", Card.Get(Rank.Six, Suit.Diamonds).ToString());
         Assert.AreEqual("7D", Card.Get(Rank.Seven, Suit.Diamonds).ToString());
         Assert.AreEqual("8D", Card.Get(Rank.Eight, Suit.Diamonds).ToString());
         Assert.AreEqual("9D", Card.Get(Rank.Nine, Suit.Diamonds).ToString());
         Assert.AreEqual("10D", Card.Get(Rank.Ten, Suit.Diamonds).ToString());
         Assert.AreEqual("JD", Card.Get(Rank.Jack, Suit.Diamonds).ToString());
         Assert.AreEqual("QD", Card.Get(Rank.Queen, Suit.Diamonds).ToString());
         Assert.AreEqual("KD", Card.Get(Rank.King, Suit.Diamonds).ToString());
         Assert.AreEqual("AD", Card.Get(Rank.Ace, Suit.Diamonds).ToString());

         Assert.AreEqual("2C", Card.Get(Rank.Two, Suit.Clubs).ToString());
         Assert.AreEqual("3C", Card.Get(Rank.Three, Suit.Clubs).ToString());
         Assert.AreEqual("4C", Card.Get(Rank.Four, Suit.Clubs).ToString());
         Assert.AreEqual("5C", Card.Get(Rank.Five, Suit.Clubs).ToString());
         Assert.AreEqual("6C", Card.Get(Rank.Six, Suit.Clubs).ToString());
         Assert.AreEqual("7C", Card.Get(Rank.Seven, Suit.Clubs).ToString());
         Assert.AreEqual("8C", Card.Get(Rank.Eight, Suit.Clubs).ToString());
         Assert.AreEqual("9C", Card.Get(Rank.Nine, Suit.Clubs).ToString());
         Assert.AreEqual("10C", Card.Get(Rank.Ten, Suit.Clubs).ToString());
         Assert.AreEqual("JC", Card.Get(Rank.Jack, Suit.Clubs).ToString());
         Assert.AreEqual("QC", Card.Get(Rank.Queen, Suit.Clubs).ToString());
         Assert.AreEqual("KC", Card.Get(Rank.King, Suit.Clubs).ToString());
         Assert.AreEqual("AC", Card.Get(Rank.Ace, Suit.Clubs).ToString());

         Assert.AreEqual("Undefined", Card.Undefined.ToString());
      }

      [TestMethod]
      public void ToGeneralStrings()
      {
         Assert.AreEqual("2S", Card.Get(Rank.Two, Suit.Spades).ToString("G"));
         Assert.AreEqual("3S", Card.Get(Rank.Three, Suit.Spades).ToString("G"));
         Assert.AreEqual("4S", Card.Get(Rank.Four, Suit.Spades).ToString("G"));
         Assert.AreEqual("5S", Card.Get(Rank.Five, Suit.Spades).ToString("G"));
         Assert.AreEqual("6S", Card.Get(Rank.Six, Suit.Spades).ToString("G"));
         Assert.AreEqual("7S", Card.Get(Rank.Seven, Suit.Spades).ToString("G"));
         Assert.AreEqual("8S", Card.Get(Rank.Eight, Suit.Spades).ToString("G"));
         Assert.AreEqual("9S", Card.Get(Rank.Nine, Suit.Spades).ToString("G"));
         Assert.AreEqual("10S", Card.Get(Rank.Ten, Suit.Spades).ToString("G"));
         Assert.AreEqual("JS", Card.Get(Rank.Jack, Suit.Spades).ToString("G"));
         Assert.AreEqual("QS", Card.Get(Rank.Queen, Suit.Spades).ToString("G"));
         Assert.AreEqual("KS", Card.Get(Rank.King, Suit.Spades).ToString("G"));
         Assert.AreEqual("AS", Card.Get(Rank.Ace, Suit.Spades).ToString("G"));

         Assert.AreEqual("2H", Card.Get(Rank.Two, Suit.Hearts).ToString("G"));
         Assert.AreEqual("3H", Card.Get(Rank.Three, Suit.Hearts).ToString("G"));
         Assert.AreEqual("4H", Card.Get(Rank.Four, Suit.Hearts).ToString("G"));
         Assert.AreEqual("5H", Card.Get(Rank.Five, Suit.Hearts).ToString("G"));
         Assert.AreEqual("6H", Card.Get(Rank.Six, Suit.Hearts).ToString("G"));
         Assert.AreEqual("7H", Card.Get(Rank.Seven, Suit.Hearts).ToString("G"));
         Assert.AreEqual("8H", Card.Get(Rank.Eight, Suit.Hearts).ToString("G"));
         Assert.AreEqual("9H", Card.Get(Rank.Nine, Suit.Hearts).ToString("G"));
         Assert.AreEqual("10H", Card.Get(Rank.Ten, Suit.Hearts).ToString("G"));
         Assert.AreEqual("JH", Card.Get(Rank.Jack, Suit.Hearts).ToString("G"));
         Assert.AreEqual("QH", Card.Get(Rank.Queen, Suit.Hearts).ToString("G"));
         Assert.AreEqual("KH", Card.Get(Rank.King, Suit.Hearts).ToString("G"));
         Assert.AreEqual("AH", Card.Get(Rank.Ace, Suit.Hearts).ToString("G"));

         Assert.AreEqual("2D", Card.Get(Rank.Two, Suit.Diamonds).ToString("G"));
         Assert.AreEqual("3D", Card.Get(Rank.Three, Suit.Diamonds).ToString("G"));
         Assert.AreEqual("4D", Card.Get(Rank.Four, Suit.Diamonds).ToString("G"));
         Assert.AreEqual("5D", Card.Get(Rank.Five, Suit.Diamonds).ToString("G"));
         Assert.AreEqual("6D", Card.Get(Rank.Six, Suit.Diamonds).ToString("G"));
         Assert.AreEqual("7D", Card.Get(Rank.Seven, Suit.Diamonds).ToString("G"));
         Assert.AreEqual("8D", Card.Get(Rank.Eight, Suit.Diamonds).ToString("G"));
         Assert.AreEqual("9D", Card.Get(Rank.Nine, Suit.Diamonds).ToString("G"));
         Assert.AreEqual("10D", Card.Get(Rank.Ten, Suit.Diamonds).ToString("G"));
         Assert.AreEqual("JD", Card.Get(Rank.Jack, Suit.Diamonds).ToString("G"));
         Assert.AreEqual("QD", Card.Get(Rank.Queen, Suit.Diamonds).ToString("G"));
         Assert.AreEqual("KD", Card.Get(Rank.King, Suit.Diamonds).ToString("G"));
         Assert.AreEqual("AD", Card.Get(Rank.Ace, Suit.Diamonds).ToString("G"));

         Assert.AreEqual("2C", Card.Get(Rank.Two, Suit.Clubs).ToString("G"));
         Assert.AreEqual("3C", Card.Get(Rank.Three, Suit.Clubs).ToString("G"));
         Assert.AreEqual("4C", Card.Get(Rank.Four, Suit.Clubs).ToString("G"));
         Assert.AreEqual("5C", Card.Get(Rank.Five, Suit.Clubs).ToString("G"));
         Assert.AreEqual("6C", Card.Get(Rank.Six, Suit.Clubs).ToString("G"));
         Assert.AreEqual("7C", Card.Get(Rank.Seven, Suit.Clubs).ToString("G"));
         Assert.AreEqual("8C", Card.Get(Rank.Eight, Suit.Clubs).ToString("G"));
         Assert.AreEqual("9C", Card.Get(Rank.Nine, Suit.Clubs).ToString("G"));
         Assert.AreEqual("10C", Card.Get(Rank.Ten, Suit.Clubs).ToString("G"));
         Assert.AreEqual("JC", Card.Get(Rank.Jack, Suit.Clubs).ToString("G"));
         Assert.AreEqual("QC", Card.Get(Rank.Queen, Suit.Clubs).ToString("G"));
         Assert.AreEqual("KC", Card.Get(Rank.King, Suit.Clubs).ToString("G"));
         Assert.AreEqual("AC", Card.Get(Rank.Ace, Suit.Clubs).ToString("G"));

         Assert.AreEqual("Undefined", Card.Undefined.ToString("G"));
      }

      [TestMethod]
      public void ToPbnStrings()
      {
         Assert.AreEqual("S2", Card.Get(Rank.Two, Suit.Spades).ToString("P"));
         Assert.AreEqual("S3", Card.Get(Rank.Three, Suit.Spades).ToString("P"));
         Assert.AreEqual("S4", Card.Get(Rank.Four, Suit.Spades).ToString("P"));
         Assert.AreEqual("S5", Card.Get(Rank.Five, Suit.Spades).ToString("P"));
         Assert.AreEqual("S6", Card.Get(Rank.Six, Suit.Spades).ToString("P"));
         Assert.AreEqual("S7", Card.Get(Rank.Seven, Suit.Spades).ToString("P"));
         Assert.AreEqual("S8", Card.Get(Rank.Eight, Suit.Spades).ToString("P"));
         Assert.AreEqual("S9", Card.Get(Rank.Nine, Suit.Spades).ToString("P"));
         Assert.AreEqual("ST", Card.Get(Rank.Ten, Suit.Spades).ToString("P"));
         Assert.AreEqual("SJ", Card.Get(Rank.Jack, Suit.Spades).ToString("P"));
         Assert.AreEqual("SQ", Card.Get(Rank.Queen, Suit.Spades).ToString("P"));
         Assert.AreEqual("SK", Card.Get(Rank.King, Suit.Spades).ToString("P"));
         Assert.AreEqual("SA", Card.Get(Rank.Ace, Suit.Spades).ToString("P"));

         Assert.AreEqual("H2", Card.Get(Rank.Two, Suit.Hearts).ToString("P"));
         Assert.AreEqual("H3", Card.Get(Rank.Three, Suit.Hearts).ToString("P"));
         Assert.AreEqual("H4", Card.Get(Rank.Four, Suit.Hearts).ToString("P"));
         Assert.AreEqual("H5", Card.Get(Rank.Five, Suit.Hearts).ToString("P"));
         Assert.AreEqual("H6", Card.Get(Rank.Six, Suit.Hearts).ToString("P"));
         Assert.AreEqual("H7", Card.Get(Rank.Seven, Suit.Hearts).ToString("P"));
         Assert.AreEqual("H8", Card.Get(Rank.Eight, Suit.Hearts).ToString("P"));
         Assert.AreEqual("H9", Card.Get(Rank.Nine, Suit.Hearts).ToString("P"));
         Assert.AreEqual("HT", Card.Get(Rank.Ten, Suit.Hearts).ToString("P"));
         Assert.AreEqual("HJ", Card.Get(Rank.Jack, Suit.Hearts).ToString("P"));
         Assert.AreEqual("HQ", Card.Get(Rank.Queen, Suit.Hearts).ToString("P"));
         Assert.AreEqual("HK", Card.Get(Rank.King, Suit.Hearts).ToString("P"));
         Assert.AreEqual("HA", Card.Get(Rank.Ace, Suit.Hearts).ToString("P"));

         Assert.AreEqual("D2", Card.Get(Rank.Two, Suit.Diamonds).ToString("P"));
         Assert.AreEqual("D3", Card.Get(Rank.Three, Suit.Diamonds).ToString("P"));
         Assert.AreEqual("D4", Card.Get(Rank.Four, Suit.Diamonds).ToString("P"));
         Assert.AreEqual("D5", Card.Get(Rank.Five, Suit.Diamonds).ToString("P"));
         Assert.AreEqual("D6", Card.Get(Rank.Six, Suit.Diamonds).ToString("P"));
         Assert.AreEqual("D7", Card.Get(Rank.Seven, Suit.Diamonds).ToString("P"));
         Assert.AreEqual("D8", Card.Get(Rank.Eight, Suit.Diamonds).ToString("P"));
         Assert.AreEqual("D9", Card.Get(Rank.Nine, Suit.Diamonds).ToString("P"));
         Assert.AreEqual("DT", Card.Get(Rank.Ten, Suit.Diamonds).ToString("P"));
         Assert.AreEqual("DJ", Card.Get(Rank.Jack, Suit.Diamonds).ToString("P"));
         Assert.AreEqual("DQ", Card.Get(Rank.Queen, Suit.Diamonds).ToString("P"));
         Assert.AreEqual("DK", Card.Get(Rank.King, Suit.Diamonds).ToString("P"));
         Assert.AreEqual("DA", Card.Get(Rank.Ace, Suit.Diamonds).ToString("P"));

         Assert.AreEqual("C2", Card.Get(Rank.Two, Suit.Clubs).ToString("P"));
         Assert.AreEqual("C3", Card.Get(Rank.Three, Suit.Clubs).ToString("P"));
         Assert.AreEqual("C4", Card.Get(Rank.Four, Suit.Clubs).ToString("P"));
         Assert.AreEqual("C5", Card.Get(Rank.Five, Suit.Clubs).ToString("P"));
         Assert.AreEqual("C6", Card.Get(Rank.Six, Suit.Clubs).ToString("P"));
         Assert.AreEqual("C7", Card.Get(Rank.Seven, Suit.Clubs).ToString("P"));
         Assert.AreEqual("C8", Card.Get(Rank.Eight, Suit.Clubs).ToString("P"));
         Assert.AreEqual("C9", Card.Get(Rank.Nine, Suit.Clubs).ToString("P"));
         Assert.AreEqual("CT", Card.Get(Rank.Ten, Suit.Clubs).ToString("P"));
         Assert.AreEqual("CJ", Card.Get(Rank.Jack, Suit.Clubs).ToString("P"));
         Assert.AreEqual("CQ", Card.Get(Rank.Queen, Suit.Clubs).ToString("P"));
         Assert.AreEqual("CK", Card.Get(Rank.King, Suit.Clubs).ToString("P"));
         Assert.AreEqual("CA", Card.Get(Rank.Ace, Suit.Clubs).ToString("P"));

         Assert.AreEqual("Undefined", Card.Undefined.ToString("P"));
      }

      [TestMethod]
      [ExpectedException(typeof(FormatException))]
      public void BadFormatSpecifier()
      {
         Card.Undefined.ToString("x");
      }

      [TestMethod]
      public void Parsing()
      {
         Assert.AreEqual(Rank.Two, Card.Parse("2S").Rank);
         Assert.AreEqual(Suit.Spades, Card.Parse("2S").Suit);
         Assert.AreEqual(Rank.Two, Card.Parse("S2").Rank);
         Assert.AreEqual(Suit.Spades, Card.Parse("S2").Suit);

         Assert.AreEqual(Rank.King, Card.Parse("KH").Rank);
         Assert.AreEqual(Suit.Hearts, Card.Parse("KH").Suit);
         Assert.AreEqual(Rank.King, Card.Parse("HK").Rank);
         Assert.AreEqual(Suit.Hearts, Card.Parse("HK").Suit);

         Assert.AreEqual(Rank.Ten, Card.Parse("TD").Rank);
         Assert.AreEqual(Suit.Diamonds, Card.Parse("TD").Suit);
         Assert.AreEqual(Rank.Ten, Card.Parse("DT").Rank);
         Assert.AreEqual(Suit.Diamonds, Card.Parse("DT").Suit);

         Assert.AreEqual(Rank.Ten, Card.Parse("10D").Rank);
         Assert.AreEqual(Suit.Diamonds, Card.Parse("10D").Suit);
         Assert.AreEqual(Rank.Ten, Card.Parse("D10").Rank);
         Assert.AreEqual(Suit.Diamonds, Card.Parse("D10").Suit);
      }

   }
}
