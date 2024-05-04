namespace DynamicAuth.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private AuthContext _authContext;
    private JWTHelper _jwtHelper;
    private IHttpContextAccessor _httpContextAccessor;

    public UserController(AuthContext authContext, JWTHelper jwtHelper, IHttpContextAccessor httpContextAccessor)
    {
        _authContext = authContext;
        _jwtHelper = jwtHelper;
        _httpContextAccessor = httpContextAccessor;
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

    [HttpGet]
    [Authorize]
    public async Task<IResult> GetUserClaims()
    {
        return TypedResults.Ok(ResultResponseFactory.QuerySuccess<object>
            (_httpContextAccessor.HttpContext.User.Claims.Select(p => new { p.Type, p.Value })));
    }
}

