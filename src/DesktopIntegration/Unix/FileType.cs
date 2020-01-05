// Copyright Bastian Eicher et al.
// Licensed under the GNU Lesser Public License

using System;
using System.IO;
using System.Net;
using ZeroInstall.Store;

namespace ZeroInstall.DesktopIntegration.Unix
{
    /// <summary>
    /// Contains control logic for applying <see cref="Store.Model.Capabilities.FileType"/> and <see cref="AccessPoints.FileType"/> on FreeDesktop.org systems.
    /// </summary>
    public static class FileType
    {
        #region Register
        /// <summary>
        /// Registers a file type in the current system.
        /// </summary>
        /// <param name="target">The application being integrated.</param>
        /// <param name="fileType">The file type to register.</param>
        /// <param name="machineWide">Register the file type machine-wide instead of just for the current user.</param>
        /// <param name="iconStore">Stores icon files downloaded from the web as local files.</param>
        /// <param name="accessPoint">Indicates that the file associations shall become default handlers for their respective types.</param>
        /// <exception cref="OperationCanceledException">The user canceled the task.</exception>
        /// <exception cref="IOException">A problem occurs while writing to the filesystem.</exception>
        /// <exception cref="WebException">A problem occurred while downloading additional data (such as icons).</exception>
        /// <exception cref="UnauthorizedAccessException">Write access to the filesystem is not permitted.</exception>
        /// <exception cref="InvalidDataException">The data in <paramref name="fileType"/> is invalid.</exception>
        public static void Register(FeedTarget target, Store.Model.Capabilities.FileType fileType, IIconStore iconStore, bool machineWide, bool accessPoint = false)
        {
            #region Sanity checks
            if (fileType == null) throw new ArgumentNullException(nameof(fileType));
            if (iconStore == null) throw new ArgumentNullException(nameof(iconStore));
            #endregion

            if (string.IsNullOrEmpty(fileType.ID)) throw new InvalidDataException("Missing ID");

            // TODO: Implement
        }
        #endregion

        #region Unregister
        /// <summary>
        /// Unregisters a file type in the current system.
        /// </summary>
        /// <param name="fileType">The file type to remove.</param>
        /// <param name="machineWide">Unregister the file type machine-wide instead of just for the current user.</param>
        /// <param name="accessPoint">Indicates that the file associations were default handlers for their respective types.</param>
        /// <exception cref="IOException">A problem occurs while writing to the filesystem.</exception>
        /// <exception cref="UnauthorizedAccessException">Write access to the filesystem is not permitted.</exception>
        /// <exception cref="InvalidDataException">The data in <paramref name="fileType"/> is invalid.</exception>
        public static void Unregister(Store.Model.Capabilities.FileType fileType, bool machineWide, bool accessPoint = false)
        {
            #region Sanity checks
            if (fileType == null) throw new ArgumentNullException(nameof(fileType));
            #endregion

            if (string.IsNullOrEmpty(fileType.ID)) throw new InvalidDataException("Missing ID");

            // TODO: Implement
        }
        #endregion
    }
}
