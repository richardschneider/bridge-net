using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Makaretu.Bridge.Scoring
{
    /// <summary>
    ///   Determines the score for a <see cref="Game"/> based upon the
    ///   <see cref="Contract"/> and made tricks.
    /// </summary>
    public class ContractScorer
    {
        static int[] DoubledNotVulnerable = new int[]
        {
         0,      -100,   -300,   -500,
         -800,  -1100,  -1400,  -1700,
         -2000, -2300,  -2600,  -2900,
         -3200, -3500
        };

        static int[] DoubledVulnerable = new int[]
        {
             0,  -200,  -500,  -800,
         -1100, -1400, -1700, -2000,
         -2300, -2600, -2900, -3200,
         -3500, -3800
        };

        /// <summary>
        ///   Determines the score for the played <see cref="Game"/>.
        /// </summary>
        /// <param name="game">
        ///   The <see cref="Game"/> to score.
        /// </param>
        /// <returns>
        ///   The score of the <see cref="Contract.Declaror"/>.
        /// </returns>
        public int Calculate(Game game)
        {
            return Calculate(game.Contract, game.Board.IsVulnerable(game.Contract.Declaror), game.Made);
        }

        /// <summary>
        ///   Determines the score based upon the <see cref="Contract"/> and made tricks.
        /// </summary>
        /// <param name="contract">
        ///   The <see cref="Contract"/>
        /// </param>
        /// <param name="vulnerable"/>
        ///   <b>true</b> if the <see cref="Contract.Delclaror"/> is vulnernable; otherwise,
        ///   <b>false</b>.
        /// </param>
        /// <param name="made">
        ///   The number of tricks made over the book contract (6) or a negative number
        ///   indicating the number of trick down on the <see cref="Contract"/>.
        /// </param>
        /// <returns>
        ///   The score of the <see cref="Contract.Delclaror"/>.
        /// </returns>
        public int Calculate(Contract contract, bool vulnerable, int made)
        {
            int overTricks = made - contract.Level;

            // Undertricks?
            if (made < 0)
            {
                if (contract.Risk == Risk.None)
                    return made * (vulnerable ? 100 : 50);
                int penalty = vulnerable ? DoubledVulnerable[-made] : DoubledNotVulnerable[-made];
                if (contract.Risk == Risk.Redoubled)
                    penalty *= 2;
                return penalty;
            }

            int score = 0;

            // Contract Points
            switch (contract.Denomination)
            {
                case Denomination.Spades:
                case Denomination.Hearts:
                    score = contract.Level * 30;
                    break;
                case Denomination.NoTrumps:
                    score = contract.Level * 30 + 10;
                    break;
                case Denomination.Clubs:
                case Denomination.Diamonds:
                    score = contract.Level * 20;
                    break;
            }
            if (contract.Risk == Risk.Doubled)
                score *= 2;
            else if (contract.Risk == Risk.Redoubled)
                score *= 4;

            // Level Bonus
            if (score < 100) // Part score?
                score += 50;
            else
            {
                score += vulnerable ? 500 : 300; // game bonus
                if (contract.Level == 7) // Grand slam?
                    score += vulnerable ? 1500 : 1000;
                else if (contract.Level == 6) // small slam?
                    score += vulnerable ? 750 : 500;
            }
            // Insult bonus
            if (contract.Risk == Risk.Doubled)
                score += 50;
            else if (contract.Risk == Risk.Redoubled)
                score += 100;

            // Overtrick bonus
            if (overTricks > 0)
            {
                if (contract.Risk == Risk.Doubled)
                    score += overTricks * (vulnerable ? 200 : 100);
                if (contract.Risk == Risk.Redoubled)
                    score += overTricks * (vulnerable ? 400 : 200);
                else
                {
                    switch (contract.Denomination)
                    {
                        case Denomination.Spades:
                        case Denomination.Hearts:
                        case Denomination.NoTrumps:
                            score += overTricks * 30;
                            break;
                        case Denomination.Clubs:
                        case Denomination.Diamonds:
                            score += overTricks * 20;
                            break;
                    }
                }
            }

            return score;
        }


    }
}
