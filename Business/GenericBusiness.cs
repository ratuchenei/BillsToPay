using Models;
using Repository;
using System;
using System.Threading.Tasks;

namespace Business
{
    public abstract class GenericBusiness<T> : BaseBusiness, IGenericBusiness<T> where T : Base
    {
        private readonly IGenericRepository<T> _repository;

        public GenericBusiness(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual async Task DeleteAsync(params T[] obj)
        {
            foreach (var i in obj)
            {
                i.UpdateDateTime = DateTime.UtcNow;
                i.IsEnable = false;
                await UpdateAsync(i);
            }
        }

        public virtual async Task InsertAsync(params T[] obj)
        {
            await _repository.InsertAsync(obj);
        }

        public virtual async Task UpdateAsync(T obj)
        {
            obj.UpdateDateTime = DateTime.UtcNow;

            await _repository.UpdateAsync(obj);
        }
    }
}
