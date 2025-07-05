namespace OlympiaWebService.Models {
    public class Answer {
        public string IDAnswer { get; set; }
        public string Answ {  get; set; }
        public string? Picture { get; set; }
        public string? Note { get; set; }
        public int? NumChars { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
