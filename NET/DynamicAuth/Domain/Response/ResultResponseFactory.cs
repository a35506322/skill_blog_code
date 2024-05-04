namespace DynamicAuth.Domain.Response;

public class ResultResponseFactory
{
    public static ResultResponse<string> LoginSuccess(string token) => new(Message: "登入成功", Info: token);
    public static ResultResponse LoginFail() => new(IsSuccess: false, Message: "登入失敗");
    public static ResultResponse<T> QuerySuccess<T>(T data) => new(Info: data);
}
