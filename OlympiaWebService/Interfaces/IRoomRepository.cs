using OlympiaWebService.Models;

namespace OlympiaWebService.Interfaces {
    public interface IRoomRepository {
        ICollection<Room> GetRooms();
        bool IsRoomExists(string id);
        Room GetRoomById(string id);
        ICollection<Room> GetRoom(string type);
        ICollection<Match> GetMatchByRoom(string id);
        bool Save();
        bool CreateRoom(Room room);
        bool DeleteRoom(Room room);
    }
}
