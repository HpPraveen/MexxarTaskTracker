
using MexxarTaskTracker.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MexxarTaskTracker.Infrastructure
{
    public class DbContext : IdentityDbContext<ApplicationUser>
    {
        public DbContext(DbContextOptions dbContext) : base(dbContext) { }

        public DbSet<Domain.Task> Tasks { get; set; }
        public DbSet<ToDoList> ToDoLists { get; set; }
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
