using ExpertsDirectory.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace ExpertsDirectory.Database
{
    public class ExpertsDirectoryContext : DbContext
    {
        public static readonly ILoggerFactory ConsoleLogger
            = LoggerFactory.Create(builder => builder.AddConsole());

        public DbSet<Member> Members { get; set; } = default!;

        public string DbPath { get; }

        public ExpertsDirectoryContext()
        {
            const Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}Experts.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options
                .UseLoggerFactory(ConsoleLogger)
                .EnableSensitiveDataLogging()
                .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ExpertsDirectory");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //#pragma warning disable 8634
            //            modelBuilder.Entity<Member>().Property(m => m.Friends).HasJsonConversion();
            //#pragma warning restore 8634
            //            modelBuilder.Entity<Member>().Property(m => m.Tags).HasJsonConversion();

            //modelBuilder.Entity<Member>().Property(m => m.Friends).HasConversion(
            //    friends => friends.ToJson(),
            //    json => json.FromJson<List<Friend>>());

            //modelBuilder.Entity<Member>().Property(m => m.Tags).HasConversion(
            //    tags => tags.ToJson(),
            //    json => json.FromJson<List<string>>());
        }
    }
}