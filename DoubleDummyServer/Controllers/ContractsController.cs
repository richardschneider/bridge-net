using System;
using System.Collections.Generic;
using System.Linq;
using Makaretu.Bridge;
using Makaretu.Bridge.Analysis;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DoubleDummyServer.Controllers
{
    public class ContractsController : ApiController
    {
        IDoubleDummy Solver { get { return new BoHaglundDds(); } }

        // GET api/contracts?pbn=W:KJ95.T.AT873.T98 76.AQJ9642.KJ.QJ QT8.K87.962.A654 A432.53.Q54.K732
        public DoubleDummySolution Get(string pbn)
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

            return Solver
                .MakeableContracts(board);
        }

    }
}
