using System.Text;
using System.Text.Json;
using MediaPlaylistManager.DAL.EFCore.Shared.Entities;
using MediaPlaylistManager.DAL.EFCore.Shared.Interfaces;

namespace MediaPlaylistManager.DAL.EFCore.WebApi.Repositories;

internal class CreatedMediaItemResponse
{
    public int Id { get; set; }
}

public class MediaItemWebApiRepository : IMediaItemRepository
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _serializerOptions;

    public MediaItemWebApiRepository(
        HttpClient client,
        JsonSerializerOptions serializerOptions)
    {
        _client = client;
        _serializerOptions = serializerOptions;
    }

    public async Task<int> CreateMediaItemAsync(MediaItemEntity mediaItem)
    {
        string json = JsonSerializer.Serialize(mediaItem, _serializerOptions);
        StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        var uri = new Uri($"{WebApiConstants.BaseUrl}/media-items");
        var response = await _client.PostAsync(uri, stringContent);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Error creating media item: {response.StatusCode}");

        var content = await response.Content.ReadAsStringAsync();
        var createdMediaItem = JsonSerializer.Deserialize<CreatedMediaItemResponse>(content, _serializerOptions) ??
                               throw new NullReferenceException("Could not deserialize media item");
        return createdMediaItem.Id;
    }

    public async Task<IReadOnlyCollection<MediaItemEntity>> GetMediaItemsByPlaylistIdAsync(int playlistId)
    {
        var uri = new Uri($"{WebApiConstants.BaseUrl}/media-items/playlist/{playlistId}");
        var response = await _client.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
            return [];

        var content = await response.Content.ReadAsStringAsync();
        var mediaItems = JsonSerializer.Deserialize<IReadOnlyCollection<MediaItemEntity>>(
            content, _serializerOptions) ?? [];
        return mediaItems;
    }

    public async Task<MediaItemEntity?> GetMediaItemByIdAsync(int id)
    {
        var uri = new Uri($"{WebApiConstants.BaseUrl}/media-items/{id}");
        var response = await _client.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
            return null;

        var content = await response.Content.ReadAsStringAsync();
        var mediaItem = JsonSerializer.Deserialize<MediaItemEntity>(content, _serializerOptions);
        return mediaItem;
    }

    public async Task<bool> UpdateMediaItemAsync(MediaItemEntity mediaItem)
    {
        string json = JsonSerializer.Serialize(mediaItem, _serializerOptions);
        StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        var uri = new Uri($"{WebApiConstants.BaseUrl}/media-items");
        var response = await _client.PutAsync(uri, stringContent);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteMediaItemByIdAsync(int id)
    {
        var uri = new Uri($"{WebApiConstants.BaseUrl}/media-items/{id}");
        var response = await _client.DeleteAsync(uri);
        return response.IsSuccessStatusCode;
    }

    public async Task<IReadOnlyCollection<MediaItemEntity>> SearchMediaItemsAsync(string query)
    {
        var uri = new Uri($"{WebApiConstants.BaseUrl}/media-items?query={query}");

        var response = await _client.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
            return [];

        var content = await response.Content.ReadAsStringAsync();
        var mediaItems = JsonSerializer.Deserialize<IReadOnlyCollection<MediaItemEntity>>(
            content, _serializerOptions) ?? [];
        return mediaItems;
    }
}