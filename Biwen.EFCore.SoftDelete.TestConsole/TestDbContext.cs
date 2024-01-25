using Biwen.EFCore.SoftDelete.TestConsole.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Biwen.EFCore.SoftDelete.TestConsole
{
    /// <summary>
    /// Test DbContext
    /// </summary>
    public class TestDbContext : SoftDeleteDbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {

        }

        public DbSet<Blog> Blogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

#if DEBUG
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
            //optionsBuilder.LogTo(Console.WriteLine);
#endif

            base.OnConfiguring(optionsBuilder);
        }
    }
}