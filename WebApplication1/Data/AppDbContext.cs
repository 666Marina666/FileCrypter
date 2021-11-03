using WebApplication1.Models;
using WebApplication1.Models.File;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNet.Identity;

namespace WebApplication1.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
{
}
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<RsaKeyPair> RsaKeyPairs { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<FileOnDatabaseModel> FilesOnDatabase { get; set; }
    }
}
