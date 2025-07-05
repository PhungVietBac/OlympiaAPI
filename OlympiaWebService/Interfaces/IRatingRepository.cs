using OlympiaWebService.Models;

namespace OlympiaWebService.Interfaces {
    public interface IRatingRepository {
        ICollection<Rating> GetRatings();
        bool IsRatingExists(string select, string lookup);
        ICollection<Rating> GetRatingBy(string select, string lookup);
        ICollection<Player> GetPlayerByRating(string time);
        ICollection<DateTime> GetTimeByRating(string idPlayer);
        bool Save();
        bool CreateRating(Rating rating);
        bool DeleteRating(Rating rating);
    }
}
