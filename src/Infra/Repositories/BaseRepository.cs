using IdentityUser.src.Domain.Common;
using IdentityUser.src.Domain.Interfaces;
using IdentityUser.src.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IdentityUser.src.Infra.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(Guid id)
        {
            var user = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                throw new KeyNotFoundException($"Entidade com o ID '{id}' não foi encontrada.");
            }
            return user;
        }

        public async Task<T> Create(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
            return entity;
        }

        public async Task<T> Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
            return entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

            if (entity != null)
            {
                _context.Set<T>().Update(entity);
                await _context.SaveChangesAsync();
                await Task.CompletedTask;
                return true;
            }

            return false;
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    }
}
