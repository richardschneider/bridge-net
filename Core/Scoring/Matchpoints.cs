using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Makaretu.Bridge.Scoring
{
    public class Matchpoints
    {
        float win;
        float tie;

        public Matchpoints()
        {
            win = 2;
            tie = 1;
        }

        public void Calculate(List<Game> games)
        {
            var played = new List<Game>(games.Count);
            foreach (var game in games)
            {
                game.MatchpointsEastWest = 0;
                game.MatchpointsNorthSouth = 0;
                // TODO: If played
                played.Add(game);
            }
            int totalGames = played.Count;
            float totalMatchpoints = totalGames * win - win;
            played.Sort((a, b) => b.PartnershipScore(Partnership.NorthSouth) - a.PartnershipScore(Partnership.NorthSouth));
            int i = 0;
            int n = played.Count - 1;
            while (i <= n)
            {
                var game = played[i];
                float matchpoints = 0;
                int score = game.PartnershipScore(Partnership.NorthSouth);
                int ties = 0;
                for (int j = i + 1; j <= n && score == played[j].PartnershipScore(Partnership.NorthSouth); ++j)
                {
                    ++ties;
                    matchpoints += tie;
                }
                matchpoints += win * (n - i - ties);
                game.MatchpointsNorthSouth = matchpoints;
                game.MatchpointsEastWest = totalMatchpoints - matchpoints;
                for (int j = 0; j < ties; ++j)
                {
                    ++i;
                    played[i].MatchpointsNorthSouth = matchpoints;
                    played[i].MatchpointsEastWest = totalMatchpoints - matchpoints;
                }
                ++i;
            }
        }
    }
}
