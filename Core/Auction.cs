using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Makaretu.Bridge
{
    /// <summary>
    ///   The history of the <see cref="Bid">bids</see>.
    /// </summary>
    public class Auction : List<Bid>
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="Auction"/> with the specified
        ///   <see cref="Dealer"/>.
        /// </summary>
        /// <param name="dealer">
        ///   The <see cref="Seat"/> who deals the cards and bids first.
        /// </param>
        public Auction(Seat dealer)
        {
            Dealer = dealer;
        }

        /// <summary>
        ///   The player who deals the cards and bids first.
        /// </summary>
        public Seat Dealer { get; private set; }

        /// <summary>
        ///   Determines the final contract based on the <see cref="BiddingHistory"/>.
        /// </summary>
        /// <returns></returns>
        public Contract FinalContract()
        {
            if (Count == 0)
                throw new NotSupportedException("No bids have been made.");

            Risk risk = Risk.None;
            int passes = 0;
            for (int i = Count - 1; 0 <= i; --i)
            {
                Bid bid = this[i];
                if (bid == Bid.Double)
                    risk = Risk.Doubled;
                else if (bid == Bid.Redouble)
                    risk = Risk.Redoubled;
                else if (bid == Bid.Pass)
                    ++passes;
                else
                {
                    if (passes != 3)
                        throw new NotSupportedException("Bidding has not finished.");
                    Seat declaror = Dealer;
                    Seat finalBidder = (Seat)((i + (int)Dealer) % 4);
                    foreach (Bid b in this)
                    {
                        if (b.Denomination == bid.Denomination &&
                           (declaror == finalBidder || declaror == Board.Partner(finalBidder)))
                            break;
                        declaror = Board.NextSeat(declaror);
                    }
                    return new Contract(bid.Level, bid.Denomination, risk, declaror);
                }
            }

            if (passes != 4)
                throw new NotSupportedException("Bidding has not finished.");
            return Contract.PassedIn;
        }
    }
}
