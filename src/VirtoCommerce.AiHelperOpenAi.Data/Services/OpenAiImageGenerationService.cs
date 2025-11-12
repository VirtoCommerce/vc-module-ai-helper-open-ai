using System.Threading.Tasks;
using OpenAI.Images;
using VirtoCommerce.AiHelper.Core.Models;
using VirtoCommerce.AiHelper.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using static VirtoCommerce.AiHelperOpenAi.Core.ModuleConstants;

namespace VirtoCommerce.AiHelperOpenAi.Data.Services;
public class OpenAiImageGenerationService : IAiImageGenerationService
{
    private readonly ISettingsManager _settingsManager;

    public OpenAiImageGenerationService(
        ISettingsManager settingsManager
        )
    {
        _settingsManager = settingsManager;
    }

    public virtual async Task<AiRequestResult> GenerateImageAsync(string prompt, int width = 0, int height = 0)
    {
        var result = AbstractTypeFactory<AiRequestResult>.TryCreateInstance();

        var apiKey = await _settingsManager.GetValueAsync<string>(Settings.General.AiHelperOpenAiKey);
        var model = await _settingsManager.GetValueAsync<string>(Settings.General.AiHelperOpenAiPistureModel);

        if (!string.IsNullOrEmpty(apiKey) && !string.IsNullOrEmpty(model) && !string.IsNullOrEmpty(prompt))
        {
            var imageClient = new ImageClient(model, apiKey);
            var options = new ImageGenerationOptions
            {
                Size = GeneratedImageSize.W1024xH1024,  // Варианты: W256xH256, W512xH512, W1024xH1024 и т.д.
                Quality = GeneratedImageQuality.Standard,  // Standard или High (для DALL-E 3)
                Style = GeneratedImageStyle.Natural,  // Natural или Vivid (для DALL-E 3)
                ResponseFormat = GeneratedImageFormat.Uri  // Url или Bytes (для прямого получения байтов)
            };

            var response = await imageClient.GenerateImageAsync(prompt, options);

            if (response.Value != null)
            {
                var item = response.Value;

                result.Result = item.ImageUri;//.ImageBytes.ToArray();
                result.IsSuccess = true;
            }
            else
            {
                result.ErrorMessage = "No image generated.";
            }
        }
        else
        {
            result.ErrorMessage = "AI provider is not configured properly.";
        }

        return result;
    }
}
