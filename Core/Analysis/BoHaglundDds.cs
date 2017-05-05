using Common.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;

namespace Makaretu.Bridge.Analysis
{
    /// <summary>
    ///   A C# wrapper for Bo Haglund's double dummy solver.
    /// </summary>
    public class BoHaglundDds : IDoubleDummy
    {
        static ILog log = LogManager.GetLogger(typeof(BoHaglundDds));

        /// <summary>
        ///   Determines all the contracts that can be made for the specified <see cref="Board"/>.
        /// </summary>
        /// <param name="board">
        ///   The <see cref="Board"/> to analyse.
        /// </param>
        /// <returns>
        ///   A <see cref="DoubleDummySolution"/> that contains all the contracts that can 
        ///   be made.
        /// </returns>
        public DoubleDummySolution MakeableContracts(Board board)
        {
            if (log.IsDebugEnabled)
                log.Debug("Starting MakeableContracts");

            var dds = new DoubleDummySolution();
            var futureTricks = new Solver.futureTricks();
            futureTricks.rank = new int[13];
            futureTricks.score = new int[13];
            futureTricks.suit = new int[13];
            futureTricks.equals = new int[13];

            const int target = -1;
            const int solutions = 1;
            var deal = new Solver.deal();
            deal.currentTrickRank = new int[3];
            deal.currentTrickSuit = new int[3];
            deal.remainCards = new uint[16];
            Convert(board, deal);

            foreach (var trump in new Denomination[] { Denomination.Clubs, Denomination.Diamonds, Denomination.Hearts, Denomination.Spades, Denomination.NoTrumps })
            {
                switch (trump)
                {
                    case Denomination.Spades: deal.trump = 0; break;
                    case Denomination.Hearts: deal.trump = 1; break;
                    case Denomination.Diamonds: deal.trump = 2; break;
                    case Denomination.Clubs: deal.trump = 3; break;
                    case Denomination.NoTrumps: deal.trump = 4; break;
                }
                int mode = 1;
                foreach (var declaror in new Seat[] { Seat.North, Seat.South, Seat.East, Seat.West })
                {
                    switch (Board.NextSeat(declaror))
                    {
                        case Seat.North: deal.first = 0; break;
                        case Seat.East: deal.first = 1; break;
                        case Seat.South: deal.first = 2; break;
                        case Seat.West: deal.first = 3; break;
                    }

                    if (log.IsTraceEnabled)
                        log.Trace(string.Format("Solving {0} for {1}", trump, declaror));

                    int status = Solver.SolveBoard(deal, target, solutions, mode, ref futureTricks);
                    if (status < 0)
                        throw new Exception(string.Format("Bo Haglund's DDS returned error code of {0}.", status));
                    int minTricks = 13;
                    for (int i = 0; i < futureTricks.cards; ++i)
                    {
                        int tricks = futureTricks.score[i];
                        if (tricks < minTricks)
                            minTricks = tricks;
                    }
                    if (minTricks > 6)
                    {
                        var contract = new Contract(minTricks - 6, trump, Risk.None, declaror);
                        dds.Add(contract);

                        if (log.IsDebugEnabled)
                            log.Debug(contract.ToString());
                    }
                    mode = 2;
                }
            }

            if (log.IsDebugEnabled)
                log.Debug("Finished MakeableContracts");
            return dds;
        }

        void Convert(Board board, Solver.deal deal)
        {
            for (int i = 0; i < 16; ++i)
                deal.remainCards[i] = 0;

            foreach (var seat in new Seat[] { Seat.North, Seat.South, Seat.East, Seat.West })
            {
                int seatBase = 0;
                switch (Board.NextSeat(seat))
                {
                    case Seat.North: seatBase = 0; break;
                    case Seat.East: seatBase = 4; break;
                    case Seat.South: seatBase = 2 * 4; break;
                    case Seat.West: seatBase = 3 * 4; break;
                }
                foreach (var card in board.Hands[seat].Cards)
                {
                    int suitOffset = 0;
                    switch (card.Suit)
                    {
                        case Suit.Spades: suitOffset = 0; break;
                        case Suit.Hearts: suitOffset = 1; break;
                        case Suit.Diamonds: suitOffset = 2; break;
                        case Suit.Clubs: suitOffset = 3; break;
                    }
                    deal.remainCards[seatBase + suitOffset] |= (uint)(1 << (int)card.Rank);
                }
            }
        }

        class Solver
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct deal
            {
                public int trump; // Spades=0, Hearts=1, Diamonds=2, Clubs=3,  NT=4 
                public int first; // 0=North, 1=East, 2=South, 3=West , Leading hand for the trick.
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
                public int[] currentTrickSuit;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
                public int[] currentTrickRank;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
                public uint[] remainCards; // 4x4. 1st index hand (0-3), 2nd index suit (0-3), values as bitstring of ranks  bit 0=0, bit 1=0, bit 2=rank 2, ………. bit 14=rank 14
            };

            [StructLayout(LayoutKind.Sequential)]
            public struct futureTricks
            {
                public int nodes;
                public int cards;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
                public int[] suit;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
                public int[] rank;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
                public int[] equals;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
                public int[] score;
            }

            [DllImport("dds.dll")]
            public static extern int SolveBoard(
               [In] deal deal, // struct deal dl, 
               int target,
               int solutions,
               int mode,
               [In] [Out] ref futureTricks futureTricks // struct futureTricks *futp
             );
        }
    }
}
