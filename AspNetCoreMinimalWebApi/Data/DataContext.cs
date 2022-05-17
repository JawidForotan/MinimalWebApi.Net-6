using AspNetWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetWebApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Student> students => Set<Student>();
    }
}
