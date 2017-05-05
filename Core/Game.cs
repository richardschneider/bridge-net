using BlackHen.Bridge.Scoring;
using System;

namespace BlackHen.Bridge
{
    public class Game
    {
        int score;

        /// <summary>
        ///   Create a new instance of the <see cref="Game"/> class for the
        ///   specified <see cref="Board"/>.
        /// </summary>
        /// <param name="board">
        ///   The <see cref="BlackHen.Bridge.Board"/> used for the duplicate game.
        /// </param>
        public Game(Board board)
        {
            Board = board;
            Bids = new Auction(board.Dealer);
        }

        public Board Board { get; private set; }

        public Auction Bids { get; private set; }

        public Pair NorthSouth { get; private set; }
        public Pair EastWest { get; private set; }

        public Contract Contract { get; set; }

        /// <summary>
        ///   The number of tricks made over the book contract (6) or a negative number
        ///   indicating the number of trick down on the <see cref="Contract"/>.
        /// </summary>
        public int Made { get; set; }

        public float MatchpointsNorthSouth { get; set; }
        public float MatchpointsEastWest { get; set; }

        /// <summary>
        ///   Gets the declarors score based on the <see cref="Contract"/> and <see cref="Made"/> tricks.
        /// </summary>
        /// <remarks>
        ///   If the <b>Score</b> has not been set, it will automatically be calculated.
        /// </remarks>
        /// <see cref="ContractScorer"/>
        public int Score
        {
            get
            {
                if (score == 0 && Made != 0)
                    score = new ContractScorer().Calculate(this);
                return score;
            }
            set
            {
                score = value;
            }
        }

        /// <summary>
        ///   Gets the score for the <see cref="Pair"/>.
        /// </summary>
        /// <param name="pair"></param>
        /// <returns>
        ///   The score for the <paramref name="pair"/>
        /// </returns>
        public int PairScore(Pair pair)
        {
            return PartnershipScore(pair.Partnership);
        }

        /// <summary>
        ///   Gets the score for the <see cref="Partnership"/>.
        /// </summary>
        /// <param name="partnership"></param>
        /// <returns>
        ///   The score for the <paramref name="partnership"/>
        /// </returns>
        public int PartnershipScore(Partnership partnership)
        {
            bool isDeclaror;
            if (partnership == Partnership.NorthSouth)
                isDeclaror = Contract.Declaror == Seat.North || Contract.Declaror == Seat.South;
            else
                isDeclaror = Contract.Declaror == Seat.East || Contract.Declaror == Seat.West;

            return isDeclaror ? Score : -Score;
        }
    }
}
