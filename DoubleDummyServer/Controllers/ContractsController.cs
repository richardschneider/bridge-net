using System;
using System.Collections.Generic;
using System.Linq;
using Makaretu.Bridge;
using Makaretu.Bridge.Analysis;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using System.IO;
using Makaretu.Bridge.Reports;
using System.Diagnostics;
using System.Management;
using GoogleAnalyticsTracker.WebAPI2;
using System.Web.Http.Cors;

namespace DoubleDummyServer.Controllers
{
    [ActionTracking("UA-99022219-1", "dds-3.apphb.com")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ContractsController : ApiController
    {
        /// <summary>
        ///   CPU clock speed in MHz
        /// </summary>
        /// <returns></returns>
        static long CPUSpeed()
        {
            using (var cpu = new ManagementObject("Win32_Processor.DeviceID='CPU0'"))
            {
                return (uint)(cpu["CurrentClockSpeed"]);
            }
        }

        IDoubleDummy Solver { get { return new BoHaglundDds(); } }

        // GET api/contracts?pbn=W:KJ95.T.AT873.T98 76.AQJ9642.KJ.QJ QT8.K87.962.A654 A432.53.Q54.K732
        // GET api/contracts?html=true&pbn=W:KJ95.T.AT873.T98 76.AQJ9642.KJ.QJ QT8.K87.962.A654 A432.53.Q54.K732
        public object Get(string pbn, bool? html = false, bool? stats = false)
        {
            var board = new Board
            {
                Hands = HandCollection.ParsePbn(pbn)
            };

            if (board.Hands[Seat.North].Cards.Count != 13)
                throw new FormatException("North needs 13 cards.");
            if (board.Hands[Seat.South].Cards.Count != 13)
                throw new FormatException("South needs 13 cards.");
            if (board.Hands[Seat.East].Cards.Count != 13)
                throw new FormatException("East needs 13 cards.");
            if (board.Hands[Seat.West].Cards.Count != 13)
                throw new FormatException("West needs 13 cards.");

            // Return HTML report?
            if (html.HasValue && html.Value)
            {
                var tournament = new Tournament();
                tournament.Boards.Add(board);
                var writer = new StringWriter();
                var report = new HandRecord() { Tournament = tournament };
                report.Produce(writer);

                var response = new HttpResponseMessage();
                response.Content = new StringContent(writer.ToString());
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                return response;
            }

            // Stats and the data?
            if (stats.HasValue && stats.Value)
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                var contracts = Solver.MakeableContracts(board);
                stopWatch.Stop();

                return new
                {
                    Stats = new
                    {
                        ProcessorCount = Environment.ProcessorCount,
                        ProcessorSpeedMhz = CPUSpeed(),
                        RunTime = stopWatch.Elapsed
                    },
                    Contracts = contracts
                };
            }

            // Just return the data.
            return Solver.MakeableContracts(board);
        }

    }
}
