using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackHen.Bridge
{
    public enum Risk : byte
    {
        None = 0, Doubled, Redoubled
    }

    /// <summary>
    ///   The statement of the pair who has won the bidding, that they will take at least the given number of tricks.
    /// </summary>
    public class Contract : Bid
    {
        public static Contract PassedIn = new Contract(0, Denomination.Undefined, Risk.None, Seat.North);

        public Contract(int level, Denomination denomination, Risk risk, Seat declaror)
           : base(level, denomination)
        {
            Risk = risk;
            Declaror = declaror;
        }

        /// <summary>
        ///   The risk of the <see cref="Contract"/>.
        /// </summary>
        /// <value>
        ///   One of the <see cref="BlackHen.Bridge.Risk"/> enumeration values.
        /// </value>
        public Risk Risk { get; private set; }

        /// <summary>
        ///   The player who first bid that contract's <see cref="Denomination"/>. The declarer plays the cards in 
        ///   his own hand as well as dummy's cards. 
        /// </summary>
        public Seat Declaror { get; private set; }

        public override string ToString()
        {
            if (this == PassedIn)
                return "-";

            string s = base.ToString();
            if (Risk == Risk.Doubled)
                return s + "X by " + Declaror.ToString();
            if (Risk == Risk.Redoubled)
                return s + "XX by " + Declaror.ToString();
            return s + " by " + Declaror.ToString();
        }
    }
}
