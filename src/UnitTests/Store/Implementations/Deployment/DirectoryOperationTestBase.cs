﻿/*
 * Copyright 2010-2016 Bastian Eicher
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser Public License for more details.
 *
 * You should have received a copy of the GNU Lesser Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.IO;
using NanoByte.Common.Storage;
using ZeroInstall.Store.Implementations.Manifests;

namespace ZeroInstall.Store.Implementations.Deployment
{
    /// <summary>
    /// Common base class for testing classes <see cref="DirectoryOperation"/> derived from.
    /// </summary>
    public abstract class DirectoryOperationTestBase : IDisposable
    {
        protected readonly TemporaryDirectory TempDir = new TemporaryDirectory("0install-unit-tests");
        public virtual void Dispose() => TempDir.Dispose();

        protected readonly string File1Path;
        protected readonly string SubdirPath;
        protected readonly string File2Path;
        protected readonly Manifest Manifest;

        protected DirectoryOperationTestBase()
        {
            File1Path = Path.Combine(TempDir, "file1");
            FileUtils.Touch(File1Path);

            SubdirPath = Path.Combine(TempDir, "subdir");
            Directory.CreateDirectory(SubdirPath);

            File2Path = Path.Combine(SubdirPath, "file2");
            FileUtils.Touch(File2Path);

            var generator = new ManifestGenerator(TempDir, ManifestFormat.Sha256New);
            generator.Run();
            Manifest = generator.Manifest;
        }
    }
}
