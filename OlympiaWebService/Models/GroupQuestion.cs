namespace OlympiaWebService.Models {
    public class GroupQuestion {
        public string IDQuestion { get; set; }
        public string IDGroup { get; set; }
        public Question Main { get; set; }
        public Question Member { get; set; }
    }
}
