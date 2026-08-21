using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DeliveryService.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DeliveryService.Infrastructure.Services;

public sealed class TokenProvider(IConfiguration configuration) : ITokenProvider
{
    public TokenResult GenerateToken(UserTokenData user)
    {
        var settings = configuration.GetSection("JwtSettings");
        var expiresAt = DateTime.UtcNow.AddMinutes(settings.GetValue<int>("ExpiresInMinutes", 60));
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings["Key"]!));
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        if (!string.IsNullOrWhiteSpace(user.Email))
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            issuer: settings["Issuer"],
            audience: settings["Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new TokenResult(new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var settings = configuration.GetSection("JwtSettings");
        var parameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings["Key"]!)),
            ValidateIssuer = true,
            ValidIssuer = settings["Issuer"],
            ValidateAudience = true,
            ValidAudience = settings["Audience"],
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero,
        };

        return new JwtSecurityTokenHandler().ValidateToken(token, parameters, out _);
    }
}
