﻿namespace Be_My_Voice_Backend.Models.DTO
{
    public class LoginResponseDTO
    {
        public string RequestId { get; set; }
        public UserModel User { get; set; }
        public string Token { get; set; }
    }
}
