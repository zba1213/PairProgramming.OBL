using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MusicRecords.Api.Auth;

public sealed class AuthSettings
{
    public const string Issuer = "MusicRecords.Api";
    public const string Audience = "MusicRecords.Frontend";
    public string? SigningKey { get; init; }
    public LoginUser[] Users { get; init; } = [];

    public SymmetricSecurityKey? GetSigningKey()
    {
        if (SigningKey is null && Users.Length == 0)
            return null;

        if (SigningKey is null || Encoding.UTF8.GetByteCount(SigningKey) < 32)
            throw new InvalidOperationException("Auth:SigningKey must contain at least 32 bytes.");

        if (Users.Any(user => string.IsNullOrWhiteSpace(user.Username)
            || string.IsNullOrWhiteSpace(user.Password)
            || string.IsNullOrWhiteSpace(user.Role))
            || Users.Select(user => user.Username).Distinct(StringComparer.OrdinalIgnoreCase).Count() != Users.Length)
        {
            throw new InvalidOperationException("Auth:Users requires unique usernames, passwords and roles.");
        }

        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey));
    }
}

public sealed record LoginUser(string Username, string Password, string Role);
