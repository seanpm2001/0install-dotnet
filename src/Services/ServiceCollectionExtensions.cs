// Copyright Bastian Eicher et al.
// Licensed under the GNU Lesser Public License

#if NETSTANDARD2_0
using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NanoByte.Common.Net;
using NanoByte.Common.Tasks;
using ZeroInstall.Services.Executors;
using ZeroInstall.Services.Feeds;
using ZeroInstall.Services.Fetchers;
using ZeroInstall.Services.PackageManagers;
using ZeroInstall.Services.Solvers;
using ZeroInstall.Store;
using ZeroInstall.Store.Feeds;
using ZeroInstall.Store.Implementations;
using ZeroInstall.Store.Trust;

namespace ZeroInstall.Services
{
    /// <summary>
    /// Provides extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    [PublicAPI, CLSCompliant(false)]
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a set of scoped services for using Zero Install functionality.
        /// </summary>
        /// <typeparam name="TTaskHandler">A callback object used when the the user needs to be asked questions or informed about download and IO tasks.</typeparam>
        /// <param name="services">The service collection to add the services to.</param>
        /// <param name="autoRefresh"><c>true</c> to wrap the default <see cref="ISolver"/> in a <see cref="RefreshingSolver"/>.</param>
        public static IServiceCollection AddZeroInstall<TTaskHandler>([NotNull] this IServiceCollection services, bool autoRefresh = false) where TTaskHandler : class, ITaskHandler
        {
            #region Sanity checks
            if (services == null) throw new ArgumentNullException(nameof(services));
            #endregion

            services
                .AddScoped<ITaskHandler, TTaskHandler>()
                .AddScoped(x => Config.Load())
                .AddScoped(x => StoreFactory.CreateDefault())
                .AddScoped<IOpenPgp, BouncyCastle>()
                .AddScoped(x => FeedCacheFactory.CreateDefault(x.GetService<IOpenPgp>()))
                .AddScoped(x => TrustDB.LoadSafe())
                .AddScoped<ITrustManager, TrustManager>()
                .AddScoped<IFeedManager, FeedManager>()
                .AddScoped<ICatalogManager, CatalogManager>()
                .AddScoped(x => PackageManagerFactory.Create())
                .AddScoped<ISelectionsManager, SelectionsManager>()
                .AddScoped<IFetcher, SequentialFetcher>()
                .AddScoped<IExecutor, Executor>()
                .AddScoped<ISelectionCandidateProvider, SelectionCandidateProvider>();

            if (autoRefresh)
            {
                services
                    .AddScoped<BacktrackingSolver>()
                    .AddScoped<ISolver>(x => new RefreshingSolver(x.GetService<BacktrackingSolver>(), x.GetService<IFeedManager>()));
            }
            else services.AddScoped<ISolver, BacktrackingSolver>();

            return services;
        }

        /// <summary>
        /// Registers a set of scoped services for using Zero Install functionality.
        /// Automatically uses <see cref="ILogger{TCategoryName}"/> and <see cref="ICredentialProvider"/> if registered in <paramref name="services"/>.
        /// </summary>
        /// <param name="services">The service collection to add the services to.</param>
        /// <param name="autoRefresh"><c>true</c> to wrap the default <see cref="ISolver"/> in a <see cref="RefreshingSolver"/>.</param>
        /// <seealso cref="ConfigurationCredentialProviderRegisration.ConfigureCredentials"/>
        public static IServiceCollection AddZeroInstall([NotNull] this IServiceCollection services, bool autoRefresh = false)
            => services.AddZeroInstall<ServiceTaskHandler>(autoRefresh);
    }
}
#endif
