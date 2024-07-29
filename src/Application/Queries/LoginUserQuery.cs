namespace IdentityUser.src.Application.Queries
{
    public sealed record LoginUserQuery
    {
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Token { get; init; } = string.Empty;
    }
}
