using System;
using System.Threading.Tasks;

namespace DotaBattleCupStatistics
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var matches = await OpenDotaClient.GetPublicMatches();
            foreach (var match in matches)
            {
                Console.WriteLine(match.ToString());
            }
        }
    }
}