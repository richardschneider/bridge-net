using System;
using System.Collections.Generic;
using System.Text;

namespace BlackHen.Bridge
{

   /// <summary>
   ///   A scoring condition assigned to each pair in advance of a deal. 
   /// </summary>
   public enum Vulnerability : byte
   {
      None, NorthSouth, EastWest, All
   }

   public enum Seat : byte
   {
      North, East, South, West
   }

   public class Board
   {
      public const int SeatCount = 4;

      public Board()
      {
         Hands = new HandCollection();
      }

      /// <summary>
      ///   The scoring condition assigned to each pair in advance of a deal. 
      /// </summary>
      public Vulnerability Vulnerability { get; set; }

      /// <summary>
      ///   The player who deals the cards and bids first.
      /// </summary>
      public Seat Dealer { get; set; }

      public int Number { get; set; }

      public HandCollection Hands { get; set; }
      
      public static Seat NextSeat(Seat seat)
      {
         return (Seat) ((int) (seat + 1) % SeatCount);
      }

      public static Seat Partner(Seat seat)
      {
         return (Seat)((int)(seat + 2) % SeatCount);
      }

      public bool IsVulnerable(Seat seat)
      {
         switch (Vulnerability)
         {
            case Vulnerability.All: return true;
            case Vulnerability.EastWest: return seat == Seat.East || seat == Seat.West;
            case Vulnerability.NorthSouth: return seat == Seat.North || seat == Seat.South;
            default:
            case Vulnerability.None: return false;
         }
      }

      public override string ToString()
      {
         var lines = new string[12];

         int maxLength = 0;
         int i = 4;
         foreach (string holding in Hands[Seat.West].ToString().Split(';'))
         {
            var h = holding.Trim();
            if (h.Length > maxLength)
               maxLength = h.Length;
            lines[i++] = h;
         }

         var padding = new string(' ', maxLength + 1);
         maxLength = 0;
         i = 0;
         foreach (string holding in Hands[Seat.North].ToString().Split(';'))
         {
            var h = holding.Trim();
            if (h.Length > maxLength)
               maxLength = h.Length;
            lines[i++] = padding + h;
         }

         i = 8;
         foreach (string holding in Hands[Seat.South].ToString().Split(';'))
         {
            var h = holding.Trim();
            if (h.Length > maxLength)
               maxLength = h.Length;
            lines[i++] = padding + h;
         }

         i = 4;
         int fillTo = padding.Length + maxLength + 1;
         foreach (string holding in Hands[Seat.East].ToString().Split(';'))
         {
            var h = holding.Trim();
            lines[i] += new string(' ', fillTo - lines[i].Length) + h;
            ++i;
         }

         StringBuilder s = new StringBuilder();
         foreach (var line in lines)
            s.AppendLine(line);

         return s.ToString();
      }

      static public List<Board> CreateSet(int boardCount)
      {
         var boards = new List<Board>(boardCount);

         int v = 0;
         int shift = 0;
         int remaining = 4;
         for (int i = 0; i < boardCount; ++i)
         {
            var board = new Board();
            board.Number = i + 1;
            board.Dealer = (Seat)(i % 4);
            board.Vulnerability = (Vulnerability) (v % 4);

            boards.Add(board);
            v++;
            if (--remaining == 0)
            {
               v = ++shift;
               remaining = 4;
            }
         }

         return boards;
      }
   }
}
