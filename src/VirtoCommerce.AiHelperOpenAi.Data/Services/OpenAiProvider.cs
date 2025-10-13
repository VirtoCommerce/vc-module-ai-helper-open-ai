using VirtoCommerce.AiHelper.Core.Services;
using ModuleConstants = VirtoCommerce.AiHelperOpenAi.Core.ModuleConstants;

namespace VirtoCommerce.AiHelperOpenAi.Data.Services;
public class OpenAiProvider : AbstractAiProvider
{
    public override string ProviderName => ModuleConstants.Providers.OpenAi;
    public override string ProviderType => nameof(OpenAiProvider);

}
