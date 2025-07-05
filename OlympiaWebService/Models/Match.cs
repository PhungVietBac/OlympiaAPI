namespace OlympiaWebService.Models {
    public class Match {
        public string IDPlayer { get; set; }
        public string IDRoom { get; set; }
        public DateTime Time { get; set; }
        public Player Player { get; set; }
        public Room Room { get; set; }
    }
}
