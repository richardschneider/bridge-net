using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackHen.Bridge
{
   /// <summary>
   ///   An organized duplicate bridge competition.
   /// </summary>
   public class Tournament
   {
      /// <summary>
      ///   Creates a new instance of the <see cref="Tournament"/> class.
      /// </summary>
      public Tournament()
      {
         Boards = new List<Board>(0);
      }

      /// <summary>
      ///   The boards (hands) used during the competition.
      /// </summary>
      /// <remarks>
      ///   A typical <b>Tournament</b> consists of between 24 and 28 <see cref="Board">Boards</see>.
      /// </remarks>
      /// <seealso cref="GenerateBoards"/>
      public List<Board> Boards { get; set; }

      /// <summary>
      ///   Generates the specified number of random <see cref="Boards"/> for the
      ///   <see cref="Tournament"/>.
      /// </summary>
      /// <param name="boardCount">
      ///    The number of <see cref="Boards"/> to create.
      /// </param>
      /// <remarks>
      ///   Each <see cref="Board"/> generated contains a monitonically increasing <see cref="Board.Number"/>
      ///   starting a 1.  The <see cref="Board.Dealer"/> and <see cref="Board.Vulnerability"/> of each
      ///   <see cref="Board"/> is set.
      /// </remarks>
      public void GenerateBoards(int boardCount)
      {
         Boards = new List<Board>(boardCount);
         DealGenerator dealGenerator = new DealGenerator();

         int v = 0;
         int shift = 0;
         int remaining = 4;
         for (int i = 0; i < boardCount; ++i)
         {
            var board = new Board();
            board.Number = i + 1;
            board.Dealer = (Seat)(i % 4);
            board.Vulnerability = (Vulnerability)(v % 4);
            dealGenerator.Generate(board.Hands);

            Boards.Add(board);
            v++;
            if (--remaining == 0)
            {
               v = ++shift;
               remaining = 4;
            }
         }
      }

   }
}
 