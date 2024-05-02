using System.Text.Json.Serialization;
namespace HTTPClient.Infrastructures;

public class YouBikeResponse
{
    [JsonPropertyName("sno")]
    public string Sno { get; set; }

    [JsonPropertyName("sna")]
    public string Sna { get; set; }

    [JsonPropertyName("tot")]
    public int Tot { get; set; }

    [JsonPropertyName("sbi")]
    public int Sbi { get; set; }

    [JsonPropertyName("sarea")]
    public string Sarea { get; set; }

    [JsonPropertyName("mday")]
    public string Mday { get; set; }

    [JsonPropertyName("lat")]
    public float Lat { get; set; }

    [JsonPropertyName("lng")]
    public float Lng { get; set; }

    [JsonPropertyName("ar")]
    public string Ar { get; set; }

    [JsonPropertyName("sareaen")]
    public string Sareaen { get; set; }

    [JsonPropertyName("snaen")]
    public string Snaen { get; set; }

    [JsonPropertyName("aren")]
    public string Aren { get; set; }

    [JsonPropertyName("bemp")]
    public int Bemp { get; set; }

    [JsonPropertyName("act")]
    public string Act { get; set; }

    [JsonPropertyName("srcUpdateTime")]
    public string SrcUpdateTime { get; set; }

    [JsonPropertyName("updateTime")]
    public string UpdateTime { get; set; }

    [JsonPropertyName("infoTime")]
    public string InfoTime { get; set; }

    [JsonPropertyName("infoDate")]
    public string InfoDate { get; set; }
}

public class YouBikeHttpClientHandler
{
    private readonly HttpClient _httpClient;

    public YouBikeHttpClientHandler(IHttpClientFactory hf)
    {
        _httpClient = hf.CreateClient("youbike");
        _httpClient.BaseAddress = new Uri("https://tcgbusfs.blob.core.windows.net/");
    }

    public async Task<IEnumerable<YouBikeResponse>?> GetYouBikeAsync() =>
        await _httpClient.GetFromJsonAsync<IEnumerable<YouBikeResponse>>("dotapp/youbike/v2/youbike_immediate.json");
}

