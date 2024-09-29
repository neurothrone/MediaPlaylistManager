using System.Globalization;
using MediaPlaylistManager.PL.Maui.Shared.Controls;

namespace MediaPlaylistManager.PL.Maui.Shared.Converters;

public class ToolbarItemIconConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isEnabled && parameter is BindableToolbarItem toolbarItem)
        {
            return isEnabled ? toolbarItem.EnabledIcon : toolbarItem.DisabledIcon;
        }

        return null;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}