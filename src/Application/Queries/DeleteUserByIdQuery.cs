namespace IdentityUser.src.Application.Queries
{
    public sealed class DeleteUserByIdQuery
    {
        public bool IsSuccess { get; }

        protected DeleteUserByIdQuery(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
    }
}
