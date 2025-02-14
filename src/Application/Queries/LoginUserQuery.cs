namespace IdentityUser.src.Application.Queries
{
    /// <summary>
    /// Represents a query for logging in a user.
    /// </summary>
    public sealed record LoginUserQuery
    {

        /// <summary>
        /// Gets the unique identifier for the login user query.
        /// </summary>
        public Guid? Id { get; init; }

        /// <summary>
        /// Gets the username of the user.
        /// </summary>
        public string Username { get; init; } = string.Empty;

        /// <summary>
        /// Gets the email of the user.
        /// </summary>
        public string Email { get; init; } = string.Empty;

        /// <summary>
        /// Gets the token associated with the user.
        /// </summary>
        public string Token { get; init; } = string.Empty;
    }
}
