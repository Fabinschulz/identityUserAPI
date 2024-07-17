namespace IdentityUser.src.Application.Queries
{
    public sealed class DeleteUserByIdQuery
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        public DeleteUserByIdQuery(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
