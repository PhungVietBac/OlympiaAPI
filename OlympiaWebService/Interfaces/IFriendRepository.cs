using OlympiaWebService.Models;

namespace OlympiaWebService.Interfaces {
    public interface IFriendRepository {
        ICollection<Friend> GetFriends();
        bool IsFriendExists(string id);
        bool IsFriendPairExists(string id1, string id2);
        ICollection<Player> GetFriend(string id);
        ICollection<Player> GetRequest(string id);
        bool Save();
        bool CreateFriend(Friend friend);
        bool UpdateFriend(Friend friend);
        bool DeleteFriend(Friend friend);
    }
}
