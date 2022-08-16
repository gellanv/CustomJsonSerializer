using Microsoft.EntityFrameworkCore;
using TestTask.Data.Models;

namespace TestTask.Models
{
    public class ApiDBContext : DbContext
    {
        public ApiDBContext(DbContextOptions<ApiDBContext> options)
            : base(options)
        {
        }
        public DbSet<Address> Address { get; set; }
        public DbSet<Person> Person { get; set; }
    }
}
