using Microsoft.AspNetCore.Authorization;

namespace DynamicAuth.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class FeatureController : ControllerBase
{
    [HttpGet]
    public async Task<IResult> QueryFeature() => TypedResults.Ok(ResultResponseFactory.QuerySuccess<string>("QueryFeature"));

    [HttpPost]
    public async Task<IResult> AddFeature() => TypedResults.Ok(ResultResponseFactory.QuerySuccess<string>("AddFeature"));

    [HttpPut]
    public async Task<IResult> EditFeature() => TypedResults.Ok(ResultResponseFactory.QuerySuccess<string>("EditFeature"));

    [HttpDelete]
    public async Task<IResult> DeleteFeature() => TypedResults.Ok(ResultResponseFactory.QuerySuccess<string>("DeleteFeature"));
}
