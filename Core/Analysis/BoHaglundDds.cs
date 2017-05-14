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
        static Denomination[] strainOrder = new Denomination[]
        {
            Denomination.Spades, Denomination.Hearts,
            Denomination.Diamonds, Denomination.Clubs,
            Denomination.NoTrumps
        };
        static Seat[] seatOrder = new Seat[]
        {
            Seat.North, Seat.East, Seat.South, Seat.West
        };

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

            var table = new Solver.tableDealPBN(board.Hands.ToPbn(board.Dealer));
            var results = new Solver.tableResults
            {
                solution = new int[20]
            };
            var status = Solver.CalcDDtablePBN(table, ref results);
            if (status < 0)
                throw new Exception(string.Format("Bo Haglund's DDS returned error code of {0}.", status));

            var dds = new DoubleDummySolution();
            var next = 0;
            foreach (var strain in strainOrder)
            {
                foreach (var declaror in seatOrder)
                {
                    var tricks = results.solution[next++];
                    if (tricks > 6)
                    {
                        var contract = new Contract(tricks - 6, strain, Risk.None, declaror);
                        dds.Add(contract);

                        if (log.IsDebugEnabled)
                            log.Debug(contract.ToString());
                    }
                }
            }

            if (log.IsDebugEnabled)
                log.Debug("Finished MakeableContracts");

            return dds;
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
               [In] [Out] ref futureTricks futureTricks, // struct futureTricks *futp
               int threadIndex // 0-15
             );

            [StructLayout(LayoutKind.Sequential)]
            public struct tableDealPBN
            {
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
                public char[] cards;

                public tableDealPBN(string pbnCards)
                {
                    cards = new char[80];
                    for (int i = 0; i < pbnCards.Length; i++)
                        cards[i] = pbnCards[i];
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct tableResults
            {
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)] 
                public int[] solution;
            }

            [DllImport("dds.dll")]
            public static extern int CalcDDtablePBN(
                [In] tableDealPBN deal,
                [In] [Out] ref tableResults results
            );
        }
    }
}
