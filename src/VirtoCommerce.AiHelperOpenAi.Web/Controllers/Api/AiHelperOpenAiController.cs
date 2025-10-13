using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VirtoCommerce.AiHelperOpenAi.Web.Controllers.Api;

[Authorize]
[Route("api/ai-helper-open-ai")]
public class AiHelperOpenAiController : Controller
{
    // GET: api/ai-helper-open-ai
    /// <summary>
    /// Get message
    /// </summary>
    /// <remarks>Return "Hello world!" message</remarks>
    [HttpGet]
    [Route("")]
    public ActionResult<string> Get()
    {
        return Ok(new { result = "Hello world!" });
    }
}
