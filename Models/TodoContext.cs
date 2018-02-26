using Microsoft.EntityFrameworkCore;

namespace MarlonApi.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoStudent> TodoItems { get; set; }

    }
}