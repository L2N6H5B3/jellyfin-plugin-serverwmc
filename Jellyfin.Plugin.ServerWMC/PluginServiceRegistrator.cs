using MediaBrowser.Controller;
using MediaBrowser.Controller.Channels;
using MediaBrowser.Controller.LiveTv;
using MediaBrowser.Controller.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace Jellyfin.Plugin.ServerWMC
{
    /// <summary>
    /// Registers the plugin's services with the Jellyfin server (required since 10.8, plugin types are no longer auto-discovered)
    /// </summary>
    public class PluginServiceRegistrator : IPluginServiceRegistrator
    {
        public void RegisterServices(IServiceCollection serviceCollection, IServerApplicationHost applicationHost)
        {
            // one WMCService instance shared by live tv and the recordings channel
            serviceCollection.AddSingleton<WMCService>();
            serviceCollection.AddSingleton<ILiveTvService>(sp => sp.GetRequiredService<WMCService>());
            serviceCollection.AddSingleton<IChannel, RecordingsChannel>();
        }
    }
}
