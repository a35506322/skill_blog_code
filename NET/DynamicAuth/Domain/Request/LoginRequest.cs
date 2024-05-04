namespace DynamicAuth.Domain.Request;

public record LoginRequest(
    [Required(ErrorMessage = "帳號必填")] string UserId,
    [Required(ErrorMessage = "密碼必填")] string Passwod);
