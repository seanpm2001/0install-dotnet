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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NanoByte.Common;
using NanoByte.Common.Storage;
using NanoByte.Common.Streams;
using Xunit;

namespace ZeroInstall.Store.Trust
{
    /// <summary>
    /// Contains common code for testing specific <see cref="IOpenPgp"/> implementations.
    /// </summary>
    public abstract class OpenPgpTest : IDisposable
    {
        private readonly TemporaryDirectory _homeDir = new TemporaryDirectory("0install-unit-test");
        private readonly IOpenPgp _sut;

        protected OpenPgpTest(IOpenPgp sut)
        {
            _sut = sut;
            _sut.HomeDir = _homeDir;
        }

        public void Dispose() => _homeDir?.Dispose();

        private readonly OpenPgpSecretKey _secretKey = new OpenPgpSecretKey(
            keyID: OpenPgpUtils.ParseKeyID("DEED44B49BE24661"),
            fingerprint: OpenPgpUtils.ParseFingerpint("E91FE1CBFCCF315543F6CB13DEED44B49BE24661"),
            userID: "Test User <test@0install.de>");

        private readonly byte[] _referenceData = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

        private readonly byte[] _signatureData = typeof(OpenPgpTest).GetEmbeddedBytes("signature.dat");

        [Fact]
        public void TestVerifyValidSignature()
        {
            TestImportKey();
            _sut.Verify(_referenceData, _signatureData).Should().Equal(
                new ValidSignature(_secretKey.KeyID, _secretKey.GetFingerprint(), new DateTime(2015, 7, 16, 17, 20, 7, DateTimeKind.Utc)));
        }

        [Fact]
        public void TestVerifyBadSignature()
        {
            TestImportKey();
            _sut.Verify(new byte[] {1, 2, 3}, _signatureData).Should().Equal(new BadSignature(_secretKey.KeyID));
        }

        [Fact]
        public void TestVerifyMissingKeySignature()
            => _sut.Verify(_referenceData, _signatureData).Should().Equal(new MissingKeySignature(_secretKey.KeyID));

        [Fact]
        public void TestVerifyInvalidData()
            => Assert.Throws<InvalidDataException>(() => _sut.Verify(new byte[] {1, 2, 3}, new byte[] {1, 2, 3}));

        [Fact]
        public void TestSign()
        {
            DeployKeyRings();

            var signatureData = _sut.Sign(_referenceData, _secretKey, "passphrase");
            signatureData.Length.Should().BeGreaterThan(10);

            TestImportKey();
            var signature = (ValidSignature)_sut.Verify(_referenceData, signatureData).Single();
            signature.GetFingerprint().Should().Equal(_secretKey.GetFingerprint());
        }

        [Fact]
        public void TestSignMissingKey()
            => Assert.Throws<KeyNotFoundException>(() => _sut.Sign(_referenceData, _secretKey));

        [Fact]
        public void TestSignWrongPassphrase()
        {
            DeployKeyRings();
            Assert.Throws<WrongPassphraseException>(() => _sut.Sign(_referenceData, _secretKey, "wrong-passphrase"));
        }

        [Fact]
        public void TestImportKey()
            => _sut.ImportKey(typeof(OpenPgpTest).GetEmbeddedBytes("pubkey.gpg"));

        [Fact]
        public void TestImportKeyInvalidData()
            => Assert.Throws<InvalidDataException>(() => _sut.ImportKey(new byte[] {1, 2, 3}));

        [Fact]
        public void TestExportKey()
        {
            DeployKeyRings();

            string exportedKey = _sut.ExportKey(_secretKey);
            string referenceKeyData = typeof(OpenPgpTest).GetEmbeddedString("pubkey.gpg")
                .GetRightPartAtFirstOccurrence("\n\n").GetLeftPartAtLastOccurrence("+");

            exportedKey.Should().StartWith("-----BEGIN PGP PUBLIC KEY BLOCK-----\n");
            exportedKey.Should().Contain(referenceKeyData);
            exportedKey.Should().EndWith("-----END PGP PUBLIC KEY BLOCK-----\n");
        }

        [Fact]
        public void TestExportKeyMissingKey()
            => Assert.Throws<KeyNotFoundException>(() => _sut.ExportKey(_secretKey));

        [Fact]
        public void TestListSecretKeys()
        {
            DeployKeyRings();
            _sut.ListSecretKeys().Should().Equal(_secretKey);
        }

        [Fact]
        public void TestGetSecretKey()
        {
            DeployKeyRings();

            _sut.GetSecretKey(_secretKey).Should().Be(_secretKey, because: "Should get secret key using parsed id source");

            _sut.GetSecretKey(_secretKey.UserID).Should().Be(_secretKey, because: "Should get secret key using user id");
            _sut.GetSecretKey(_secretKey.FormatKeyID()).Should().Be(_secretKey, because: "Should get secret key using key id string");
            _sut.GetSecretKey(_secretKey.FormatFingerprint()).Should().Be(_secretKey, because: "Should get secret key using fingerprint string");

            _sut.GetSecretKey().Should().Be(_secretKey, because: "Should get default secret key");

            Assert.Throws<KeyNotFoundException>(() => _sut.GetSecretKey("unknown@user.com"));
        }

        private void DeployKeyRings()
        {
            typeof(OpenPgpTest).CopyEmbeddedToFile("pubring.gpg", Path.Combine(_sut.HomeDir, "pubring.gpg"));
            typeof(OpenPgpTest).CopyEmbeddedToFile("secring.gpg", Path.Combine(_sut.HomeDir, "secring.gpg"));
        }
    }
}
