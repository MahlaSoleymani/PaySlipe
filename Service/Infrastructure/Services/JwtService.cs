using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Common;
using Database.Repositories.RepositoryWrapper;
using Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Service.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly SiteSettings _siteSetting;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public JwtService(IOptionsSnapshot<SiteSettings> settings, SignInManager<User> signInManager, UserManager<User> userManager, IRepositoryWrapper repository, IMapper mapper)
        {
            _siteSetting = settings.Value;
            _signInManager = signInManager;
            _userManager = userManager;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AccessToken> GenerateAsync(User user, List<Claim> customeClaims = null, bool driverLogin = false)
        {
            var secretKey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.SecretKey); // longer that 16 character
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            var encryptionkey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.Encryptkey); //must be 16 character
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            var claims = await GetClaimsAsync(user, customeClaims);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _siteSetting.JwtSettings.Issuer,
                Audience = _siteSetting.JwtSettings.Audience,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.NotBeforeMinutes),
                Expires = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.ExpirationMinutes),
                // Expires = DateTime.Now.AddSeconds(15),
                SigningCredentials = signingCredentials,
                //EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(claims),
            };

            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            //JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);

            //string encryptedJwt = tokenHandler.WriteToken(securiyToken);

            return new AccessToken(securityToken);
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(User user, List<Claim> customeClaims)
        {
            var result = await _signInManager.ClaimsFactory.CreateAsync(user);
            var claims = new List<Claim>(result.Claims);

            if (customeClaims is { Count: > 0 })
                claims.AddRange(customeClaims);

            var roles = _repository.UserRole.TableNoTracking
                .Where(x => x.UserId == user.Id)
                .Select(x => x.Role);

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role.Name!));

            return claims;

        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}