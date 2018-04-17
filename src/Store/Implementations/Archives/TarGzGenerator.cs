// Copyright Bastian Eicher et al.
// Licensed under the GNU Lesser Public License

using System.IO;
using ICSharpCode.SharpZipLib.GZip;
using JetBrains.Annotations;

namespace ZeroInstall.Store.Implementations.Archives
{
    /// <summary>
    /// Creates a GZip-compressed TAR archive from a directory. Preserves execuable bits, symlinks, hardlinks and timestamps.
    /// </summary>
    public class TarGzGenerator : TarGenerator
    {
        internal TarGzGenerator([NotNull] string sourcePath, [NotNull] Stream stream)
            : base(sourcePath, new GZipOutputStream(stream))
        {}
    }
}
