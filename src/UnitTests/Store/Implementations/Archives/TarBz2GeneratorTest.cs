﻿/*
 * Copyright 2010-2015 Bastian Eicher
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

using System.IO;

namespace ZeroInstall.Store.Implementations.Archives
{
    /// <summary>
    /// Contains test methods for <see cref="TarBz2Generator"/>.
    /// </summary>
    public class TarBz2GeneratorTest : TarGeneratorTest
    {
        protected override TarGenerator CreateGenerator(string sourceDirectory, Stream stream)
        {
            return new TarBz2Generator(sourceDirectory, stream);
        }

        protected override Stream OpenArchive()
        {
            return TarBz2Extractor.GetDecompressionStream(base.OpenArchive());
        }
    }
}
