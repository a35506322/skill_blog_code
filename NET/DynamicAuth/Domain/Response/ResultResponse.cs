namespace DynamicAuth.Domain.Response;

public record ResultResponse<T>(bool IsSuccess = true, string Message = "", T Info = default);
public record ResultResponse(bool IsSuccess = true, string Message = "", object Info = null);

