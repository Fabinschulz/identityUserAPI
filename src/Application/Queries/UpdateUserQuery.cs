namespace IdentityUser.src.Application.Queries
{
    public sealed record UpdateUserQuery
    {
        public Guid id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;

    }
}
