using System.Diagnostics;

namespace OlympiaWebService.Models {
    public class Rating {
        public string IDPlayer { get; set; }
        public string Comment { get; set; }
        public DateTime Time { get; set; }
        public Player Player { get; set; }
    }
}
