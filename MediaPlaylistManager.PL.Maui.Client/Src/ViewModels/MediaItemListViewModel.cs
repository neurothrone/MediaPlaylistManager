using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediaPlaylistManager.PL.Maui.Client.Enums;
using MediaPlaylistManager.PL.Maui.Client.Messages;
using MediaPlaylistManager.PL.Maui.Client.Utils;
using MediaPlaylistManager.SL.Interfaces;

namespace MediaPlaylistManager.PL.Maui.Client.ViewModels;

public abstract partial class MediaItemListViewModel :
    ObservableObject,
    IRecipient<MediaItemUpdatedMessage>,
    IRecipient<MediaItemDeletedMessage>
{
    protected readonly IMediaItemService MediaItemService;
    protected readonly INavigator Navigator;

    public ObservableCollection<MediaItemViewModel> MediaItems { get; } = [];

    protected MediaItemListViewModel(
        IMediaItemService mediaItemService,
        INavigator navigator)
    {
        MediaItemService = mediaItemService;
        Navigator = navigator;

        WeakReferenceMessenger.Default.Register<MediaItemUpdatedMessage>(this);
        WeakReferenceMessenger.Default.Register<MediaItemDeletedMessage>(this);
    }

    ~MediaItemListViewModel()
    {
        WeakReferenceMessenger.Default.Unregister<MediaItemUpdatedMessage>(this);
        WeakReferenceMessenger.Default.Unregister<MediaItemDeletedMessage>(this);
    }

    [RelayCommand]
    private void OpenMediaItemInPlayer(MediaItemViewModel mediaItem)
    {
        WeakReferenceMessenger.Default.Send(new MediaItemLoadedMessage(mediaItem));
    }

    [RelayCommand]
    private async Task NavigateToEditMediaItemPage(int mediaItemId)
    {
        await Navigator.GoToAsync($"{nameof(AppRoute.EditMediaItem)}?{nameof(MediaItemViewModel.Id)}={mediaItemId}");
    }

    [RelayCommand]
    private async Task DeleteMediaItem(MediaItemViewModel mediaItem)
    {
        if (!await MediaItemService.DeleteMediaItemAsync(mediaItem.Id))
            return;

        WeakReferenceMessenger.Default.Send(new MediaItemDeletedMessage(mediaItem));
    }

    private async Task UpdateMediaItemAsync(MediaItemViewModel mediaItem)
    {
        var mediaItemDto = mediaItem.ToMediaItemDto();
        if (!await MediaItemService.UpdateMediaItemAsync(mediaItemDto))
            return;

        var mediaItemToUpdate = MediaItems.FirstOrDefault(item => item.Id == mediaItemDto.Id);
        if (mediaItemToUpdate is null)
            return;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            mediaItemToUpdate.FilePath = mediaItemDto.FilePath;
            mediaItemToUpdate.Title = mediaItemDto.Title;
            mediaItemToUpdate.Artist = mediaItemDto.Artist;
            mediaItemToUpdate.Duration = mediaItemDto.Duration;
        });
    }

    private void RemoveMediaItem(MediaItemViewModel mediaItem)
    {
        var itemToRemove = MediaItems.FirstOrDefault(item => item.Id == mediaItem.Id);
        if (itemToRemove is null)
            return;

        MainThread.BeginInvokeOnMainThread(() => MediaItems.Remove(itemToRemove));
    }

    #region IRecipient<MediaItemUpdatedMessage>

    public async void Receive(MediaItemUpdatedMessage message)
    {
        await UpdateMediaItemAsync(message.MediaItem);
    }

    #endregion

    #region IRecipient<MediaItemDeletedMessage>

    public void Receive(MediaItemDeletedMessage message)
    {
        RemoveMediaItem(message.MediaItem);
    }

    #endregion
}