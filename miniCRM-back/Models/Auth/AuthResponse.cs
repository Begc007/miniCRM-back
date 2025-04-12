using miniCRM_back.DTOs;

namespace miniCRM_back.Models.Auth {
    public class AuthResponse {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public UserDto User { get; set; } = null!;
    }
}
