namespace miniCRM_back.Models.Auth {
    public class RegisterRequest {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public string? FIO { get; set; }
        public string? Position { get; set; }
        
    }
}
