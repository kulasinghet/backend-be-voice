﻿using System.Text.Json;

namespace Be_My_Voice_Backend.Models.DTO
{
    public class CreateTranslationDTO
    {
        public Guid sessionID { get; set; }
        public Guid userID { get; set; }
        public string resultObjectFromSkeleton { get; set; } = null;
        public string userType { get; set; } = "";
    }
}
