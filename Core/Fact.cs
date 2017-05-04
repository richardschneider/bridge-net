using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackHen.Bridge
{
   public interface Fact
   {
      bool IsTrue(Hand hand);
      bool MakeGood(Hand hand, HandCollection otherHands);
   }
}
