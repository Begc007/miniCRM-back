﻿namespace miniCRM_back.Models.Auth {
    public class LoginRequest {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
