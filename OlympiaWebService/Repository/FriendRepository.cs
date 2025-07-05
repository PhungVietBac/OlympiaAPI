using OlympiaWebService.Data;
using OlympiaWebService.Interfaces;
using OlympiaWebService.Models;

namespace OlympiaWebService.Repository {
    public class FriendRepository : IFriendRepository {
        private readonly DataContext _context;

        public FriendRepository(DataContext context) {
            _context = context;
        }

        public bool CreateFriend(Friend friend) {
            _context.Add(friend);
            return Save();
        }

        public bool DeleteFriend(Friend friend) {
            _context.Remove(friend);
            return Save();
        }

        public ICollection<Player> GetFriend(string id) { 
            return _context.Friends.Where(f => f.IDSelf == id && f.IsAcpt).Select(p => p.FriendPlayer).Union(
                _context.Friends.Where(f => f.IDFriend == id && f.IsAcpt).Select(p => p.Player)).ToList();
        }

        public ICollection<Friend> GetFriends() {
            return _context.Friends.OrderBy(f => f.IDSelf).ToList();
        }

        public ICollection<Player> GetRequest(string id) {
            return _context.Friends.Where(f => f.IDFriend == id && !f.IsAcpt).Select(p => p.Player).ToList();
        }

        public bool IsFriendExists(string id) {
            return _context.Friends.Any(f => f.IDSelf == id) ||
                _context.Friends.Any(f => f.IDFriend == id);
        }

        public bool IsFriendPairExists(string id1, string id2) {
            return _context.Friends.Any(f => f.IDSelf == id1 && f.IDFriend == id2);
        }

        public bool Save() {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateFriend(Friend friend) {
            _context.Update(friend);
            return Save();
        }
    }
}
