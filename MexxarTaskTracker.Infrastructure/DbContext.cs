
using MexxarTaskTracker.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MexxarTaskTracker.Infrastructure
{
    public class DbContext : IdentityDbContext<ApplicationUser>
    {
        public DbContext(DbContextOptions<DbContext> dbContext) : base(dbContext) { }

        public DbSet<Domain.UserTask> Tasks { get; set; }
        public DbSet<ToDoList> ToDoLists { get; set; }
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasMany(o => o.ToDoLists).WithOne(td => td.User).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ToDoList>().HasMany(td => td.Tasks).WithOne(t => t.ToDoList).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
