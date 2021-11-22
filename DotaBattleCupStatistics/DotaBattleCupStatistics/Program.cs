using System;
using System.Threading.Tasks;

namespace DotaBattleCupStatistics
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var matches = await OpenDotaClient.Get100PublicMatchesBeforeMatchId(6287642404);
            foreach (var match in matches)
            {
                Console.WriteLine(match.ToString());
            }
        }
    }
}