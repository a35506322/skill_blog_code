using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Demo.IValidatableObjec.Controllers;

[ApiController]
[Route("[controller]")]
public class DemoController : ControllerBase
{
    [HttpPost]
    public IResult Post([FromBody] LikeSportsRequest request) => Results.Ok(request);
}

public class LikeSportsRequest : IValidatableObject
{
    [Required]
    public Sport Sport { get; set; }
    public string? OtherSport { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Sport == Sport.Other && string.IsNullOrWhiteSpace(OtherSport))
        {
            yield return new ValidationResult("其他運動為必填", new[] { nameof(OtherSport) });
        }
    }
}

public enum Sport
{
    Football,
    Basketball,
    Baseball,
    Soccer,
    Other
}
