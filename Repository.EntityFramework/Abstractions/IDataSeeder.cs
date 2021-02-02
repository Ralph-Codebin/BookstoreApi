using Microsoft.EntityFrameworkCore;

namespace Repository.EntityFramework.Abstractions
{
    public interface IDataSeeder<T> where T: DbContext
    {
        void Seed(ModelBuilder modelBuilder);
    }
}
