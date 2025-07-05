namespace OlympiaWebService.Models {
    public class Room {
        public string IDRoom { get; set; }
        public bool IsFull { get; set; }
        public ICollection<Match> Matches { get; set; }
    }
}
