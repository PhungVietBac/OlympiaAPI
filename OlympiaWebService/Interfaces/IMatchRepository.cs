using OlympiaWebService.Models;

namespace OlympiaWebService.Interfaces {
    public interface IMatchRepository {
        ICollection<Match> GetMatches();
        ICollection<Match> GetMatchBy(string select, string lookup);
        bool IsMatchExists(string select, string lookup);
        ICollection<Player> GetPlayerByMatch(string select, string lookup);
        ICollection<Room> GetRoomByMatch(string select, string lookup);
        ICollection<DateTime> GetTimeByMatch (string select, string lookup);
        bool Save();
        bool CreateMatch(Match match);
        bool DeleteMatch(Match match);
    }
}
