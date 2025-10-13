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

            public static SettingDescriptor AiHelperOpenAiKey { get; } = new()
            {
                Name = "AiHelperOpenAi.Key",
                GroupName = "AiHelper|OpenAI",
                ValueType = SettingValueType.SecureString,
            };

            public static SettingDescriptor AiHelperOpenAiPromptTranslate { get; } = new()
            {
                Name = "AiHelperOpenAi.PromptTranslate",
                GroupName = "AiHelper|Prompts",
                ValueType = SettingValueType.LongText,
                DefaultValue = "Translate to {locale} the text, preserve HTML or Markdown markups: {text}",
            };


            public static IEnumerable<SettingDescriptor> AllGeneralSettings
            {
                get
                {
                    yield return AiHelperOpenAiModel;
                    yield return AiHelperOpenAiKey;
                    yield return AiHelperOpenAiPromptTranslate;
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
