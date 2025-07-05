using OlympiaWebService.Data;
using OlympiaWebService.Interfaces;
using OlympiaWebService.Models;

namespace OlympiaWebService.Repository {
    public class RatingRepository : IRatingRepository {
        private readonly DataContext _context;

        public RatingRepository(DataContext context) {
            _context = context;            
        }

        public bool CreateRating(Rating rating) {
            _context.Add(rating);
            return Save();
        }

        public bool DeleteRating(Rating rating) {
            _context.Remove(rating);
            return Save();
        }

        public ICollection<Player> GetPlayerByRating(string time) {
            return _context.Ratings.Where(r => r.Time == DateTime.Parse(time)).Select(p => p.Player).ToList();
        }

        public ICollection<Rating> GetRatingBy(string select, string lookup) {
            if (select == "idPlayer")
                return _context.Ratings.Where(r => r.IDPlayer == lookup).ToList();
            else if (select == "time")
                return _context.Ratings.Where(r => r.Time == DateTime.Parse(lookup)).ToList();
            return null;
        }

        public ICollection<Rating> GetRatings() {
            return _context.Ratings.OrderBy(r => r.IDPlayer).ToList();
        }

        public ICollection<DateTime> GetTimeByRating(string idPlayer) {
            return _context.Ratings.Where(r => r.IDPlayer == idPlayer).Select(r => r.Time).ToList();
        }

        public bool IsRatingExists(string select, string lookup) {
            if (select == "idPlayer")
                return _context.Ratings.Any(r => r.IDPlayer == lookup);
            else if (select == "time")
                return _context.Ratings.Any(r => r.Time == DateTime.Parse(lookup));
            return false;
        }

        public bool Save() {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
