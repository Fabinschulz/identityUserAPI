using IdentityUser.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityUser.src.Domain.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
