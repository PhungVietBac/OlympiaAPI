using OlympiaWebService.Models;

namespace OlympiaWebService.Interfaces {
    public interface IPlayerRepository {
        ICollection<Player> GetPlayers();
        bool IsPlayerExists(string select, string lookup);
        Player GetPlayerBy(string select, string lookup);
        ICollection<Player> GetPlayerByName(string name);
        ICollection<Player> GetPlayerByAvatar(string avatar);
        ICollection<ICollection<Match>> GetMatchFromPlayer(string select, string lookup);
        ICollection<ICollection<Rating>> GetRatingFromPlayer(string select, string lookup);
        ICollection<ICollection<Player>> GetFriendFromPlayer(string select, string lookup);
        bool Save();
        bool CreatePlayer(Player player);
        bool UpdatePlayer(Player player);
        bool DeletePlayer(Player player);
    }
}
