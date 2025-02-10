namespace IdentityUser.src.Application.Common.Exceptions
{
    /// <summary>
    /// Represents errors that occur during application execution due to a bad request.
    /// </summary>
    public class BadRequestException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public BadRequestException(string message) : base(message)
        {
            Errors = new string[] { message };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/> class with multiple error messages.
        /// </summary>
        /// <param name="errors">An array of error messages.</param>
        public BadRequestException(string[] errors) : base("Multiple errors ocurred. See the Errors property for details.")
        {
            Errors = errors;
        }

        /// <summary>
        /// Gets the error messages associated with the exception.
        /// </summary>
        public string[] Errors { get; }
    }
}
