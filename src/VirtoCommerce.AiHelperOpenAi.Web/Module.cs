using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.AiHelper.Core.Services;
using VirtoCommerce.AiHelperOpenAi.Core;
using VirtoCommerce.AiHelperOpenAi.Data.MySql;
using VirtoCommerce.AiHelperOpenAi.Data.PostgreSql;
using VirtoCommerce.AiHelperOpenAi.Data.Repositories;
using VirtoCommerce.AiHelperOpenAi.Data.Services;
using VirtoCommerce.AiHelperOpenAi.Data.SqlServer;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.MySql.Extensions;
using VirtoCommerce.Platform.Data.PostgreSql.Extensions;
using VirtoCommerce.Platform.Data.SqlServer.Extensions;
using CoreModuleConstants = VirtoCommerce.AiHelper.Core.ModuleConstants;

namespace VirtoCommerce.AiHelperOpenAi.Web;

public class Module : IModule, IHasConfiguration
{
    public ManifestModuleInfo ModuleInfo { get; set; }
    public IConfiguration Configuration { get; set; }

    public void Initialize(IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<AiHelperOpenAiDbContext>(options =>
        {
            var databaseProvider = Configuration.GetValue("DatabaseProvider", "SqlServer");
            var connectionString = Configuration.GetConnectionString(ModuleInfo.Id) ?? Configuration.GetConnectionString("VirtoCommerce");

            switch (databaseProvider)
            {
                case "MySql":
                    options.UseMySqlDatabase(connectionString, typeof(MySqlDataAssemblyMarker), Configuration);
                    break;
                case "PostgreSql":
                    options.UsePostgreSqlDatabase(connectionString, typeof(PostgreSqlDataAssemblyMarker), Configuration);
                    break;
                default:
                    options.UseSqlServerDatabase(connectionString, typeof(SqlServerDataAssemblyMarker), Configuration);
                    break;
            }
        });

        serviceCollection.AddSingleton<OpenAiProvider>();
        serviceCollection.AddSingleton<OpenAiTranslationService>();
    }

    public void PostInitialize(IApplicationBuilder appBuilder)
    {
        var serviceProvider = appBuilder.ApplicationServices;

        // Register settings
        var settingsRegistrar = serviceProvider.GetRequiredService<ISettingsRegistrar>();
        settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);

        var openAiTranslationService = serviceProvider.GetRequiredService<OpenAiTranslationService>();
        var importerRegistrar = appBuilder.ApplicationServices.GetService<IAiProviderRegistrar>();
        importerRegistrar.Register<OpenAiProvider>(() => appBuilder.ApplicationServices.GetService<OpenAiProvider>())
            .WithService(openAiTranslationService);

        var settingsManager = appBuilder.ApplicationServices.GetRequiredService<ISettingsManager>();
        CoreModuleConstants.Settings.General.AiHelperTranslationProvider.AllowedValues = CoreModuleConstants.Settings.General.AiHelperTranslationProvider.AllowedValues.Concat(importerRegistrar.GetAiProvidersByService<IAiTranslationService>().Select(x => x.ProviderType).ToArray()).Distinct().ToArray();

        // Apply migrations
        using var serviceScope = serviceProvider.CreateScope();
        using var dbContext = serviceScope.ServiceProvider.GetRequiredService<AiHelperOpenAiDbContext>();
        dbContext.Database.Migrate();
    }

    public void Uninstall()
    {
        // Nothing to do here
    }
}
