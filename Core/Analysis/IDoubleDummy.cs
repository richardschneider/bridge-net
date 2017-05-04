using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackHen.Bridge.Analysis
{
   /// <summary>
   ///   A double dummy algorithm is able to solve bridge problems by examining
   ///   all the cards in a <see cref="Board"/>.
   /// </summary>
   public interface IDoubleDummy
   {
      /// <summary>
      ///   Determines all the contracts that can be made for the specified <see cref="Board"/>.
      /// </summary>
      /// <param name="board">
      ///   The <see cref="Board"/> to analyse.
      /// </param>
      /// <returns>
      ///   A <see cref="DoubleDummySolution"/> that contains all the contracts that can 
      ///   be made.
      /// </returns>
      DoubleDummySolution MakeableContracts(Board board);
   }
}
