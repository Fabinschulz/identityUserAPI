namespace IdentityUser.src.Application.Queries
{

    /// <summary>
    /// Represents a query to get a user by their unique identifier.
    /// </summary>
    public sealed record GetUserByIdQuery
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the role of the user.
        /// </summary>
        public required string Role { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
    }

}
