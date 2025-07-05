using Microsoft.EntityFrameworkCore;
using OlympiaWebService.Data;
using OlympiaWebService.Interfaces;
using OlympiaWebService.Models;

namespace OlympiaWebService.Repository {
    public class PlayerRepository : IPlayerRepository {
        private readonly DataContext _context;
        public PlayerRepository(DataContext context) {
            _context = context;
        }

        public ICollection<ICollection<Match>> GetMatchFromPlayer(string select, string lookup) {
            MatchRepository matches = new MatchRepository(_context);
            if (select == "name") {
                ICollection<Player> players = GetPlayerByName(lookup);
                HashSet<string> id = players.Select(p => p.IDPlayer).ToHashSet();
                HashSet<ICollection<Match>> ms = new HashSet<ICollection<Match>>();
                foreach (string i in id) {
                    ms.Add(matches.GetMatchBy("idPlayer", i));
                }
                return ms;
            } else {
                string id = GetPlayerBy(select, lookup).IDPlayer;
                HashSet<ICollection<Match>> ms = [matches.GetMatchBy("idPlayer", id)];
                return ms;
            }
        }

        public ICollection<ICollection<Rating>> GetRatingFromPlayer(string select, string lookup) {
            RatingRepository ratingRepository = new RatingRepository(_context);
            if (select == "name") {
                ICollection<Player> players = GetPlayerByName(lookup);
                HashSet<string> id = players.Select(p => p.IDPlayer).ToHashSet();
                HashSet<ICollection<Rating>> rs = new HashSet<ICollection<Rating>>();
                foreach (string i in id) {
                    rs.Add(ratingRepository.GetRatingBy("idPlayer", i));
                }
                return rs;
            } else {
                string id = GetPlayerBy(select, lookup).IDPlayer;
                HashSet<ICollection<Rating>> rs = [ratingRepository.GetRatingBy("idPlayer", id)];
                return rs;
            }
        }

        public ICollection<ICollection<Player>> GetFriendFromPlayer(string select, string lookup) {
            FriendRepository friends = new FriendRepository(_context);
            if (select == "name") {
                ICollection<Player> players = GetPlayerByName(lookup);
                HashSet<string> id = players.Select(p => p.IDPlayer).ToHashSet();
                HashSet<ICollection<Player>> ps = new HashSet<ICollection<Player>>();
                foreach (string i in id) {
                    ps.Add(friends.GetFriend(i));
                }
                return ps;
            } else {
                string id = GetPlayerBy(select, lookup).IDPlayer;
                HashSet<ICollection<Player>> ps = [friends.GetFriend(id)];
                return ps;
            }
        }

        public Player GetPlayerBy(string select, string lookup) {
            if (select == "id")
                return _context.Players.Where(p => p.IDPlayer == lookup).AsNoTracking().FirstOrDefault();
            else if (select == "username")
                return _context.Players.Where(p => p.Username == lookup).AsNoTracking().FirstOrDefault();
            else if (select == "email")
                return _context.Players.Where(p => p.Email == lookup).AsNoTracking().FirstOrDefault();
            else if (select == "phone")
                return _context.Players.Where(p => p.PhoneNumber == lookup).AsNoTracking().FirstOrDefault();
            return null;
        }

        public ICollection<Player> GetPlayerByName(string name) {
            return _context.Players.Where(p => p.Name == name).ToHashSet();
        }

        public ICollection<Player> GetPlayers() {
            return _context.Players.OrderBy(p => p.IDPlayer).ToHashSet();
        }

        public bool IsPlayerExists(string select, string lookup) {
            if (select == "id")
                return _context.Players.Any(p => p.IDPlayer == lookup);
            else if (select == "username")
                return _context.Players.Any(p => p.Username == lookup);
            else if (select == "email")
                return _context.Players.Any(p => p.Email == lookup);
            else if (select == "name")
                return _context.Players.Any(p => p.Name == lookup);
            else if (select == "phone")
                return _context.Players.Any(p => p.PhoneNumber == lookup);
            else if (select == "avatar")
                return _context.Players.Any(p => p.Avatar.ToString() == lookup);
            return false;
        }

        public bool Save() {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool CreatePlayer(Player player) {
            _context.Add(player);
            return Save();
        }

        public bool UpdatePlayer(Player player) {
            _context.Update(player);
            return Save();
        }

        public bool DeletePlayer(Player player) {
            _context.Remove(player);
            return Save();
        }

        public ICollection<Player> GetPlayerByAvatar(string avatar) {
            return _context.Players.Where(p => p.Avatar.ToString() == avatar).ToHashSet();
        }
    }
}
