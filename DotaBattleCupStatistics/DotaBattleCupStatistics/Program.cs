using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotaBattleCupStatistics
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var matches = await Get_100_000_PublicMatchesBeforeMatchIdUntilError(6215834180);

            var localStorage = new LocalStorage();

            localStorage.SetMatches(matches);
            localStorage.SetTotalMatches(matches.Count);
            localStorage.SetBattleCupMatches(matches.Count(m => m.LobbyType == OpenDotaLobbyType.BattleCup));

            await localStorage.Save();
        }

        private static async Task<IReadOnlyCollection<OpenDotaMatch>> Get_100_000_PublicMatchesBeforeMatchIdUntilError(long matchId)
        {
            var result = new List<OpenDotaMatch>();

            try
            {
                var currentMatchId = matchId;
                for (var i = 0; i < 1000; i++)
                {
                    var matches = await OpenDotaClient.Get100PublicMatchesBeforeMatchId(currentMatchId);
                    result.AddRange(matches);
                    currentMatchId = result.Last().MatchId;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return result;
        }
    }
}