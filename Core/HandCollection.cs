using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Makaretu.Bridge
{
    public class HandCollection : List<Hand>
    {
        public HandCollection()
           : base(Board.SeatCount)
        {
            for (int i = 0; i < Board.SeatCount; ++i)
                this.Add(new Hand());
        }

        public static Seat NextSeat(Seat seat)
        {
            return (Seat)((int)(seat + 1) % Board.SeatCount);
        }

        public Hand this[Seat seat]
        {
            get { return this[(int)seat]; }
            private set { this[(int)seat] = value; }
        }

        public HandCollection OtherHands(Seat seat)
        {
            var otherHands = new HandCollection();
            for (int i = 0; i < this.Count; ++i)
                otherHands[i] = this[i];
            otherHands[(int)seat] = new Hand();

            return otherHands;
        }

        public HandCollection OtherHands(Hand hand)
        {
            var otherHands = new HandCollection();
            for (int i = 0; i < this.Count; ++i)
                otherHands[i] = hand == this[i] ? new Hand() : this[i];

            return otherHands;
        }

        public HandCollection OtherHands(params Hand[] hands)
        {
            var otherHands = new HandCollection();
            for (int i = 0; i < this.Count; ++i)
                otherHands[i] = hands.Contains<Hand>(this[i]) ? new Hand() : this[i];

            return otherHands;
        }

        /// <summary>
        ///   Returns the hands as a PBN deal string.
        /// </summary>
        ///  <param name="dealer"></param>
        ///  <seealso cref="ParsePbn"/>
        public string ToPbn(Seat dealer = Seat.South)
        {
            var pbn = new StringBuilder();
            pbn.Append(dealer.ToString()[0]);
            pbn.Append(':');

            for (int i = 0; i < Board.SeatCount; ++i)
            {
                if (i != 0)
                {
                    pbn.Append(' ');
                }
                pbn.Append(this[dealer].ToString("P"));
                dealer = NextSeat(dealer);
            }

            return pbn.ToString();
        }

        public static HandCollection ParsePbn(string s)
        {
            if (s.IndexOf(':') != 1)
                throw new FormatException("Missing the 'first' delimiter.");
            Seat seat;
            switch (s[0])
            {
                case 'N': seat = Seat.North; break;
                case 'E': seat = Seat.East; break;
                case 'S': seat = Seat.South; break;
                case 'W': seat = Seat.West; break;
                default: throw new FormatException("Invalid 'first' valid.");
            }
            string[] handStrings = s.Substring(2).Split(' ');
            if (handStrings.Length != 4)
                throw new FormatException("Not all four hands are defined.");

            var hands = new HandCollection();
            foreach (string cards in handStrings)
            {
                hands[seat] = cards == "-" ? new Hand() : Hand.ParsePbn(cards);
                seat = NextSeat(seat);
            }

            return hands;
        }

    }
}
