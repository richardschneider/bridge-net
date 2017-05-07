// TODO: Rename HandRecord
// TODO: Make ranks a little taller

using Makaretu.Bridge.Analysis;
using Makaretu.Bridge.Facts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Makaretu.Bridge.Reports
{
    /// <summary>
    ///   Produces an HTML page containing the boards of a <see cref="Tournament"/>.
    /// </summary>
    public class HandoutHtml
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="HandoutHtml"/> class.
        /// </summary>
        public HandoutHtml()
        {
            Columns = 3;
        }

        /// <summary>
        ///   The <see cref="Tournament"/> used for reporting.
        /// </summary>
        public Tournament Tournament { get; set; }

        /// <summary>
        ///   The number of columns in the report.
        /// </summary>
        /// <value>
        ///   The report is designed to be printed for 1 to 3 columns.  The
        ///   default value is 3.
        /// </value>
        /// <remarks>
        ///   Each column is used to describe a <see cref="Board"/>.
        /// </remarks>
        public int Columns { get; set; }

        public void Produce(TextWriter report)
        {
            report.WriteLine(startHtml);
            report.WriteLine("<table class='handout'>");

            int column = 0;
            foreach (var board in Tournament.Boards)
            {
                if (column == 0)
                    report.WriteLine("<tr>");

                report.WriteLine("<td>");
                Produce(report, board);
                report.WriteLine("</td>");

                if (++column == Columns)
                {
                    report.WriteLine("</tr>");
                    column = 0;
                }
            }

            if (column == 0)
                report.WriteLine("<tr>");
            report.WriteLine("<td>");
            ProduceStatistics(report);
            report.WriteLine("</td>");

            report.WriteLine("</tr>");
            report.WriteLine("</table>");
            report.WriteLine(endHtml);
        }

        void Produce(TextWriter report, Board board)
        {
            report.WriteLine(boardStartHmtl, board.Number);

            report.WriteLine("<tr><td rowspan='4' class='boardnumber'>{0}</td>", board.Number == 0 ? "" : board.Number.ToString());
            Produce(report, board, Seat.North, Suit.Spades);
            if (board.Number != 0)
            {
                report.WriteLine("<td rowspan='4' class='state'>{0} dealer<br/>{1} vul.</td>", board.Dealer, board.Vulnerability);
            }
            else
            {
                report.WriteLine("<td rowspan='4' class='state'>&nbsp;</td>");
            }
            report.WriteLine("</tr>");

            report.WriteLine("<tr>");
            Produce(report, board, Seat.North, Suit.Hearts);
            report.WriteLine("</tr>");
            report.WriteLine("<tr>");
            Produce(report, board, Seat.North, Suit.Diamonds);
            report.WriteLine("</tr>");
            report.WriteLine("<tr>");
            Produce(report, board, Seat.North, Suit.Clubs);
            report.WriteLine("</tr>");

            report.WriteLine("<tr>");
            Produce(report, board, Seat.West, Suit.Spades);
            Produce(report, board, Seat.East, Suit.Spades);
            report.WriteLine("</tr>");
            report.WriteLine("<tr>");
            Produce(report, board, Seat.West, Suit.Hearts);
            Produce(report, board, Seat.East, Suit.Hearts);
            report.WriteLine("</tr>");
            report.WriteLine("<tr>");
            Produce(report, board, Seat.West, Suit.Diamonds);
            Produce(report, board, Seat.East, Suit.Diamonds);
            report.WriteLine("</tr>");
            report.WriteLine("<tr>");
            Produce(report, board, Seat.West, Suit.Clubs);
            Produce(report, board, Seat.East, Suit.Clubs);
            report.WriteLine("</tr>");

            report.WriteLine("<tr>");
            ProduceHcp(report, board);
            Produce(report, board, Seat.South, Suit.Spades);
            ProduceContracts(report, board);
            report.WriteLine("</tr>");

            report.WriteLine("<tr>");
            Produce(report, board, Seat.South, Suit.Hearts);
            report.WriteLine("</tr>");
            report.WriteLine("<tr>");
            Produce(report, board, Seat.South, Suit.Diamonds);
            report.WriteLine("</tr>");
            report.WriteLine("<tr>");
            Produce(report, board, Seat.South, Suit.Clubs);
            report.WriteLine("</tr>");

            report.WriteLine(boardEndHtml);
        }

        private void ProduceHcp(TextWriter report, Board board)
        {
            report.WriteLine("<td rowspan='4' style='height: 100%'><table class='hcp'>");
            report.WriteLine(hcpHtml,
               HcpEvaluator.GetHcp(board.Hands[Seat.North]),
               HcpEvaluator.GetHcp(board.Hands[Seat.West]),
               HcpEvaluator.GetHcp(board.Hands[Seat.East]),
               HcpEvaluator.GetHcp(board.Hands[Seat.South]));
            report.WriteLine("</table></td>");
        }

        void Produce(TextWriter report, Board board, Seat seat, Suit suit)
        {
            report.WriteLine("<td class='suit' colspan='2'>");
            switch (suit)
            {
                case Suit.Clubs: report.WriteLine("<span class='bsuit'>&clubs;</span>&nbsp;"); break;
                case Suit.Spades: report.WriteLine("<span class='bsuit'>&spades;</span>&nbsp;"); break;
                case Suit.Diamonds: report.WriteLine("<span class='rsuit'>&diams;</span>&nbsp;"); break;
                case Suit.Hearts: report.WriteLine("<span class='rsuit'>&hearts;</span>&nbsp;"); break;
            }
            var honours = new StringBuilder();
            var rags = new StringBuilder();
            var cards = new CardCollection();
            cards.AddRange(board.Hands[seat].Cards);
            cards.Sort((a, b) => (int)b.Rank - (int)a.Rank);
            foreach (Card card in cards)
            {
                if (card.Suit == suit)
                {
                    if (card.IsBiddingHonour)
                        honours.Append(card.ToString("P")[1]);
                    else
                        rags.Append(card.ToString("P")[1]);
                }
            }
            if (honours.Length > 0)
                report.Write(honours.ToString());
            if (rags.Length > 0)
                report.Write("<span class='rags'>{0}</span>", rags.ToString());

            report.WriteLine("</td>");
        }

        public void ProduceStatistics(TextWriter report)
        {
            report.WriteLine("<table class='stats'>");
            report.WriteLine("<thead><tr><th></th><th>N</th><th>S</th><th>E</th><th>W</th></tr></thead>");
            report.WriteLine("<tbody>");

            var hcpAvg = DealStatistics.AverageHcp(Tournament.Boards);
            report.WriteLine("<tr><th>HCP avg.</th><td>{0:0.00}</td><td>{1:0.00}</td><td>{2:0.00}</td><td>{3:0.00}</td></tr>",
               hcpAvg.North, hcpAvg.South, hcpAvg.East, hcpAvg.West);

            var balanced = DealStatistics.Balanced(Tournament.Boards);
            report.WriteLine("<tr><th>Balanced</th><td>{0:0}</td><td>{1:0}</td><td>{2:0}</td><td>{3:0}</td></tr>",
               balanced.North, balanced.South, balanced.East, balanced.West);

            var voids = DealStatistics.Voids(Tournament.Boards);
            report.WriteLine("<tr><th>Voids</th><td>{0:0}</td><td>{1:0}</td><td>{2:0}</td><td>{3:0}</td></tr>",
               voids.North, voids.South, voids.East, voids.West);

            var singletons = DealStatistics.Singletons(Tournament.Boards);
            report.WriteLine("<tr><th>Singletons</th><td>{0:0}</td><td>{1:0}</td><td>{2:0}</td><td>{3:0}</td></tr>",
               singletons.North, singletons.South, singletons.East, singletons.West);

            var sevenPlus = DealStatistics.SevenPlusSuits(Tournament.Boards);
            report.WriteLine("<tr><th>7+ suits</th><td>{0:0}</td><td>{1:0}</td><td>{2:0}</td><td>{3:0}</td></tr>",
               sevenPlus.North, sevenPlus.South, sevenPlus.East, sevenPlus.West);

            report.WriteLine("</tbody></table>");
        }


        public void ProduceContracts(TextWriter report, Board board)
        {
            var dds = new BoHaglundDds().MakeableContracts(board);

            report.WriteLine("<td rowspan='4' style='height: 100%'><table class='contracts'>");
            report.WriteLine(@"<thead><tr><th></th>
<th><span class='bsuit'>&clubs;</span></th>
<th><span class='rsuit'>&diams;</span></th>
<th><span class='rsuit'>&hearts;</span></th>
<th><span class='bsuit'>&spades;</span></th>
<th>NT</th>
</tr></thead>");
            report.WriteLine("<tbody>");

            report.Write("<tr><td>N</td><td>{0:#}</td><td>{1:#}</td><td>{2:#}</td><td>{3:#}</td><td>{4:#}</td></tr>",
               dds[Seat.North, Denomination.Clubs].Level, dds[Seat.North, Denomination.Diamonds].Level,
               dds[Seat.North, Denomination.Hearts].Level, dds[Seat.North, Denomination.Spades].Level,
               dds[Seat.North, Denomination.NoTrumps].Level);
            report.Write("<tr><td>S</td><td>{0:#}</td><td>{1:#}</td><td>{2:#}</td><td>{3:#}</td><td>{4:#}</td></tr>",
               dds[Seat.South, Denomination.Clubs].Level, dds[Seat.South, Denomination.Diamonds].Level,
               dds[Seat.South, Denomination.Hearts].Level, dds[Seat.South, Denomination.Spades].Level,
               dds[Seat.South, Denomination.NoTrumps].Level);
            report.Write("<tr><td>E</td><td>{0:#}</td><td>{1:#}</td><td>{2:#}</td><td>{3:#}</td><td>{4:#}</td></tr>",
               dds[Seat.East, Denomination.Clubs].Level, dds[Seat.East, Denomination.Diamonds].Level,
               dds[Seat.East, Denomination.Hearts].Level, dds[Seat.East, Denomination.Spades].Level,
               dds[Seat.East, Denomination.NoTrumps].Level);
            report.Write("<tr><td>W</td><td>{0:#}</td><td>{1:#}</td><td>{2:#}</td><td>{3:#}</td><td>{4:#}</td></tr>",
               dds[Seat.West, Denomination.Clubs].Level, dds[Seat.West, Denomination.Diamonds].Level,
               dds[Seat.West, Denomination.Hearts].Level, dds[Seat.West, Denomination.Spades].Level,
               dds[Seat.West, Denomination.NoTrumps].Level);
            report.WriteLine("</tbody></table></td>");
        }

        #region HTML strings
        const string startHtml = @"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html>
<head>
   <title>Event</title>
   <style type='text/css'>
@media print
{
table.board {page-break-before: auto}
}
      table.handout
      {
         padding: 0px;
         margin: 0px;
         border: 0px;
         border: 1px solid black;
         border-spacing: 0px;
         border-collapse: collapse;
      }
      table.handout td
      {
         padding: 0px;
         margin: 0px;
         border: 0px;
         border-spacing: 0px;
         border-collapse: collapse;
      }
      table.board, table.stats  
      {
         width: 6.5cm;
         border: 1px solid black;
         padding: 2px;
         margin: 0px;
         border-spacing: 0px;
         border-collapse: collapse;
         font: 9pt Arial Black;
      }
      table.stats { height: 100%; vertical-align: top; }
      table.stats td { font: 9pt Arial; }
      table.stats tbody th { text-align: left; font: normal 8pt verdana; }

      td.suit 
      { 
         font: 9pt Arial Black;
         letter-spacing: 0px;
         padding: 0px;
         margin: 0px;
         padding-left: 2px;
         line-height: 11pt;
      }
      span.rags 
      { 
         font: 9pt Arial;
         letter-spacing: 0px;
      }
      rags 
      { 
         font: 9pt Arial;
         letter-spacing: 0px;
      }
      span.rsuit 
      {
         font-size: 11pt;
         color: Red;
         line-height: 11pt;
      }
      span.bsuit
      {
         font-size: 11pt;
         color: Black;
         line-height: 11pt;
      }
      td.boardnumber 
      { 
         padding: 0px;
         margin: 0px;
         font-size: 20pt; 
         text-align: center;
         vertical-align: middle;
      }
      table.hcp  
      {
         width: 100%;
         height: 100%;
         padding: 0px;
         margin: 0px;
         border-spacing: 0px;
         border-collapse: collapse;
         font: normal 8pt Arial;
         text-align: center;
         vertical-align: middle;
      }
      table.contracts  
      {
         width: 100%;
         height: 100%;
         padding: 0px;
         margin: 0px;
         border-spacing: 0px;
         border-collapse: collapse;
         font: normal 7pt verdana;
         text-align: center;
         line-height: 8pt;
      }
      td.state
      {
         padding-top: 3px;
         vertical-align: middle;
         text-align: center;
         font: normal 8pt verdana;
      }
   </style>
</head>
<body>";
        const string endHtml = "</body></html>";

        const string boardStartHmtl = @"
<table class='board' id='board{0}'>
<col width='25%' />
<col width='33%' />
<col width='17%' />
<col width='25%' />
<tbody>";

        const string boardEndHtml = "</tbody></table>";

        const string hcpHtml = @"<tr><td></td><td>{0}</td><td></td></tr>
  <tr><td style='text-align: right'>{1}</td><td></td><td style='text-align: left'>{2}</td></tr>
  <tr><td></td><td>{3}</td><td></td></tr>";

        #endregion
    }
}
