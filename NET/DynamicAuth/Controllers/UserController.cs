using DynamicAuth.Infrastructures.JWT;

namespace DynamicAuth.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private AuthContext _authContext;
    private JWTHelper _jwtHelper;

    public UserController(AuthContext authContext, JWTHelper jwtHelper)
    {
        _authContext = authContext;
        _jwtHelper = jwtHelper;
    }

    [HttpPost]
    public async Task<IResult> Login([FromBody] LoginRequest request)
    {
        var user = await _authContext.User.SingleOrDefaultAsync(x => x.UserId == request.UserId & x.Password == request.Passwod);

        if (user is null)
            return TypedResults.Ok(ResultResponseFactory.LoginFail());

        var userRoles = await _authContext.User_Role.Where(x => x.UserId == user.UserId).ToListAsync();
        string token = _jwtHelper.GenerateToken(user.UserId, userRoles);

        return TypedResults.Ok(ResultResponseFactory.LoginSuccess(token));
    }
}

