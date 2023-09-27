using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TennisApp2.Models;

namespace TennisApp2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TennisApp2.Models.Coach> Coach { get; set; } = default!;
        public DbSet<TennisApp2.Models.Member> Member { get; set; } = default!;
        public DbSet<TennisApp2.Models.Schedule> Schedule { get; set; } = default!;
        public DbSet<TennisApp2.Models.ScheduleJoin> ScheduleJoin { get; set; } = default!;
    }
}