namespace OlympiaWebService.Dto {
    public class QuestionDto {
        public string IDQuestion { get; set; }
        public int Round { get; set; }
        public string Quest { get; set; }
        public string Media { get; set; }
        public bool IsMain { get; set; }
        public string IDAnswer { get; set; }
        public int Time { get; set; }
    }
}
