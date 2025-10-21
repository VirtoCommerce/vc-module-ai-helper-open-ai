using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenAI.Responses;
using VirtoCommerce.AiHelper.Core.Models;
using VirtoCommerce.AiHelper.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using static VirtoCommerce.AiHelperOpenAi.Core.ModuleConstants;

namespace VirtoCommerce.AiHelperOpenAi.Data.Services;
public class OpenAiImageRecognitionService : IAiImageRecognitionService
{
    private readonly ISettingsManager _settingsManager;

    public OpenAiImageRecognitionService(
        ISettingsManager settingsManager
        )
    {
        _settingsManager = settingsManager;
    }

    public virtual async Task<AiRequestResult> RecognizeImageAsync(string prompt, string[] images, string context = null)
    {
        var result = AbstractTypeFactory<AiRequestResult>.TryCreateInstance();

        var apiKey = await _settingsManager.GetValueAsync<string>(Settings.General.AiHelperOpenAiKey);
        var model = await _settingsManager.GetValueAsync<string>(Settings.General.AiHelperOpenAiModel);

        if (!string.IsNullOrEmpty(apiKey) && !string.IsNullOrEmpty(model) && !string.IsNullOrEmpty(prompt))
        {
#pragma warning disable OPENAI001
            OpenAIResponseClient client = new(model: model, apiKey: apiKey);

            List<ResponseContentPart> responseContentParts = [
                ResponseContentPart.CreateInputTextPart(prompt)
                ];

            if (images != null && images.Any())
            {
                foreach (var image in images)
                {
                    if (IsUrl(image))
                    {
                        Uri imgUrl = new(image);
                        var imagePart = ResponseContentPart.CreateInputImagePart(imgUrl, ResponseImageDetailLevel.Low);
                        responseContentParts.Add(imagePart);
                    }
                    else
                    {
                        byte[] imageBytes = Convert.FromBase64String(image);
                        using MemoryStream ms = new(imageBytes);
                        var imagePart = ResponseContentPart.CreateInputImagePart(BinaryData.FromStream(ms), "image/jpg", ResponseImageDetailLevel.Low);
                        responseContentParts.Add(imagePart);
                    }
                }
            }
            else
            {
                result.ErrorMessage = "Image data is not provided.";
                return result;
            }

            OpenAIResponse response = (OpenAIResponse)client.CreateResponse([
                ResponseItem.CreateUserMessageItem(responseContentParts)
            ]);
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

    private bool IsUrl(string s)
    {
        s = s.Trim();
        return Regex.IsMatch(s, @"^https?://(?:www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b(?:[-a-zA-Z0-9()@:%_\+.~#?&//=]*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }

    //private string GetImageFormat(string imagePath)
    //{
    //    var extension = Path.GetExtension(imagePath).ToLower();
    //    return extension switch
    //    {
    //        ".jpg" or ".jpeg" => "image/jpg",
    //        ".png" => "image/png",
    //        ".gif" => "image/gif",
    //        ".webp" => "image/webp",
    //        _ => "image/jpg"
    //    };
    //}
}
