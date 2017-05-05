using System;
using System.Text;

namespace Makaretu.Bridge
{
    public enum Rank : byte
    {
        Undefined = 0,
        Two = 2, Three, Four, Five, Six, Seven, Eight, Nine,
        Ten,
        Jack, Queen, King, Ace
    }

    public enum Suit : byte
    {
        Undefined = 0,
        Clubs = 1, Diamonds, Hearts, Spades
    }

    /// <summary>
    ///   A card contains a <see cref="Rank"/> and <see cref="Suit"/>.
    /// </summary>
    /// <remarks>
    ///   <see cref="Card"/> instances are immutable and only 52 instance ever exist.
    ///   <para>
    ///   To obtain an instance of a <see cref="Card"/> use the <see cref="Get"/>
    ///   or <see cref="Parse"/> <c>static</c> methods.
    ///   </para>
    /// </remarks>
    public struct Card
    {
        /// <summary>
        ///   The number of cards in a <see cref="Suit"/>.
        /// </summary>
        public const int CardsInSuit = 13;

        /// <summary>
        ///   The number of suits.
        /// </summary>
        public const int SuitCount = 4;

        /// <summary>
        ///   The unicode points for each <see cref="Suit"/>.
        /// </summary>
        public const string SuitGlyph = "U\u2663\u2666\u2665\u2660";

        [Flags]
        enum Info : short
        {
            MajorSuit = 0x0001,
            BiddingHonour = 0x0002,
            PlayingHonour = 0x0004,
            Undefined1 = 0x0008,
            Undefined2 = 0x0010,
            Undefined3 = 0x0020,
            Undefined4 = 0x0040,
            Undefined5 = 0x0080
        }

        Rank rank;
        Suit suit;
        Info info;

