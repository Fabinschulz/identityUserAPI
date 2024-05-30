namespace IdentityUser.src.Application.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }

        public BadRequestException(string[] errors) : base("Multiple errors ocurred. See the Errors property for details.")
        {
            Errors = errors;
        }

        public string[] Errors { get; }
    }
}
