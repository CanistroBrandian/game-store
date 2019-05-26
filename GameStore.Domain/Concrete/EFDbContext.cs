using GameStore.Domain.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public EFDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        var connectionString = GetConnectionString();

        //        optionsBuilder.UseSqlServer(connectionString)
        //        //.UseLoggerFactory(LoggerFactory)
        //        .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning))
        //        //.EnableDetailedErrors();
        //    }

        //    base.OnConfiguring(optionsBuilder);
        //}

        //private string GetConnectionString()
        //{
        //    var webConfig = new XmlDocument();
        //    const string pattern = "bin$";
        //    var path = Regex.Replace(AppDomain.CurrentDomain.BaseDirectory, pattern, string.Empty);
        //    webConfig.Load(File.OpenRead(Path.Combine(path, "web.config")));
        //    return webConfig.DocumentElement
        //        .SelectSingleNode("//connectionStrings/add[@name='EFDbContext']")
        //        .Attributes["connectionString"]
        //        .Value;
        //}

    }
}