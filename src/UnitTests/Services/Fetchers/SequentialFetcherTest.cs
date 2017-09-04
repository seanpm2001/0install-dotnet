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
using NanoByte.Common.Net;
using Xunit;
using ZeroInstall.Store.Model;

namespace ZeroInstall.Services.Fetchers
{
    /// <summary>
    /// Runs test methods for <see cref="SequentialFetcher"/>
    /// </summary>
    public class SequentialFetcherTest : FetcherTest
    {
        protected override IFetcher BuildFetcher() => new SequentialFetcher(Config, StoreMock.Object, Handler);

        [Fact]
        public void DownloadSingleArchiveMirror()
        {
            StoreMock.Setup(x => x.Flush());
            using (var mirrorServer = new MicroServer("archive/http/invalid/directory%23archive.zip", ZipArchiveStream))
            {
                Config.FeedMirror = mirrorServer.ServerUri;
                TestDownloadArchives(
                    new Archive {Href = new Uri("http://invalid/directory/archive.zip"), MimeType = Archive.MimeTypeZip, Size = ZipArchiveStream.Length, Extract = "extract", Destination = "destination"});
            }
        }
    }
}
