using Models;
using System.Threading.Tasks;

namespace Repository
{
    public abstract class GenericRepository<T> : BaseRepository, IGenericRepository<T> where T : Base
    {
        public GenericRepository(DatabaseSettings settings) : base(settings)
        {
        }

        public async Task DeleteAsync(params T[] obj)
        {
            using (var context = GetContext())
            {
                context.RemoveRange(obj);
                await context.SaveChangesAsync();
            }
        }

        public async Task InsertAsync(params T[] obj)
        {
            using (var context = GetContext())
            {
                await context.Set<T>().AddRangeAsync(obj);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(T obj)
        {
            using (var context = GetContext())
            {
                context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}
