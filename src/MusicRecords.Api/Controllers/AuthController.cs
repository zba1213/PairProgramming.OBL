using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MusicRecords.Api.Auth;

namespace MusicRecords.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(AuthSettings settings) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public ActionResult<LoginResponse> Login(LoginRequest request)
    {
        var user = settings.Users.FirstOrDefault(candidate =>
            string.Equals(candidate.Username, request.Username.Trim(), StringComparison.OrdinalIgnoreCase));
        var passwordMatches = CryptographicOperations.FixedTimeEquals(
            SHA256.HashData(Encoding.UTF8.GetBytes(request.Password)),
            SHA256.HashData(Encoding.UTF8.GetBytes(user?.Password ?? string.Empty)));
        if (user is null || !passwordMatches)
            return Unauthorized();

        var now = DateTime.UtcNow;
        var expiresAt = now.AddMinutes(30);
        var token = new JwtSecurityToken(
            issuer: AuthSettings.Issuer,
            audience: AuthSettings.Audience,
            claims: [new Claim(JwtRegisteredClaimNames.Sub, user.Username), new Claim("role", user.Role)],
            notBefore: now,
            expires: expiresAt,
            signingCredentials: new SigningCredentials(settings.GetSigningKey()!, SecurityAlgorithms.HmacSha256));
        return Ok(new LoginResponse(new JwtSecurityTokenHandler().WriteToken(token), user.Username, user.Role, expiresAt));
    }
}

public sealed record LoginRequest(
    [Required, StringLength(100)] string Username,
    [Required, StringLength(256)] string Password);

public sealed record LoginResponse(string Token, string Username, string Role, DateTime ExpiresAt);
