using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.AiHelperOpenAi.Data.Repositories;

public class AiHelperOpenAiDbContext : DbContextBase
{
    public AiHelperOpenAiDbContext(DbContextOptions<AiHelperOpenAiDbContext> options)
        : base(options)
    {
    }

    protected AiHelperOpenAiDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //modelBuilder.Entity<AiHelperOpenAiEntity>().ToTable("AiHelperOpenAi").HasKey(x => x.Id);
        //modelBuilder.Entity<AiHelperOpenAiEntity>().Property(x => x.Id).HasMaxLength(IdLength).ValueGeneratedOnAdd();

        switch (Database.ProviderName)
        {
            case "Pomelo.EntityFrameworkCore.MySql":
                modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.AiHelperOpenAi.Data.MySql"));
                break;
            case "Npgsql.EntityFrameworkCore.PostgreSQL":
                modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.AiHelperOpenAi.Data.PostgreSql"));
                break;
            case "Microsoft.EntityFrameworkCore.SqlServer":
                modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.AiHelperOpenAi.Data.SqlServer"));
                break;
        }
    }
}
