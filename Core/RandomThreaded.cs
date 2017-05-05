using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Makaretu.Bridge
{
    /// <summary>
    ///   Thread safe <see cref="Random"/> number.
    /// </summary>
    /// <remarks>
    ///   The standard <see cref="Random">randon number generator</see> is not thread safe.
    ///   <para>
    ///   The <see cref="Generator"/> provides a RNG for each thread.  A global seed RNG is used
    ///   to initialise each thread specific RNG.
    ///   </para>
    /// </remarks>
    public class RandomThreaded
    {
        static Random seed = new Random();

        [ThreadStatic]
        static Random rng;

        public static Random Generator
        {
            get
            {
                if (rng == null)
                {
                    lock (seed)
                    {
                        if (rng == null)
                            rng = new Random(seed.Next());
                    }
                }
                return rng;
            }
        }
    }
}
