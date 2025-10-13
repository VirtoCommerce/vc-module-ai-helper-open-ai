using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VirtoCommerce.AiHelperOpenAi.Data.Repositories;

namespace VirtoCommerce.AiHelperOpenAi.Data.SqlServer;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AiHelperOpenAiDbContext>
{
    public AiHelperOpenAiDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<AiHelperOpenAiDbContext>();
        var connectionString = args.Length != 0 ? args[0] : "Server=(local);User=virto;Password=virto;Database=VirtoCommerce3;";

        builder.UseSqlServer(
            connectionString,
            options => options.MigrationsAssembly(typeof(SqlServerDataAssemblyMarker).Assembly.GetName().Name));

        return new AiHelperOpenAiDbContext(builder.Options);
    }
}
