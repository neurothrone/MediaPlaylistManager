using System.Text;
using System.Text.Json;
using MediaPlaylistManager.DAL.EFCore.Shared.Entities;
using MediaPlaylistManager.DAL.EFCore.Shared.Interfaces;

namespace MediaPlaylistManager.DAL.EFCore.WebApi.Repositories;

public class PlaylistWebApiRepository : IPlaylistRepository
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _serializerOptions;

    public PlaylistWebApiRepository()
    {
        _client = new HttpClient();
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    public async Task<PlaylistEntity> CreatePlaylistAsync(string title)
    {
        var playlist = new PlaylistEntity { Title = title };
        string json = JsonSerializer.Serialize(playlist, _serializerOptions);
        StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        var uri = new Uri($"{WebApiConstants.BaseUrl}/playlists");
        var response = await _client.PostAsync(uri, stringContent);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Error creating playlist: {response.StatusCode}");

        var content = await response.Content.ReadAsStringAsync();
        var createdPlaylist = JsonSerializer.Deserialize<PlaylistEntity>(content, _serializerOptions) ??
                              throw new NullReferenceException("Could not deserialize playlist");
        return createdPlaylist;
    }

    public async Task<List<PlaylistEntity>> GetPlaylistsAsync()
    {
        var uri = new Uri($"{WebApiConstants.BaseUrl}/playlists");
        var response = await _client.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
            return [];

        var content = await response.Content.ReadAsStringAsync();
        var playlists = JsonSerializer.Deserialize<List<PlaylistEntity>>(content, _serializerOptions) ?? [];
        return playlists;
    }

    public async Task<PlaylistEntity?> GetPlaylistByIdAsync(int id)
    {
        var uri = new Uri($"{WebApiConstants.BaseUrl}/playlists/{id}");
        var response = await _client.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
            return null;

        var content = await response.Content.ReadAsStringAsync();
        var playlist = JsonSerializer.Deserialize<PlaylistEntity>(content, _serializerOptions);
        return playlist;
    }

    public async Task<bool> UpdatePlaylistAsync(PlaylistEntity playlist)
    {
        string json = JsonSerializer.Serialize(playlist, _serializerOptions);
        StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        var uri = new Uri($"{WebApiConstants.BaseUrl}/playlists");
        var response = await _client.PutAsync(uri, stringContent);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeletePlaylistByIdAsync(int id)
    {
        var uri = new Uri($"{WebApiConstants.BaseUrl}/playlists/{id}");
        var response = await _client.DeleteAsync(uri);
        return response.IsSuccessStatusCode;
    }
}