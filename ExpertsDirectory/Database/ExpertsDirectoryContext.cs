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
    }
}