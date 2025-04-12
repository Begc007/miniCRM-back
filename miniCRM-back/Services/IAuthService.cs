using miniCRM_back.Models;
using miniCRM_back.Models.Auth;

namespace miniCRM_back.Services {
    public interface IAuthService {
        Task<AuthResponse?> LoginAsync(LoginRequest request);
        Task<AuthResponse?> RegisterAsync(RegisterRequest request);
        string GenerateJwtToken(User user);
    }
}
