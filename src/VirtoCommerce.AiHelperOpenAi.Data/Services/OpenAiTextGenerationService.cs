using System.Threading.Tasks;
using OpenAI.Responses;
using VirtoCommerce.AiHelper.Core.Models;
using VirtoCommerce.AiHelper.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using static VirtoCommerce.AiHelperOpenAi.Core.ModuleConstants;

namespace VirtoCommerce.AiHelperOpenAi.Data.Services;
public class OpenAiTextGenerationService : IAiTextGenerationService
{
    private readonly ISettingsManager _settingsManager;

    public OpenAiTextGenerationService(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
    }

    public virtual async Task<AiRequestResult> GenerateTextAsync(string prompt, string context = null)
    {
        var result = AbstractTypeFactory<AiRequestResult>.TryCreateInstance();

        var apiKey = await _settingsManager.GetValueAsync<string>(Settings.General.AiHelperOpenAiKey);
        var model = await _settingsManager.GetValueAsync<string>(Settings.General.AiHelperOpenAiModel);

        if (!string.IsNullOrEmpty(apiKey) && !string.IsNullOrEmpty(model) && !string.IsNullOrEmpty(prompt))
        {
#pragma warning disable OPENAI001
            var client = new OpenAIResponseClient(model: model, apiKey: apiKey);
            OpenAIResponse response = client.CreateResponse(prompt);
#pragma warning restore OPENAI001

            result.Result = response.GetOutputText();
            result.IsSuccess = true;
        }
        else
        {
            result.ErrorMessage = "AI provider is not configured properly.";
        }

        return result;
    }

    public virtual Task<string> GetTranslationPrompt()
    {
        return _settingsManager.GetValueAsync<string>(Settings.General.AiHelperOpenAiPromptTranslate);
    }

    public virtual Task<string> GetProductDescriptionGenerationPrompt()
    {
        return _settingsManager.GetValueAsync<string>(Settings.General.AiHelperOpenAiPromptDescriptionGeneration);
    }
}
