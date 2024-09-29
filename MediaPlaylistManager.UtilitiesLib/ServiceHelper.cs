using Microsoft.Extensions.DependencyInjection;

namespace MediaPlaylistManager.UtilitiesLib;

// Source:
// https://blog.ewers-peters.de/are-you-using-dependency-injection-in-your-net-maui-app-yet

public static class ServiceHelper
{
    private static IServiceProvider? _services;

    public static void Initialize(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);

        if (_services != null)
            throw new InvalidOperationException("The service provider has already been set.");

        _services = serviceProvider;
    }

    public static T? GetService<T>()
    {
        if (_services is null)
            throw new InvalidOperationException("Service provider not initialized. Call Initialize() first.");

        return _services.GetService<T>();
    }

    public static T GetRequiredService<T>() where T : notnull
    {
        if (_services == null)
            throw new InvalidOperationException("Service provider not initialized. Call Initialize() first.");

        return _services.GetRequiredService<T>();
    }
}