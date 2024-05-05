using NATS.Client.Internals.SimpleJSON;
using NBomber.CSharp;
using NBomber.Http;
using NBomber.Http.CSharp;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using NBomber.Contracts;
using System.Net.Http.Json;


using var httpClient = new HttpClient();

Http.GlobalJsonSerializerOptions = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};


string token = string.Empty;
var scenario = Scenario.Create("http_scenario", async context =>
{
    var step1 = await Step.Run("step_1", context, async () =>
    {
        var request =
            Http.CreateRequest("POST", "https://localhost:7039/api/Feature/AddFeature")
                .WithHeader("Authorization", $"Bearer {token}");

        var clientArgs = HttpClientArgs.Create(logger: context.Logger);

        var response = await Http.Send(httpClient, clientArgs, request);

        return response;
    });

    return Response.Ok();
})
.WithInit(async context =>
{
    using var client = new HttpClient();
    LoginRequest loginRequest = new LoginRequest("Admin", "=-09poiu");
    var request =
        Http.CreateRequest("POST", "https://localhost:7039/api/User/Login")
            .WithJsonBody(loginRequest);

    var response = await client.PostAsJsonAsync("https://localhost:7039/api/User/Login", loginRequest);
    var result = await response.Content.ReadFromJsonAsync<ResultResponse<string>>();
    token = result.Info;
}); ;

NBomberRunner
    .RegisterScenarios(scenario)
    .WithWorkerPlugins(new HttpMetricsPlugin(new[] { HttpVersion.Version1 }))
    // .WithWorkerPlugins(new HttpMetricsPlugin(new [] { HttpVersion.Version1, HttpVersion.Version2 }))
    .Run();

public record LoginRequest(string UserId, string Passwod);
public record ResultResponse<T>(bool IsSuccess = true, string Message = "", T Info = default);