        /// <summary>
        ///   Creates all the <see cref="Card"/> instances.
        /// </summary>
        static Card()
        {
            var cards = new Card[Deck.CardCount];
            int i = 0;
            for (int suit = 1; suit <= SuitCount; ++suit)
            {
                for (int rank = 2; rank < 15; ++rank)
                {
                    cards[i++] = new Card((Rank)rank, (Suit)suit);
                }
            }

            All = cards;

            SuitsDescending = new Suit[] { Suit.Spades, Suit.Hearts, Suit.Diamonds, Suit.Clubs };
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="Card"/> class with the specified
        ///   <see cref="Rank"/> and <see cref="Suit"/>.
        /// </summary>
        /// <param name="rank">
        ///   The <see cref="Makaretu.Bridge.Rank"/> of the card (2-10, J, Q, K and A).
        /// </param>
        /// <param name="suit">
        ///   The <see cref="Makaretu.Bridge.Suit"/> of the card (C, D, H and S).
        /// </param>
        private Card(Rank rank, Suit suit)
        {
            this.rank = rank;
            this.suit = suit;

            this.info = (suit == Suit.Hearts || suit == Suit.Spades) ? Info.MajorSuit : 0;
            this.info |= (Rank.Jack <= rank) ? Info.BiddingHonour : 0;
            this.info |= (Rank.Ten <= rank) ? Info.PlayingHonour : 0;
        }

        /// <summary>
        ///   Get all the <see cref="Card"/> instances.
        /// </summary>
        static internal Card[] All { get; private set; }

        /// <summary>
        ///   Get all the <see cref="Suit">suits</see> in descending order (highest first).
        /// </summary>
        static public Suit[] SuitsDescending { get; private set; }

        /// <summary>
        ///   Gets an instance of the <see cref="Card"/> class with the specified
        ///   <see cref="Rank"/> and <see cref="Suit"/>.
        /// </summary>
        /// <param name="rank">
        ///   The <see cref="Makaretu.Bridge.Rank"/> of the card (2-10, J, Q, K and A).
        /// </param>
        /// <param name="suit">
        ///   The <see cref="Makaretu.Bridge.Suit"/> of the card (C, D, H and S).
        /// </param>
        public static Card Get(Rank rank, Suit suit)
        {
            int i = (((int)suit - 1) * 13) + (int)rank - 2;
            return All[i];
        }

        public static Card Get(char rank, char suit)
        {
            return Card.Parse(new string(new char[] { rank, suit }));
        }

        /// <summary>
        ///   The "undefined" <see cref="Card"/>.
        /// </summary>
        /// <remarks>
        ///   The <see cref="Card"/> is a value not a class.  Thus a <b>null</b> cannot be used; however, this
        ///   value can do in a pinch.
        /// </remarks>
        public static Card Undefined = new Card(Rank.Undefined, Suit.Clubs);

        /// <summary>
        ///  Gets the first compoment of the card.
        /// </summary>
        /// <value>
        ///   One of the <see cref="Makaretu.Bridge.Rank"/> enumeration values.
        /// </value>
        public Rank Rank { get { return rank; } }

        /// <summary>
        ///   Gets the second compoment of the card.
        /// </summary>
        /// <value>
        ///   One of the <see cref="Makaretu.Bridge.Suit"/> enumeration values.
        /// </value>
        public Suit Suit { get { return suit; } }

        /// <summary>
        ///   Determines if the <see cref="Card"/> is a major <see cref="Suit"/>.
        /// </summary>
        /// <value>
        ///   <b>true</b> if the <see cref="Suit"/> is <see cref="Suit.Spades"/> or
        ///   <see cref="Suite.Hearts"/>.
        /// </value>
        public bool IsMajor { get { return (info & Info.MajorSuit) != 0; } }

        /// <summary>
        ///   Determines if the <see cref="Card"/> is a minor <see cref="Suit"/>.
        /// </summary>
        /// <value>
        ///   <b>true</b> if the <see cref="Suit"/> is <see cref="Suit.Diamonds"/> or
        ///   <see cref="Suite.Clubs"/>.
        /// </value>
        public bool IsMinor { get { return !IsMajor; } }

        public bool IsBiddingHonour { get { return (info & Info.BiddingHonour) != 0; } }
        public bool IsPlayingHonour { get { return (info & Info.PlayingHonour) != 0; } }

        #region Formatting/Parsing

        /// <summary>
        ///   Converts the value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>
        ///   The string representation of the value of this instance, consisting of its
        ///   <see cref="Rank"/> (2, 3, 4, 5, 6, 7, 8, 9, 10, J, Q, K, A) and its 
        ///   <see cref="Suit"/> (C, D, H, S).
        /// </returns>
        /// <seealso cref="Parse"/>
        public override string ToString()
        {
            return ToString("G", null);
        }

        /// <summary>
        ///   Converts the value of this instance to its equivalent string representation, 
        ///   using the specified format.
        /// </summary>
        /// <param name="format">
        ///   A format string. "G" for general, "P" for <see href="">Portable Bridge Notation</see>.
        /// </param>
        /// <returns>
        ///   The string representation of the value of this instance as specified by 
        ///   <paramref name="format"/>.
        /// </returns>
        /// <remarks>
        ///   If <paramref name="format"/> is a <b>null</b> reference or an empty string (""), the return 
        ///   value of this instance is formatted with the general format specifier ("G").
        ///   <para>
        ///   When <pararef name="format"/> is "G", the string representation consists of the
        ///   <see cref="Rank"/> (2, 3, 4, 5, 6, 7, 8, 9, 10, J, Q, K, A) and the
        ///   <see cref="Suit"/> (C, D, H, S).
        ///   </para>
        ///   <para>
        ///   When <pararef name="format"/> is "P", the string representation is the same as "G",
        ///   except that <see cref="Rank.Ten"/> is represented as "T".
        ///   </para>
        /// </remarks>
        /// <seealso cref="Parse"/>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        /// <summary>
        ///   Converts the value of this instance to its equivalent string representation, 
        ///   using the specified format.
        /// </summary>
        /// <param name="format">
        ///   A format string. "G" for general, "P" for <see href="">Portable Bridge Notation</see>.
        /// </param>
        /// <param name="provider">
        ///   Ignored.
        /// </param>
        /// <returns>
        ///   The string representation of the value of this instance as specified by 
        ///   <paramref name="format"/>.
        /// </returns>
        /// <remarks>
        ///   If <paramref name="format"/> is a <b>null</b> reference or an empty string (""), the return 
        ///   value of this instance is formatted with the general format specifier ("G").
        ///   <para>
        ///   When <pararef name="format"/> is "G", the string representation consists of the
        ///   <see cref="Rank"/> (2, 3, 4, 5, 6, 7, 8, 9, 10, J, Q, K, A) and the
        ///   <see cref="Suit"/> (C, D, H, S).
        ///   </para>
        ///   <para>
        ///   When <pararef name="format"/> is "P", the string representation consists of the
        ///   <see cref="Suit"/> (C, D, H, S) and the
        ///   <see cref="Rank"/> (2, 3, 4, 5, 6, 7, 8, 9, T, J, Q, K, A).
        ///   </para>
        ///   <para>
        ///   </para>
        /// </remarks>
        /// <seealso cref="Parse"/>
        public string ToString(string format, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(format))
                format = "G";

            StringBuilder s = new StringBuilder(3);

            switch (format)
            {
                case "G":
                    switch (Rank)
                    {
                        case Rank.Two: s.Append('2'); break;
                        case Rank.Three: s.Append('3'); break;
                        case Rank.Four: s.Append('4'); break;
                        case Rank.Five: s.Append('5'); break;
                        case Rank.Six: s.Append('6'); break;
                        case Rank.Seven: s.Append('7'); break;
                        case Rank.Eight: s.Append('8'); break;
                        case Rank.Nine: s.Append('9'); break;
                        case Rank.Ten: s.Append("10"); break;
                        case Rank.Jack: s.Append('J'); break;
                        case Rank.Queen: s.Append('Q'); break;
                        case Rank.King: s.Append('K'); break;
                        case Rank.Ace: s.Append('A'); break;
                        default:
                            return "Undefined";
                    }
                    switch (Suit)
                    {
                        case Suit.Spades: s.Append('S'); break;
                        case Suit.Hearts: s.Append('H'); break;
                        case Suit.Diamonds: s.Append('D'); break;
                        case Suit.Clubs: s.Append('C'); break;
                        default:
                            return "Undefined";
                    }
                    break;

                case "P":
                    switch (Suit)
                    {
                        case Suit.Spades: s.Append('S'); break;
                        case Suit.Hearts: s.Append('H'); break;
                        case Suit.Diamonds: s.Append('D'); break;
                        case Suit.Clubs: s.Append('C'); break;
                        default:
                            return "Undefined";
                    }
                    switch (Rank)
                    {
                        case Rank.Two: s.Append('2'); break;
                        case Rank.Three: s.Append('3'); break;
                        case Rank.Four: s.Append('4'); break;
                        case Rank.Five: s.Append('5'); break;
                        case Rank.Six: s.Append('6'); break;
                        case Rank.Seven: s.Append('7'); break;
                        case Rank.Eight: s.Append('8'); break;
                        case Rank.Nine: s.Append('9'); break;
                        case Rank.Ten: s.Append('T'); break;
                        case Rank.Jack: s.Append('J'); break;
                        case Rank.Queen: s.Append('Q'); break;
                        case Rank.King: s.Append('K'); break;
                        case Rank.Ace: s.Append('A'); break;
                        default:
                            return "Undefined";
                    }
                    break;

                default:
                    throw new FormatException("Unknown format specifier.");
            }

            return s.ToString();
        }
        #endregion

