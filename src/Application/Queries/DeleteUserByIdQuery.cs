namespace IdentityUser.src.Application.Queries
{
    /// <summary>
    /// Represents the result of a delete user by ID operation.
    /// </summary>
    public sealed class DeleteUserByIdQuery
    {
        /// <summary>
        /// Gets a value indicating whether the delete operation was successful.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets a message providing additional information about the delete operation.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteUserByIdQuery"/> class.
        /// </summary>
        /// <param name="isSuccess">A value indicating whether the delete operation was successful.</param>
        /// <param name="message">A message providing additional information about the delete operation.</param>
        public DeleteUserByIdQuery(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
