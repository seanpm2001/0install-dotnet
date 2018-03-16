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

using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
using Moq;
using NanoByte.Common.Native;
using NanoByte.Common.Storage;
using Xunit;
using ZeroInstall.Store.Feeds;
using ZeroInstall.Store.Trust;

namespace ZeroInstall.Store.Model.Preferences
{
    /// <summary>
    /// Contains test methods for <see cref="DiskFeedCache"/>.
    /// </summary>
    public class DiskFeedCacheTest : TestWithMocks
    {
        private readonly TemporaryDirectory _tempDir;
        private readonly DiskFeedCache _cache;
        private readonly Feed _feed1;

        public DiskFeedCacheTest()
        {
            // Create a temporary cache
            _tempDir = new TemporaryDirectory("0install-unit-tests");
            _cache = new DiskFeedCache(_tempDir, new Mock<IOpenPgp>().Object);

            // Add some dummy feeds to the cache
            _feed1 = FeedTest.CreateTestFeed();
            _feed1.Uri = FeedTest.Test1Uri;
            _feed1.SaveXml(Path.Combine(_tempDir, _feed1.Uri.Escape()));

            var feed2 = FeedTest.CreateTestFeed();
            feed2.Uri = FeedTest.Test2Uri;
            feed2.SaveXml(Path.Combine(_tempDir, feed2.Uri.Escape()));
            File.WriteAllText(Path.Combine(_tempDir, "http_invalid"), "");
        }

        public override void Dispose()
        {
            base.Dispose();
            _tempDir.Dispose();
        }

        [Fact]
        public void TestContains()
        {
            _cache.Contains(FeedTest.Test1Uri).Should().BeTrue();
            _cache.Contains(FeedTest.Test2Uri).Should().BeTrue();
            _cache.Contains(FeedTest.Test3Uri).Should().BeFalse();

            using (var localFeed = new TemporaryFile("0install-unit-tests"))
            {
                _feed1.SaveXml(localFeed);
                _cache.Contains(new FeedUri(localFeed))
                    .Should().BeTrue(because: "Should detect local feed files without them actually being in the cache");
            }

            using (var tempDir = new TemporaryDirectory("0install-unit-tests"))
            {
                _cache.Contains(new FeedUri(Path.Combine(tempDir, "feed.xml")))
                    .Should().BeFalse(because: "Should not detect phantom local feed files");
            }
        }

        [Fact]
        public void TestContainsCaseSensitive()
        {
            _cache.Contains(new FeedUri("http://0install.de/feeds/test/test1.xml")).Should().BeTrue();
            _cache.Contains(new FeedUri("http://0install.de/feeds/test/Test1.xml")).Should().BeFalse(because: "Should not be case-sensitive");
        }

        [Fact]
        public void TestListAll()
        {
            _cache.ListAll()
                .Should().Equal(FeedTest.Test1Uri, FeedTest.Test2Uri);
        }

        [Fact]
        public void TestGetFeed()
        {
            _cache.GetFeed(_feed1.Uri)
                .Should().Be(_feed1);
        }

        [Fact]
        public void TestGetFeedCaseSensitive()
        {
            _cache.Invoking(x => x.GetFeed(new FeedUri("http://0install.de/feeds/test/test1.xml"))).Should().NotThrow<KeyNotFoundException>();
            Assert.Throws<KeyNotFoundException>(() => _cache.GetFeed(new FeedUri("http://0install.de/feeds/test/Test1.xml")));
        }

        [Fact]
        public void TestGetSignatures()
        {
            _cache.GetSignatures(FeedTest.Test1Uri)
                .Should().BeEmpty();
        }

        [Fact]
        public void TestAdd()
        {
            var feed = FeedTest.CreateTestFeed();
            feed.Uri = FeedTest.Test3Uri;

            _cache.Add(feed.Uri, ToArray(feed));

            _cache.GetFeed(feed.Uri)
                .Should().Be(feed);
        }

        [Fact]
        public void TestRemove()
        {
            _cache.Contains(FeedTest.Test1Uri).Should().BeTrue();
            _cache.Remove(FeedTest.Test1Uri);
            _cache.Contains(FeedTest.Test1Uri).Should().BeFalse();
            _cache.Contains(FeedTest.Test2Uri).Should().BeTrue();
        }

        /// <summary>
        /// Ensures <see cref="DiskFeedCache"/> can handle feed URIs longer than the OSes maximum supported file path length.
        /// </summary>
        [SkippableFact(Skip = "Slow")]
        public void TestTooLongFilename()
        {
            Skip.IfNot(WindowsUtils.IsWindows, "Windows systems have a specific upper limit to file path lengths");

            var longHttpUrlBuilder = new StringBuilder(255);
            for (int i = 0; i < 255; i++)
                longHttpUrlBuilder.Append("x");

            var feed = FeedTest.CreateTestFeed();
            feed.Uri = new FeedUri("http://0install.de/feeds/test-" + longHttpUrlBuilder);

            _cache.Add(feed.Uri, ToArray(feed));

            feed.Normalize(feed.Uri);
            _cache.GetFeed(feed.Uri).Should().Be(feed);

            _cache.Contains(feed.Uri).Should().BeTrue();
            _cache.Remove(feed.Uri);
            _cache.Contains(feed.Uri).Should().BeFalse();
        }

        private static byte[] ToArray(Feed feed)
        {
            using (var stream = new MemoryStream())
            {
                feed.SaveXml(stream);
                return stream.ToArray();
            }
        }
    }
}
