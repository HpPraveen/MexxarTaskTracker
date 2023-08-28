
using MexxarTaskTracker.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MexxarTaskTracker.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContext) : base(dbContext) { }

        public DbSet<UserTask> Tasks { get; set; }
        public DbSet<ToDoList> ToDoLists { get; set; }
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().HasKey(u => u.Id);
            builder.Entity<ApplicationUser>().HasMany(o => o.ToDoLists).WithOne(td => td.User).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ToDoList>().HasMany(td => td.Tasks).WithOne(t => t.ToDoList).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
