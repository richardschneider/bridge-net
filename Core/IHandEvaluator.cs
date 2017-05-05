using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Makaretu.Bridge
{
    public interface IHandEvaluator
    {
        List<Fact> Evaluate(Hand hand);
    }
}
