using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Data;

namespace Repository
{
    public abstract class BaseRepository : IBaseRepository
    {
        private readonly DatabaseSettings _settings;

        protected BaseRepository(DatabaseSettings settings)
        {
            _settings = settings;
        }

        protected Context GetContext()
        {
            var options = new DbContextOptionsBuilder<Context>();
            options.UseSqlServer(_settings.ConnectionString);

            var context = new Context(options.Options);
            
            return context;
        }

        protected IDbConnection GetConnection()
        {
            return new SqlConnection(_settings.ConnectionString);
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
