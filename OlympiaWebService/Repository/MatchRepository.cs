using OlympiaWebService.Data;
using OlympiaWebService.Interfaces;
using OlympiaWebService.Models;

namespace OlympiaWebService.Repository {
    public class MatchRepository : IMatchRepository {
        private readonly DataContext _context;
        public MatchRepository(DataContext context) {
            _context = context;
        }

        public bool CreateMatch(Match match) {
            _context.Add(match);
            return Save();
        }

        public bool DeleteMatch(Match match) {
            _context.Remove(match);
            return Save();
        }

        public ICollection<Match> GetMatchBy(string select, string lookup) {
            if (select == "idPlayer")
                return _context.Matches.Where(m => m.IDPlayer == lookup).ToList();
            else if (select == "idRoom")
                return _context.Matches.Where(m => m.IDRoom == lookup).ToList();
            else if (select == "time") 
                return _context.Matches.Where(m => m.Time == DateTime.Parse(lookup)).ToList();
            return null;
        }

        public ICollection<Match> GetMatches() {
            return _context.Matches.OrderBy(m => m.IDPlayer).ToList();
        }

        public ICollection<Player> GetPlayerByMatch(string select, string lookup) {
            if (select == "idRoom")
                return _context.Matches.Where(m => m.IDRoom == lookup).Select(p => p.Player).ToList();
            else if (select == "time")
                return _context.Matches.Where(m => m.Time == DateTime.Parse(lookup)).Select(p => p.Player).ToList();
            return null;
        }

        public ICollection<Room> GetRoomByMatch(string select, string lookup) {
            if (select == "idPlayer")
                return _context.Matches.Where(m => m.IDPlayer == lookup).Select(r => r.Room).ToList();
            else if (select == "time")
                return _context.Matches.Where(m => m.Time == DateTime.Parse(lookup)).Select(r => r.Room).ToList();
            return null;
        }

        public ICollection<DateTime> GetTimeByMatch(string select, string lookup) {
            if (select == "idPlayer")
                return _context.Matches.Where(m => m.IDPlayer == lookup).Select(d => d.Time).ToList();
            else if (select == "idRoom")
                return _context.Matches.Where(m => m.IDRoom == lookup).Select(d => d.Time).ToList();
            return null;
        }

        public bool IsMatchExists(string select, string lookup) {
            if (select == "idPlayer")
                return _context.Matches.Any(m => m.IDPlayer == lookup);
            else if (select == "idRoom")
                return _context.Matches.Any(m => m.IDRoom == lookup);
            else if (select == "time")
                return _context.Matches.Any(m => m.Time == DateTime.Parse(lookup));
            return false;
        }

        public bool Save() {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
