using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Tweetbook.Domain;
using Tweetbook.Options;
using Tweetbook.Services.Interface;

namespace Tweetbook.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        
        public IdentityService(
            UserManager<IdentityUser> userManager, JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
        }
        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            var existing = await _userManager.FindByEmailAsync(email);
            if (existing != null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] {"User with this email address already exists"}
                };
            }
            
            var newUser = new IdentityUser
            {
                Email = email,
                UserName = email,
            };

            var created = await _userManager.CreateAsync(newUser, password);
            if (!created.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = created.Errors.Select(x => x.Description)
                };
            }
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, newUser.Email), 
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
                    new Claim(JwtRegisteredClaimNames.Email, newUser.Email), 
                    new Claim("id", newUser.Id), 
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}