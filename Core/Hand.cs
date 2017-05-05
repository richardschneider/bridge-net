using System;
using System.Collections.Generic;
using System.Text;

namespace BlackHen.Bridge
{
    /// <summary>
    ///   The cards held by one player.
    /// </summary>
    /// <remarks>
    ///   A <b>Hand</b> can be used to describe the actual cards held by a player, or facts assertained from
    ///   the auction or play or cards.
    /// </remarks>
    public class Hand
    {
        List<Fact> facts;

        public Hand()
        {
            Cards = new CardCollection();
        }

        public CardCollection Cards { get; private set; }

        public void Add(Card card)
        {
            Cards.Add(card);
        }

        public List<Fact> Facts
        {
            get
            {
                if (facts == null)
                    facts = new List<Fact>();
                return facts;
            }
        }

        /// <summary>
        ///   Returns the number of cards held by the <see cref="Hand"/> in the specified <see cref="Suit"/>.
        /// </summary>
        /// <param name="suit">
        ///   One of the <see cref="Suit"/> enumeration values.
        /// </param>
        /// <returns>
        ///   The number of cards with the specified <paramref name="suit"/>.
        /// </returns>
        public byte SuitLength(Suit suit)
        {
            byte length = 0;
            foreach (var card in Cards)
            {
                if (card.Suit == suit)
                    ++length;
            }
            return length;
        }

        #region ToString
        public override string ToString()
        {
            return ToString("G");
        }

        public string ToString(string format)
        {
            if (format == null)
                format = "G";

            string suitSeperator;
            bool showSuit;
            switch (format)
            {
                case "G":
                    suitSeperator = "; ";
                    showSuit = true;
                    break;
                case "N":
                    suitSeperator = Environment.NewLine;
                    showSuit = true;
                    break;
                case "P":
                    suitSeperator = ".";
                    showSuit = false;
                    break;

                default:
                    throw new FormatException("Unknown format specifier.");
            };

            StringBuilder s = new StringBuilder();
            var lastSuit = Card.SuitsDescending[Card.SuitCount - 1];
            var cards = new CardCollection();
            cards.AddRange(this.Cards);
            cards.Sort((a, b) => (int)b.Rank - (int)a.Rank);
            foreach (var suit in Card.SuitsDescending)
            {
                if (showSuit)
                {
                    s.Append(Card.SuitGlyph[(int)suit]);
                    s.Append(' ');
                }

                foreach (Card card in cards)
                {
                    if (card.Suit == suit)
                    {
                        s.Append(card.ToString("P")[1]);
                    }
                }

                if (suit != lastSuit)
                {
                    if (s[s.Length - 1] == ' ')
                        s.Length -= 1;
                    s.Append(suitSeperator);
                }
            }

            return s.ToString().Trim();
        }
        #endregion

        public static Hand ParsePbn(string cards)
        {
            if (cards.Split('.').Length != 4)
                throw new FormatException("All four suits must be declared.");

            var hand = new Hand();
            var suits = "SHDC";
            var suitIndex = 0;
            foreach (char rank in cards)
            {
                if (rank == '.')
                {
                    ++suitIndex;
                    continue;
                }
                hand.Add(Card.Get(rank, suits[suitIndex]));
            }

            return hand;
        }
    }
}
