using OlympiaWebService.Data;
using OlympiaWebService.Interfaces;
using OlympiaWebService.Models;

namespace OlympiaWebService.Repository {
    public class RoomRepository : IRoomRepository {
        private readonly DataContext _context;
        public RoomRepository(DataContext context) {
            _context = context;
        }

        public bool CreateRoom(Room room) {
            _context.Add(room);
            return Save();
        }

        public bool DeleteRoom(Room room) {
            _context.Remove(room);
            return Save();
        }

        public ICollection<Match> GetMatchByRoom(string id) {
            MatchRepository matchRepository = new MatchRepository(_context);
            return matchRepository.GetMatchBy("idRoom", id);
        }

        public ICollection<Room> GetRoom(string type) {
            bool select = false;
            if (type == "full")
                select = true;
            return _context.Rooms.Where(r => r.IsFull == select).ToList();
        }

        public Room GetRoomById(string id) {
            return _context.Rooms.Where(r => r.IDRoom == id).FirstOrDefault();
        }

        public ICollection<Room> GetRooms() {
            return _context.Rooms.OrderBy(r => r.IDRoom).ToList();
        }

        public bool IsRoomExists(string id) {
            return _context.Rooms.Any(r => r.IDRoom == id);
        }

        public bool Save() {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
