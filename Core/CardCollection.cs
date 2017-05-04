using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackHen.Bridge
{
   public class CardCollection : List<Card>
   {
      public CardCollection()
         : base(13)
      {
      }

      public int FindIndexRandom(Predicate<Card> match)
      {
         int count = this.Count;
         int index = RandomThreaded.Generator.Next(0, count);
         for (int i = count; 0 < i; --i)
         {
            if (index == count)
               index = 0;
            if (match(this[index]))
               return index;
            index++;
         }

         return -1;
      }
   }
}
