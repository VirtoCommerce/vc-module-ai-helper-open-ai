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
        serviceCollection.AddSingleton<OpenAiTextGenerationService>();
        serviceCollection.AddSingleton<OpenAiImageRecognitionService>();
        serviceCollection.AddSingleton<OpenAiImageGenerationService>();
    }

    public void PostInitialize(IApplicationBuilder appBuilder)
    {
        var serviceProvider = appBuilder.ApplicationServices;

        // Register settings
        var settingsRegistrar = serviceProvider.GetRequiredService<ISettingsRegistrar>();
        settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);

        var openAiTranslationService = serviceProvider.GetRequiredService<OpenAiTextGenerationService>();
        var openAiImageRecognitionService = serviceProvider.GetRequiredService<OpenAiImageRecognitionService>();
        var openAiImageGenerationService = serviceProvider.GetRequiredService<OpenAiImageGenerationService>();
        var aiProviderRegistrar = appBuilder.ApplicationServices.GetService<IAiProviderRegistrar>();
        aiProviderRegistrar.Register<OpenAiProvider>(() => appBuilder.ApplicationServices.GetService<OpenAiProvider>())
            .WithService(openAiTranslationService)
            .WithService(openAiImageRecognitionService)
            .WithService(openAiImageGenerationService);

        var settingsManager = appBuilder.ApplicationServices.GetRequiredService<ISettingsManager>();
        CoreModuleConstants.Settings.General.AiHelperTextGenerationProvider.AllowedValues =
            CoreModuleConstants.Settings.General.AiHelperTextGenerationProvider.AllowedValues
            .Concat(aiProviderRegistrar.GetAiProvidersByService<IAiTextGenerationService>().Select(x => x.ProviderType).ToArray()).Distinct().ToArray();
        CoreModuleConstants.Settings.General.AiHelperImageRecognitionProvider.AllowedValues =
            CoreModuleConstants.Settings.General.AiHelperImageRecognitionProvider.AllowedValues
            .Concat(aiProviderRegistrar.GetAiProvidersByService<IAiImageRecognitionService>().Select(x => x.ProviderType).ToArray()).Distinct().ToArray();
        CoreModuleConstants.Settings.General.AiHelperImageGenerationProvider.AllowedValues =
            CoreModuleConstants.Settings.General.AiHelperImageGenerationProvider.AllowedValues
            .Concat(aiProviderRegistrar.GetAiProvidersByService<IAiImageGenerationService>().Select(x => x.ProviderType).ToArray()).Distinct().ToArray();

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
