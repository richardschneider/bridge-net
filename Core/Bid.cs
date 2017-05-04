using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackHen.Bridge
{
   /// <summary>
   ///   Component of a <see cref="Bid"/> that denotes the proposed trump suit or notrump;
   ///   also known as a strain.
   /// </summary>
   /// <remarks>
   ///   The <see cref="Suit"/> enumeration is a subset of <b>Denomination</b> and contains
   ///   the same values.
   /// </remarks>
   /// <seealso cref="Contract"/>
   public enum Denomination : byte
   {
      Undefined = 0,
      Clubs = 1, Diamonds, Hearts, Spades,
      NoTrumps
   }

   /// <summary>
   ///   A declaration of both <see cref="Level"/> and <see cref="Denomination"/> that suggests a final contract. Some bids are 
   ///   instead used as conventions: they carry coded messages and are not normally intended as final contracts.
   /// </summary>
   public class Bid
   {
      public static Bid Pass = new Bid(-1, Denomination.Undefined);
      public static Bid Double = new Bid(-2, Denomination.Undefined);
      public static Bid Redouble = new Bid(-3, Denomination.Undefined);

      public Bid(int level, Denomination denomination)
      {
         Level = (byte) level;
         Denomination = denomination;
      }

      public Bid(int level, Suit denomination)
      {
         Level = (byte)level;
         Denomination = (Denomination) denomination;
      }

      /// <summary>
      ///   The number of <see cref="Tricks"/> that (when added to the book of six tricks) a bid or contract states will be taken. 
      /// </summary> 
      /// <value>
      ///   A value between 1 and 7.  For special bids (<see cref="Pass"/>, <see cref="Doubble"/> and
      ///   <see cref="Redouble"/>) this is a negative number.
      /// </value>
      /// <remarks>
      ///   For example, a bid at the four level contracts to take (6 + 4) = 10 tricks. 
      /// </remarks>
      public byte Level { get; private set; }

      /// <summary>
      ///   The number of tricks required to make the <see cref="Bid"/>.
      /// </summary>
      /// <remarks> 
      ///   This is simply <see cref="Level"/>+6.
      /// </remarks>
      public int Tricks { get { return Level + 6; } }
      
      /// <summary>
      ///   The proposed trump suit or notrumps.
      /// </summary>
      /// <value>
      ///   One of the <see cref="BlackHen.Bridge.Denomination"/> enumeration values.
      /// </value>
      public Denomination Denomination { get; private set; }

      public override string ToString()
      {
         if (this == Pass) return "-";
         if (this == Double) return "X";
         if (this == Redouble) return "XX";
         string d;
         switch (Denomination)
         {
            case Denomination.Clubs: d = "C"; break;
            case Denomination.Diamonds: d = "D"; break;
            case Denomination.Hearts: d = "H"; break;
            case Denomination.Spades: d = "S"; break;
            case Denomination.NoTrumps: d = "NT"; break;
            default: d = "?"; break;
         }
         return Level.ToString() + d;
      }
   }
}
