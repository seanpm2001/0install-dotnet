// Copyright Bastian Eicher et al.
// Licensed under the GNU Lesser Public License

using NanoByte.Common.Native;
using ZeroInstall.Commands.Desktop;
using ZeroInstall.DesktopIntegration;
using ZeroInstall.Services;
using ZeroInstall.Services.Feeds;
using ZeroInstall.Store.Configuration;

namespace ZeroInstall.Commands;

/// <summary>
/// Common base class for Zero Install operations that require scoped dependency resolution.
/// </summary>
/// <param name="handler">A callback object used when the the user needs to be asked questions or informed about download and IO tasks.</param>
public abstract class ScopedOperation(ITaskHandler handler) : ServiceProvider(handler)
{
    /// <summary>
    /// Converts an interface or feed URI to its canonical representation.
    /// </summary>
    /// <exception cref="UriFormatException"><paramref name="uri"/> is an invalid interface URI.</exception>
    [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "This method handles a number of non-standard URI types which cannot be represented by the regular Uri class.")]
    public FeedUri GetCanonicalUri(string uri)
    {
        if (string.IsNullOrEmpty(uri)) throw new UriFormatException();

        try
        {
            if (uri.StartsWith("file://")) return new(uri);
            if (uri.StartsWith("file:/")) throw new UriFormatException(Resources.FilePrefixAbsoluteUsage);
            if (uri.StartsWith("file:", out string? path)) return new(Path.GetFullPath(path));
            if (uri.StartsWith("http:") || uri.StartsWith("https:")) return new(uri);

            if (TryResolveAlias(uri) is {} resolvedAlias) return resolvedAlias;

            if (Path.IsPathRooted(uri)) return new(uri);

            path = Path.GetFullPath(WindowsUtils.IsWindows ? Environment.ExpandEnvironmentVariables(uri) : uri);
            if (File.Exists(path)) return new(path);

            if (TryResolveCatalog(uri) is {} resolvedCatalog) return resolvedCatalog;

            return new(path);
        }
        #region Error handling
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or NotSupportedException)
        {
            throw new UriFormatException(string.Format(Resources.InvalidFeedUri, uri), ex);
        }
        #endregion
    }

    private static FeedUri? TryResolveAlias(string uri)
    {
        var appList = AppList.LoadSafe();

        const string aliasPrefix = "alias:";
        if (uri.StartsWith(aliasPrefix, out string? aliasName))
        {
            return appList.ResolveAlias(aliasName)
                ?? throw new UriFormatException(string.Format(Resources.AliasNotFound, aliasName));
        }
        else
        {
            if (appList.ResolveAlias(uri) is {} result)
            {
                Log.Info(string.Format(Resources.ResolvedUsingAlias, uri, result));
                return result;
            }
            return null;
        }
    }

    private FeedUri? TryResolveCatalog(string shortName)
    {
        if (FindByShortName(shortName) is {} feed)
        {
            Log.Info(string.Format(Resources.ResolvedUsingCatalog, shortName, feed.Uri));
            return feed.Uri;
        }
        else return null;
    }

    /// <summary>
    /// Returns a merged view of all <see cref="Catalog"/>s specified by the configuration files.
    /// </summary>
    /// <remarks>Handles caching based on <see cref="FeedManager.Refresh"/> flag.</remarks>
    /// <exception cref="WebException">Attempted to download catalog and failed.</exception>
    [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Performs network IO")]
    protected Catalog GetCatalog()
        => (FeedManager.Refresh ? null : CatalogManager.GetCached())
        ?? CatalogManager.GetOnlineSafe();

    /// <summary>
    /// Uses <see cref="Catalog.FindByShortName"/> to find a <see cref="Feed"/> matching a specific short name.
    /// </summary>
    /// <param name="shortName">The short name to look for. Must match either <see cref="Feed.Name"/> or <see cref="EntryPoint.BinaryName"/> of <see cref="Command.NameRun"/>.</param>
    /// <returns>The first matching <see cref="Feed"/>; <c>null</c> if no match was found.</returns>
    /// <remarks>Handles caching based on <see cref="FeedManager.Refresh"/> flag.</remarks>
    protected Feed? FindByShortName(string shortName)
    {
        #region Sanity checks
        if (string.IsNullOrEmpty(shortName)) throw new ArgumentNullException(nameof(shortName));
        #endregion

        Feed? GetCached() => CatalogManager.GetCachedSafe().FindByShortName(shortName);
        Feed? GetOnline() => CatalogManager.GetOnlineSafe().FindByShortName(shortName);

        if (FeedManager.Refresh)
            return GetOnline();
        else if (Config.EffectiveNetworkUse == NetworkLevel.Full)
            return GetCached() ?? GetOnline();
        else
            return GetCached();
    }

    /// <summary>
    /// Automatically updates Zero Install itself in a background process.
    /// </summary>
    /// <returns><c>true</c> if a background check was started; <c>false</c> if an update check was not due.</returns>
    protected bool BackgroundSelfUpdate()
    {
        if (ZeroInstallInstance.IsDeployed
         && !ZeroInstallInstance.IsMachineWide
         && Environment.UserInteractive
         && Config.EffectiveNetworkUse >= MinimumNetworkUseForBackgroundSelfUpdate
         && Config.SelfUpdateUri is {} uri
         && FeedManager.IsStale(uri)
         && !FeedManager.RateLimit(uri))
        {
            Log.Info("Starting periodic background self-update check");
            StartCommandBackground(Self.Name, Self.Update.Name, "--batch");
            return true;
        }
        else return false;
    }

    /// <summary>
    /// The minimum <see cref="Config.EffectiveNetworkUse"/> at which <see cref="BackgroundSelfUpdate"/> will consider an update check.
    /// </summary>
    protected virtual NetworkLevel MinimumNetworkUseForBackgroundSelfUpdate => NetworkLevel.Full;

    /// <summary>
    /// Starts executing a command in a background process. Returns immediately.
    /// </summary>
    /// <param name="command">The name of the command to execute.</param>
    /// <param name="args">Additional arguments to pass to the command.</param>
    protected static void StartCommandBackground(string command, params string[] args)
    {
        #region Sanity checks
        if (string.IsNullOrEmpty(command)) throw new ArgumentNullException(nameof(command));
        #endregion

        if (ProgramUtils.GuiStartInfo([command, "--background", ..args]) is {} startInfo)
        {
            try
            {
                startInfo.WorkingDirectory = Locations.InstallBase; // Avoid locking the user's working directory
                startInfo.Start();
            }
            #region Error handling
            catch (Exception ex) when (ex is OperationCanceledException or IOException)
            {}
            #endregion
        }
        else Log.Info("Skipping background command because there is no GUI subsystem available");
    }
}
