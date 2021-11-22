using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotaBattleCupStatistics
{
    internal static class LocalStorage
    {
        public static async Task SaveMatches(IEnumerable<OpenDotaMatch> matches)
        {
            var fileName = $"LocalStorage_Matches_{DateTime.Now:yyyy-MM-ddTHH-mm-ss.fffffff}.json";
            await using var fileStream = File.Create(fileName);
            await JsonSerializer.SerializeAsync(fileStream, matches);
        }
    }
}