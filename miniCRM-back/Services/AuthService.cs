using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using miniCRM_back.Database;
using miniCRM_back.Models;
using miniCRM_back.Models.Auth;
using miniCRM_back.Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace miniCRM_back.Services {
    public class AuthService : IAuthService {
        private readonly crmDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public AuthService(crmDbContext context, IOptions<JwtSettings> jwtSettings) {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthResponse?> LoginAsync(Models.Auth.LoginRequest request) {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == request.Username);

            if (user == null || !VerifyPasswordHash(request.Password, user.PasswordHash)) {
                return null;
            }

            var token = GenerateJwtToken(user);

            return new AuthResponse {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            };
        }

        public async Task<AuthResponse?> RegisterAsync(Models.Auth.RegisterRequest request) {
            if (await _context.Users.AnyAsync(u => u.Name == request.Username)) {
                return null; // Пользователь с таким именем уже существует
            }

            // Создание нового пользователя
            var user = new User {
                Name = request.Username,
                PasswordHash = HashPassword(request.Password),
                FIO = request.FIO,
                Position = request.Position,
                CreateTimestamp = DateTime.UtcNow,
                CreatedBy = "System"
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);

            return new AuthResponse {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            };
        }

        public string GenerateJwtToken(User user) {
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string HashPassword(string password) {
            // Используйте безопасный алгоритм хеширования
            // В реальном приложении лучше использовать BCrypt или Argon2
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPasswordHash(string password, string storedHash) {
            // Соответствующая проверка пароля
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashedPassword = Convert.ToBase64String(hashedBytes);

            return storedHash == hashedPassword;
        }
    }


}
