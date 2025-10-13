using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VirtoCommerce.AiHelperOpenAi.Data.Repositories;

namespace VirtoCommerce.AiHelperOpenAi.Data.PostgreSql;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AiHelperOpenAiDbContext>
{
    public AiHelperOpenAiDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<AiHelperOpenAiDbContext>();
        var connectionString = args.Length != 0 ? args[0] : "Server=localhost;Username=virto;Password=virto;Database=VirtoCommerce3;";

        builder.UseNpgsql(
            connectionString,
            options => options.MigrationsAssembly(typeof(PostgreSqlDataAssemblyMarker).Assembly.GetName().Name));

        return new AiHelperOpenAiDbContext(builder.Options);
    }
}
