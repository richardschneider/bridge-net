using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackHen.Bridge
{
    public interface IHandEvaluator
    {
        List<Fact> Evaluate(Hand hand);
    }
}
