using OlympiaWebService.Helper;

namespace OlympiaWebService.Dto {
    public class PlayerDto {
        public string IDPlayer { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int WinCount { get; set; }
        public byte[] Avatar { get; set; }
    }
}
