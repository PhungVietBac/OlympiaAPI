﻿using OlympiaWebService.Helper;

namespace OlympiaWebService.Dto {
    public class PlayerDtoSignup {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] Avatar { get; set; }
    }
}
