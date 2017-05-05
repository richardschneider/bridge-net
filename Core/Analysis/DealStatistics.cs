using BlackHen.Bridge.Facts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackHen.Bridge.Analysis
{
    /// <summary>
    ///   Provides some common statistics on a list of <see cref="Board">boards</see>.
    /// </summary>
    public class DealStatistics
    {
        public class Info
        {
            public float North;
            public float South;
            public float East;
            public float West;

            public override string ToString()
            {
                return string.Format("N: {0:0.##} S: {1:0.##} E: {2:0.##} N: {3:0.##}", North, South, East, West);
            }
        }

        static public Info AverageHcp(List<Board> boards)
        {
            var info = new Info();

            if (boards.Count == 0)
                return info;

            foreach (var board in boards)
            {
                info.North += HcpEvaluator.GetHcp(board.Hands[Seat.North]);
                info.South += HcpEvaluator.GetHcp(board.Hands[Seat.South]);
                info.East += HcpEvaluator.GetHcp(board.Hands[Seat.East]);
                info.West += HcpEvaluator.GetHcp(board.Hands[Seat.West]);
            }

            info.North /= boards.Count;
            info.South /= boards.Count;
            info.East /= boards.Count;
            info.West /= boards.Count;

            return info;
        }

        static public Info Balanced(List<Board> boards)
        {
            var info = new Info();

            foreach (var board in boards)
            {
                info.North += BalancedEvaluator.IsBalanced(board.Hands[Seat.North]) ? 1 : 0;
                info.South += BalancedEvaluator.IsBalanced(board.Hands[Seat.South]) ? 1 : 0;
                info.East += BalancedEvaluator.IsBalanced(board.Hands[Seat.East]) ? 1 : 0;
                info.West += BalancedEvaluator.IsBalanced(board.Hands[Seat.West]) ? 1 : 0;
            }

            return info;
        }

        static public Info Voids(List<Board> boards)
        {
            var info = new Info();

            foreach (var board in boards)
            {
                for (int i = 1; i <= 4; ++i)
                {
                    var length = SuitLength(board.Hands, (Suit)i);
                    info.North += length.North == 0 ? 1 : 0;
                    info.South += length.South == 0 ? 1 : 0;
                    info.East += length.East == 0 ? 1 : 0;
                    info.West += length.West == 0 ? 1 : 0;
                }
            }

            return info;
        }

        static public Info Singletons(List<Board> boards)
        {
            var info = new Info();

            foreach (var board in boards)
            {
                for (int i = 1; i <= 4; ++i)
                {
                    var length = SuitLength(board.Hands, (Suit)i);
                    info.North += length.North == 1 ? 1 : 0;
                    info.South += length.South == 1 ? 1 : 0;
                    info.East += length.East == 1 ? 1 : 0;
                    info.West += length.West == 1 ? 1 : 0;
                }
            }

            return info;
        }

        static public Info SevenPlusSuits(List<Board> boards)
        {
            var info = new Info();

            foreach (var board in boards)
            {
                for (int i = 1; i <= 4; ++i)
                {
                    var length = SuitLength(board.Hands, (Suit)i);
                    info.North += length.North >= 7 ? 1 : 0;
                    info.South += length.South >= 7 ? 1 : 0;
                    info.East += length.East >= 7 ? 1 : 0;
                    info.West += length.West >= 7 ? 1 : 0;
                }
            }

            return info;
        }

        static Info SuitLength(HandCollection hand, Suit suit)
        {
            var info = new Info();
            info.North = hand[Seat.North].SuitLength(suit);
            info.South = hand[Seat.South].SuitLength(suit);
            info.East = hand[Seat.East].SuitLength(suit);
            info.West = hand[Seat.West].SuitLength(suit);

            return info;
        }
    }
}
