using BlackHen.Bridge.Facts;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackHen.Bridge
{
   [TestClass]
   public class HandGeneratorTest
   {
      [TestMethod]
      public void Generator()
      {
         var board = new Board();
         var hands = board.Hands;
         hands[Seat.North].Facts.Add(new Hcp(18));
         hands[Seat.North].Facts.Add(new SuitLength(6, Suit.Spades));
         hands[Seat.South].Facts.Add(new SuitLength(2, 4, Suit.Spades));
         hands[Seat.South].Facts.Add(new Hcp(4, 10));

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

      [TestMethod]
      public void GeneratorDeclaror()
      {
         var board = new Board();
         var hands = board.Hands;
         var declaror = hands[Seat.South];
         declaror.Facts.Add(new SuitLength(6, Suit.Spades));
         declaror.Facts.Add(new Hcp(6, 9));
         new DealGenerator().Generate(hands);
         Console.WriteLine("Original Hand");
         Console.WriteLine(board);
         foreach (var hand in hands)
         {
            foreach (var fact in hand.Facts)
            {
               Assert.IsTrue(fact.IsTrue(hand), fact.ToString());
            }
         }
         var myHand = declaror.ToString();

         new DealGenerator().Generate(hands, declaror);
         Console.WriteLine("Generated Hand");
         Console.WriteLine(board);
         foreach (var hand in hands)
         {
            foreach (var fact in hand.Facts)
            {
               Assert.IsTrue(fact.IsTrue(hand), fact.ToString());
            }
         }
         Assert.AreEqual(myHand, declaror.ToString());
      }
   }
}
