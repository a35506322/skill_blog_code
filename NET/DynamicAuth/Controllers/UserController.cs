namespace DynamicAuth.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private AuthContext _authContext;
    public UserController(AuthContext authContext)
    {
        _authContext = authContext;
    }

    [HttpPost]
    public async Task<IResult> Login([FromBody] LoginRequest request)
    {
        var user = await _authContext.User.SingleOrDefaultAsync(x => x.UserId == request.UserId & x.Password == request.Passwod);

        if (user is null)
            return TypedResults.Ok(ResultResponseFactory.LoginFail());

        return TypedResults.Ok(ResultResponseFactory.LoginSuccess("123"));
    }
}

