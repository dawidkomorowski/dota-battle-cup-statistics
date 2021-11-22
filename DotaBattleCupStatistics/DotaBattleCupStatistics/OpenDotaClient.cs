using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DotaBattleCupStatistics
{
    internal static class OpenDotaClient
    {
        private static readonly Uri BaseUrl = new("https://api.opendota.com/api/");

        public static async Task<IEnumerable<OpenDotaMatch>> GetTop100LatestPublicMatches()
        {
            var apiUrl = new Uri(BaseUrl, "publicMatches");
            var client = new HttpClient();
            var result = await client.GetFromJsonAsync<IReadOnlyCollection<MatchDto>>(apiUrl);

            if (result == null)
            {
                throw new InvalidOperationException($"Unexpected null value from API: {apiUrl.AbsoluteUri}");
            }

            return result.Select(dto => new OpenDotaMatch
            {
                MatchId = dto.Match_Id,
                StartTime = UnixTimeStampToDateTime(dto.Start_Time),
                LobbyType = ConvertLobbyType(dto.Lobby_Type)
            });
        }

        public static async Task<IEnumerable<OpenDotaMatch>> Get100PublicMatchesBeforeMatchId(long matchId)
        {
            var apiUrl = new Uri(BaseUrl, $"publicMatches?less_than_match_id={matchId}");
            var client = new HttpClient();
            var result = await client.GetFromJsonAsync<IReadOnlyCollection<MatchDto>>(apiUrl);

            if (result == null)
            {
                throw new InvalidOperationException($"Unexpected null value from API: {apiUrl.AbsoluteUri}");
            }

            return result.Select(dto => new OpenDotaMatch
            {
                MatchId = dto.Match_Id,
                StartTime = UnixTimeStampToDateTime(dto.Start_Time),
                LobbyType = ConvertLobbyType(dto.Lobby_Type)
            });
        }

        private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTimeStamp).ToLocalTime();
        }

        private static OpenDotaLobbyType ConvertLobbyType(int lobbyType)
        {
            return lobbyType switch
            {
                0 => OpenDotaLobbyType.Normal,
                1 => OpenDotaLobbyType.Practice,
                2 => OpenDotaLobbyType.Tournament,
                3 => OpenDotaLobbyType.Tutorial,
                4 => OpenDotaLobbyType.CoopBots,
                5 => OpenDotaLobbyType.RankedTeamMM,
                6 => OpenDotaLobbyType.RankedSoloMM,
                7 => OpenDotaLobbyType.Ranked,
                8 => OpenDotaLobbyType.Mid1Vs1,
                9 => OpenDotaLobbyType.BattleCup,
                _ => throw new ArgumentOutOfRangeException(nameof(lobbyType), lobbyType, "Unknown lobby type.")
            };
        }

        private sealed class MatchDto
        {
            // ReSharper disable once InconsistentNaming
            public long Match_Id { get; set; }

            // ReSharper disable once InconsistentNaming
            public long Start_Time { get; set; }

            // ReSharper disable once InconsistentNaming
            public int Lobby_Type { get; set; }
        }
    }

    internal sealed record OpenDotaMatch
    {
        public long MatchId { get; init; }
        public DateTime StartTime { get; init; }
        public OpenDotaLobbyType LobbyType { get; init; }
    }

    internal enum OpenDotaLobbyType
    {
        Normal,
        Practice,
        Tournament,
        Tutorial,
        CoopBots,

        // ReSharper disable once InconsistentNaming
        RankedTeamMM,

        // ReSharper disable once InconsistentNaming
        RankedSoloMM,
        Ranked,
        Mid1Vs1,
        BattleCup
    }
}