using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotaBattleCupStatistics
{
    internal sealed class LocalStorage
    {
        private readonly string _fileName;
        private readonly Data _data = new();

        public LocalStorage()
        {
            _fileName = $"LocalStorage_Matches_{DateTime.Now:yyyy-MM-ddTHH-mm-ss.fffffff}.json";
        }

        public void SetMatches(IReadOnlyCollection<OpenDotaMatch> matches)
        {
            _data.Matches = matches;
        }

        public void SetTotalMatches(long count)
        {
            _data.TotalMatches = count;
        }

        public void SetBattleCupMatches(long count)
        {
            _data.BattleCupMatches = count;
        }

        public async Task Save()
        {
            await using var fileStream = File.Create(_fileName);
            await JsonSerializer.SerializeAsync(fileStream, _data);
        }

        private sealed class Data
        {
            public long? TotalMatches { get; set; }
            public long? BattleCupMatches { get; set; }
            public IReadOnlyCollection<OpenDotaMatch>? Matches { get; set; }
        }
    }
}