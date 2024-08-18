using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ModelBindingEnumValuesError.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class DemoController : ControllerBase
{

    [HttpPost]
    public IActionResult DemoPostError([FromBody] DemoEnum demoEnum)
    {
        return Ok(demoEnum);
    }

    [HttpPost]
    public IActionResult DemoPostSuccess([FromBody][ValidEnumValue] DemoEnum demoEnum)
    {
        return Ok(demoEnum);
    }
}

public enum DemoEnum
{
    星期一 = 1,
    星期二 = 2,
    星期三 = 3,
    星期四 = 4,
    星期五 = 5,
    星期六 = 6,
    星期日 = 7
}

public class ValidEnumValueAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        Type enumType = value.GetType();
        bool valid = Enum.IsDefined(enumType, value);
        if (!valid)
        {
            return new ValidationResult(String.Format("{0} 不存在於 {1}", value, enumType.Name));
        }
        return ValidationResult.Success;
    }
}
