using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.AiHelperOpenAi.Core;

public static class ModuleConstants
{
    public static class Providers
    {
        public const string OpenAi = "OpenAI";
    }

    public static class Settings
    {
        public static class General
        {
            public static SettingDescriptor AiHelperOpenAiModel { get; } = new()
            {
                Name = "AiHelperOpenAi.Model",
                GroupName = "AiHelper|OpenAI",
                ValueType = SettingValueType.ShortText,
                AllowedValues = ["gpt-5", "gpt-5-mini", "gpt-5-nano"],
                DefaultValue = "gpt-5-nano",
            };

            public static SettingDescriptor AiHelperOpenAiPistureModel { get; } = new()
            {
                Name = "AiHelperOpenAi.PictureModel",
                GroupName = "AiHelper|OpenAI",
                ValueType = SettingValueType.ShortText,
                AllowedValues = ["dall-e-3"],
                DefaultValue = "dall-e-3",
            };

            public static SettingDescriptor AiHelperOpenAiKey { get; } = new()
            {
                Name = "AiHelperOpenAi.Key",
                GroupName = "AiHelper|OpenAI",
                ValueType = SettingValueType.SecureString,
            };

            public static SettingDescriptor AiHelperOpenAiPromptTranslate { get; } = new()
            {
                Name = "AiHelperOpenAi.PromptTranslate",
                GroupName = "AiHelper|Prompts - OpenAI",
                ValueType = SettingValueType.LongText,
                DefaultValue = "Translate to {locale} the text, preserve HTML or Markdown markups: {text}",
            };

            public static SettingDescriptor AiHelperOpenAiPromptDescriptionGeneration { get; } = new()
            {
                Name = "AiHelperOpenAi.PromptDescriptionGeneration",
                GroupName = "AiHelper|Prompts - OpenAI",
                ValueType = SettingValueType.LongText,
                DefaultValue = "Generate pretty seo-friendly description in {locale} language (maximum 1000 words, use only HTML tags if you need) for marketplace product: {product}",
            };

            public static SettingDescriptor AiHelperOpenAiPromptDescriptionGenerationByImage { get; } = new()
            {
                Name = "AiHelperOpenAi.PromptDescriptionGenerationByImage",
                GroupName = "AiHelper|Prompts - OpenAI",
                ValueType = SettingValueType.LongText,
                DefaultValue = "Generate pretty seo-friendly description in {locale} language (maximum 1000 words, use only HTML tags if you need) for marketplace product. Product name is {product.name}",
            };

            public static SettingDescriptor AiHelperOpenAiPromptFillPropertiesByImage { get; } = new()
            {
                Name = "AiHelperOpenAi.PromptFillPropertiesByImage",
                GroupName = "AiHelper|Prompts - OpenAI",
                ValueType = SettingValueType.LongText,
                DefaultValue = "Using proposed images you need fill the properties of product {product.name}. Look at json template and fill field 'value' in every paragraph. You should use only values from list 'availableValues' if it fill for the property, otherwise use the most suitable in you opinion. Any property may have more than one value from different images, if property has 'isMultivalue' you may fill multiple answer comma separated, otherwise single only. Return answer in json format with keys and values in order as template. Answer in English. Json template is: {jsonTemplate}",
            };

            public static IEnumerable<SettingDescriptor> AllGeneralSettings
            {
                get
                {
                    yield return AiHelperOpenAiModel;
                    yield return AiHelperOpenAiPistureModel;
                    yield return AiHelperOpenAiKey;
                    yield return AiHelperOpenAiPromptTranslate;
                    yield return AiHelperOpenAiPromptDescriptionGeneration;
                    yield return AiHelperOpenAiPromptDescriptionGenerationByImage;
                    yield return AiHelperOpenAiPromptFillPropertiesByImage;
                }
            }
        }

        public static IEnumerable<SettingDescriptor> AllSettings
        {
            get
            {
                return General.AllGeneralSettings;
            }
        }
    }
}
