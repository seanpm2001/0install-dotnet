// Copyright Bastian Eicher et al.
// Licensed under the GNU Lesser Public License

using NanoByte.Common.Net;
using ZeroInstall.Archives.Builders;
using ZeroInstall.Archives.Extractors;
using ZeroInstall.Services.Fetchers;
using ZeroInstall.Store.FileSystem;
using ZeroInstall.Store.Implementations;

namespace ZeroInstall.Commands.Basic;

partial class StoreMan
{
    public class Add : StoreSubCommand
    {
        public const string Name = "add";
        public override string Description => Resources.DescriptionStoreAdd;
        public override string Usage => "DIGEST (DIRECTORY | (ARCHIVE [EXTRACT [MIME-TYPE [...]]))";
        protected override int AdditionalArgsMin => 2;

        public Add(ICommandHandler handler)
            : base(handler)
        {}

        public override ExitCode Execute()
        {
            var manifestDigest = new ManifestDigest(AdditionalArgs[0]);
            if (Directory.Exists(AdditionalArgs[1]))
            {
                if (AdditionalArgs.Count > 2) throw new OptionException(Resources.TooManyArguments + Environment.NewLine + AdditionalArgs.Skip(2).JoinEscapeArguments(), null);
                return AddToStore(manifestDigest, FromDirectory);
            }
            return AddToStore(manifestDigest, FromArchives);
        }

        private void FromDirectory(IBuilder builder)
            => Handler.RunTask(new ReadDirectory(Path.GetFullPath(AdditionalArgs[1]), builder));

        private void FromArchives(IBuilder builder)
        {
            for (int i = 0; i < (AdditionalArgs.Count + 1) / 3; i++)
            {
                string path = AdditionalArgs[i * 3 + 1];
                string mimeType = (AdditionalArgs.Count > i * 3 + 3)
                    ? AdditionalArgs[i * 3 + 3]
                    : Archive.GuessMimeType(AdditionalArgs[i * 3 + 1]);
                string? subDir = (AdditionalArgs.Count > i * 3 + 2) ? AdditionalArgs[i * 3 + 2] : null;

                void Callback(Stream stream)
                    => ArchiveExtractor.For(mimeType, Handler)
                                       .Extract(builder, stream, subDir);

                Handler.RunTask(
                    Uri.TryCreate(path, UriKind.Absolute, out var uri) && !uri.IsFile
                        ? new DownloadFile(uri, Callback)
                        : new ReadFile(path, Callback));
            }
        }
    }

    public class Copy : StoreSubCommand
    {
        public const string Name = "copy";
        public override string Description => Resources.DescriptionStoreCopy;
        public override string Usage => "(DIRECTORY | URI | discover:DIGEST) [CACHE]";
        protected override int AdditionalArgsMin => 1;
        protected override int AdditionalArgsMax => 2;

        public Copy(ICommandHandler handler)
            : base(handler)
        {}

        public override ExitCode Execute()
        {
            SetStorePaths(AdditionalArgs.Skip(1).ToList());

            if (Uri.TryCreate(AdditionalArgs[0], UriKind.Absolute, out var uri) && !uri.IsFile)
            {
                var manifestDigest = new ManifestDigest(Path.GetFileNameWithoutExtension(uri.LocalPath));
                if (uri.Scheme == "discover") uri = DiscoverImplementation(manifestDigest);
                var extractor = ArchiveExtractor.For(Archive.GuessMimeType(uri.LocalPath), Handler);
                return AddToStore(manifestDigest, builder => Handler.RunTask(new DownloadFile(uri, stream => extractor.Extract(builder, stream))));
            }
            else
            {
                string path = Path.GetFullPath(AdditionalArgs[0]).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                var manifestDigest = new ManifestDigest(Path.GetFileName(path));
                return AddToStore(manifestDigest, builder => Handler.RunTask(new ReadDirectory(path, builder)));
            }
        }

        private Uri DiscoverImplementation(ManifestDigest manifestDigest)
            => Handler.RunTaskAndReturn(ResultTask.Create(Resources.DiscoveringImplementation, () =>
            {
                using var discovery = new ImplementationDiscovery();
                return discovery.GetImplementation(manifestDigest, Handler.CancellationToken);
            }));
    }

    public class Export : StoreSubCommand
    {
        public const string Name = "export";
        public override string Description => Resources.DescriptionStoreExport;
        public override string Usage => "DIGEST OUTPUT-ARCHIVE [MIME-TYPE]";
        protected override int AdditionalArgsMin => 2;
        protected override int AdditionalArgsMax => 3;

        public Export(ICommandHandler handler)
            : base(handler)
        {}

        public override ExitCode Execute()
        {
            var digest = new ManifestDigest(AdditionalArgs[0]);
            ArchiveBuilder.RunForDirectory(
                sourcePath: ImplementationStore.GetPath(digest) ?? throw new ImplementationNotFoundException(digest),
                archivePath: AdditionalArgs[1],
                mimeType: AdditionalArgs.Count == 3 ? AdditionalArgs[3] : Archive.GuessMimeType(AdditionalArgs[1]),
                Handler);

            return ExitCode.OK;
        }
    }

    public class Find : StoreSubCommand
    {
        public const string Name = "find";
        public override string Description => Resources.DescriptionStoreFind;
        public override string Usage => "DIGEST";
        protected override int AdditionalArgsMin => 1;
        protected override int AdditionalArgsMax => 1;

        public Find(ICommandHandler handler)
            : base(handler)
        {}

        public override ExitCode Execute()
        {
            var digest = new ManifestDigest(AdditionalArgs[0]);

            string path = ImplementationStore.GetPath(digest) ?? throw new ImplementationNotFoundException(digest);
            Handler.Output(string.Format(Resources.LocalPathOf, AdditionalArgs[0]), path);
            return ExitCode.OK;
        }
    }

    public class Remove : StoreSubCommand
    {
        public const string Name = "remove";
        public override string Description => Resources.DescriptionStoreRemove;
        public override string Usage => "DIGEST+";
        protected override int AdditionalArgsMin => 1;

        public Remove(ICommandHandler handler)
            : base(handler)
        {}

        public override ExitCode Execute()
        {
            bool removed = false;
            foreach (var digest in AdditionalArgs.Select(x => new ManifestDigest(x)))
                removed |= ImplementationStore.Remove(digest);
            return removed ? ExitCode.OK : ExitCode.NoChanges;
        }
    }

    public class Verify : StoreSubCommand
    {
        public const string Name = "verify";
        public override string Description => Resources.DescriptionStoreVerify;
        public override string Usage => "[DIRECTORY] DIGEST";
        protected override int AdditionalArgsMin => 1;
        protected override int AdditionalArgsMax => 2;

        public Verify(ICommandHandler handler)
            : base(handler)
        {}

        public override ExitCode Execute()
        {
            try
            {
                switch (AdditionalArgs.Count)
                {
                    case 1:
                        // Verify a directory inside the store
                        ImplementationStore.Verify(new ManifestDigest(AdditionalArgs[0]));
                        break;

                    case 2:
                        // Verify an arbitrary directory
                        ImplementationStoreUtils.Verify(AdditionalArgs[0], new ManifestDigest(AdditionalArgs[1]), Handler);
                        break;
                }
            }
            catch (DigestMismatchException ex)
            {
                Handler.Output(Resources.VerifyImplementation, ex.LongMessage);
                return ExitCode.DigestMismatch;
            }

            return ExitCode.OK;
        }
    }
}
