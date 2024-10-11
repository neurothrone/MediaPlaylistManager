using System.Text;
using System.Text.Json;
using MediaPlaylistManager.DAL.EFCore.Shared.Entities;
using MediaPlaylistManager.DAL.EFCore.Shared.Interfaces;

namespace MediaPlaylistManager.DAL.EFCore.WebApi.Repositories;

internal class CreatedMediaItemResponse
{
    public int Id { get; set; }
}

public class MediaItemWebApiRepository : BaseRepository, IMediaItemRepository
{
    public MediaItemWebApiRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public async Task<int> CreateMediaItemAsync(MediaItemEntity mediaItem)
    {
        HttpClient client = CreateHttpClient();

        string json = JsonSerializer.Serialize(mediaItem, SerializerOptions);
        StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        var uri = new Uri($"{WebApiConstants.BaseUrl}/media-items");
        var response = await client.PostAsync(uri, stringContent);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Error creating media item: {response.StatusCode}");

        var content = await response.Content.ReadAsStringAsync();
        var createdMediaItem = JsonSerializer.Deserialize<CreatedMediaItemResponse>(content, SerializerOptions) ??
                               throw new NullReferenceException("Could not deserialize media item");
        return createdMediaItem.Id;
    }

    public async Task<IReadOnlyCollection<MediaItemEntity>> GetMediaItemsByPlaylistIdAsync(int playlistId)
    {
        HttpClient client = CreateHttpClient();

        var uri = new Uri($"{WebApiConstants.BaseUrl}/media-items/playlist/{playlistId}");
        var response = await client.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
            return [];

        var content = await response.Content.ReadAsStringAsync();
        var mediaItems = JsonSerializer.Deserialize<IReadOnlyCollection<MediaItemEntity>>(
            content, SerializerOptions) ?? [];
        return mediaItems;
    }

    public async Task<MediaItemEntity?> GetMediaItemByIdAsync(int id)
    {
        HttpClient client = CreateHttpClient();

        var uri = new Uri($"{WebApiConstants.BaseUrl}/media-items/{id}");
        var response = await client.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
            return null;

        var content = await response.Content.ReadAsStringAsync();
        var mediaItem = JsonSerializer.Deserialize<MediaItemEntity>(content, SerializerOptions);
        return mediaItem;
    }

    public async Task<bool> UpdateMediaItemAsync(MediaItemEntity mediaItem)
    {
        HttpClient client = CreateHttpClient();

        string json = JsonSerializer.Serialize(mediaItem, SerializerOptions);
        StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        var uri = new Uri($"{WebApiConstants.BaseUrl}/media-items");
        var response = await client.PutAsync(uri, stringContent);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteMediaItemByIdAsync(int id)
    {
        HttpClient client = CreateHttpClient();

        var uri = new Uri($"{WebApiConstants.BaseUrl}/media-items/{id}");
        var response = await client.DeleteAsync(uri);
        return response.IsSuccessStatusCode;
    }

    public async Task<IReadOnlyCollection<MediaItemEntity>> SearchMediaItemsAsync(string query)
    {
        HttpClient client = CreateHttpClient();

        var uri = new Uri($"{WebApiConstants.BaseUrl}/media-items?query={query}");

        var response = await client.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
            return [];

        var content = await response.Content.ReadAsStringAsync();
        var mediaItems = JsonSerializer.Deserialize<IReadOnlyCollection<MediaItemEntity>>(
            content, SerializerOptions) ?? [];
        return mediaItems;
    }
}