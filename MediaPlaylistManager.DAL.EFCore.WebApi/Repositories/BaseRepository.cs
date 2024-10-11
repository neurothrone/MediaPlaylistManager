using System.Text.Json;

namespace MediaPlaylistManager.DAL.EFCore.WebApi.Repositories;

public class BaseRepository
{
    private readonly IHttpClientFactory _httpClientFactory;

    protected static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    protected BaseRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    protected HttpClient CreateHttpClient() => _httpClientFactory.CreateClient(nameof(WebApiConstants.Name));
}