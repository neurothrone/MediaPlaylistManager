namespace MediaPlaylistManager.DAL.EFCore.WebApi.Repositories;

public class BaseRepository
{
    private readonly IHttpClientFactory _httpClientFactory;

    protected BaseRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    protected HttpClient CreateHttpClient() => _httpClientFactory.CreateClient(nameof(WebApiConstants.Name));
}