        #region Parsing
        /// <summary>
        ///   Returns a <see cref="Card"/> instance from the specified string.
        /// </summary>
        /// <param name="value">
        ///   The string representation of a <see cref="Card"/>.  It consists of a
        ///   <see cref="Rank"/> and <see cref="Suit"/> in any order, i.e. "2S" or "S2" for the
        ///   two of spades.  Also <see cref="Rank.Ten"/> is either "T" or "10".
        /// </param>
        /// <returns>
        ///   A <see cref="Card"/> instance that represents the <paramref name="value"/>.
        /// </returns>
        static public Card Parse(string value)
        {
            Rank rank = Rank.Undefined;
            Suit suit = Suit.Undefined;

            if (2 <= value.Length && value.Length <= 3)
            {
                switch (value[0])
                {
                    case 'C': suit = Suit.Clubs; break;
                    case 'D': suit = Suit.Diamonds; break;
                    case 'H': suit = Suit.Hearts; break;
                    case 'S': suit = Suit.Spades; break;
                };
                int ri = (suit != Suit.Undefined) ? 1 : 0;
                int si = (suit != Suit.Undefined) ? 0 : 1;
                switch (value[ri])
                {
                    case '2': rank = Rank.Two; break;
                    case '3': rank = Rank.Three; break;
                    case '4': rank = Rank.Four; break;
                    case '5': rank = Rank.Five; break;
                    case '6': rank = Rank.Six; break;
                    case '7': rank = Rank.Seven; break;
                    case '8': rank = Rank.Eight; break;
                    case '9': rank = Rank.Nine; break;
                    case 'T': rank = Rank.Ten; break;
                    case 'J': rank = Rank.Jack; break;
                    case 'Q': rank = Rank.Queen; break;
                    case 'K': rank = Rank.King; break;
                    case 'A': rank = Rank.Ace; break;
                    case '1':
                        if (ri + 1 < value.Length && value[ri + 1] == '0')
                        {
                            rank = Rank.Ten;
                            si++;
                        }
                        break;
                }
                if (suit == Suit.Undefined)
                {
                    switch (value[si])
                    {
                        case 'C': suit = Suit.Clubs; break;
                        case 'D': suit = Suit.Diamonds; break;
                        case 'H': suit = Suit.Hearts; break;
                        case 'S': suit = Suit.Spades; break;
                    };
                }

            }

            if (rank == Rank.Undefined || suit == Suit.Undefined)
                throw new FormatException(string.Format("Cannot parse the card '{0}'.", value));

            return Card.Get(rank, suit);
        }

        #endregion

    }

}
