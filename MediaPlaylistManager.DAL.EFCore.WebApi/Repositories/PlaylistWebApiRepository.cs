using System.Text;
using System.Text.Json;
using MediaPlaylistManager.DAL.EFCore.Shared.Entities;
using MediaPlaylistManager.DAL.EFCore.Shared.Interfaces;

namespace MediaPlaylistManager.DAL.EFCore.WebApi.Repositories;

public class PlaylistWebApiRepository : BaseRepository, IPlaylistRepository
{
    private readonly JsonSerializerOptions _serializerOptions;

    public PlaylistWebApiRepository(
        IHttpClientFactory httpClientFactory,
        JsonSerializerOptions serializerOptions) : base(httpClientFactory)
    {
        _serializerOptions = serializerOptions;
    }

    public async Task<PlaylistEntity> CreatePlaylistAsync(string title)
    {
        HttpClient client = CreateHttpClient();
        
        var playlist = new PlaylistEntity { Title = title };
        string json = JsonSerializer.Serialize(playlist, _serializerOptions);
        StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        var uri = new Uri($"{WebApiConstants.BaseUrl}/playlists");
        var response = await client.PostAsync(uri, stringContent);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Error creating playlist: {response.StatusCode}");

        var content = await response.Content.ReadAsStringAsync();
        var createdPlaylist = JsonSerializer.Deserialize<PlaylistEntity>(content, _serializerOptions) ??
                              throw new NullReferenceException("Could not deserialize playlist");
        return createdPlaylist;
    }

    public async Task<IReadOnlyCollection<PlaylistEntity>> GetPlaylistsAsync()
    {
        HttpClient client = CreateHttpClient();
        
        var uri = new Uri($"{WebApiConstants.BaseUrl}/playlists");
        var response = await client.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
            return [];

        var content = await response.Content.ReadAsStringAsync();
        var playlists = JsonSerializer.Deserialize<IReadOnlyCollection<PlaylistEntity>>(
            content, _serializerOptions) ?? [];
        return playlists;
    }

    public async Task<PlaylistEntity?> GetPlaylistByIdAsync(int id)
    {
        HttpClient client = CreateHttpClient();
        
        var uri = new Uri($"{WebApiConstants.BaseUrl}/playlists/{id}");
        var response = await client.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
            return null;

        var content = await response.Content.ReadAsStringAsync();
        var playlist = JsonSerializer.Deserialize<PlaylistEntity>(content, _serializerOptions);
        return playlist;
    }

    public async Task<bool> UpdatePlaylistAsync(PlaylistEntity playlist)
    {
        HttpClient client = CreateHttpClient();
        
        string json = JsonSerializer.Serialize(playlist, _serializerOptions);
        StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        var uri = new Uri($"{WebApiConstants.BaseUrl}/playlists");
        var response = await client.PutAsync(uri, stringContent);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeletePlaylistByIdAsync(int id)
    {
        HttpClient client = CreateHttpClient();
        
        var uri = new Uri($"{WebApiConstants.BaseUrl}/playlists/{id}");
        var response = await client.DeleteAsync(uri);
        return response.IsSuccessStatusCode;
    }
}