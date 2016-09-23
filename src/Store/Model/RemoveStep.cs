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
using System.ComponentModel;
using System.Xml.Serialization;

namespace ZeroInstall.Store.Model
{
    /// <summary>
    /// Removes or moves a file or directory. It is an error if the path is outside the implementation.
    /// </summary>
    [Description("Removes or moves a file or directory. It is an error if the path is outside the implementation.")]
    [Serializable, XmlRoot("remove", Namespace = Feed.XmlNamespace), XmlType("remove", Namespace = Feed.XmlNamespace)]
    public sealed class RemoveStep : FeedElement, IRecipeStep, IEquatable<RemoveStep>, ICloneable
    {
        /// <summary>
        /// The file or directory to be removed relative to the implementation root as a Unix-style path.
        /// </summary>
        [Description("The file or directory to be removed relative to the implementation root as a Unix-style path.")]
        [XmlAttribute("path"), DefaultValue("")]
        public string Path { get; set; }

        #region Normalize
        /// <inheritdoc/>
        public void Normalize(FeedUri feedUri)
        {}
        #endregion

        #region Conversion
        /// <summary>
        /// Returns the remove step in the form "Path". Not safe for parsing!
        /// </summary>
        public override string ToString() => Path;
        #endregion

        #region Clone
        /// <summary>
        /// Creates a deep copy of this <see cref="RemoveStep"/> instance.
        /// </summary>
        /// <returns>The new copy of the <see cref="RemoveStep"/>.</returns>
        public IRecipeStep CloneRecipeStep() => new RemoveStep {UnknownAttributes = UnknownAttributes, UnknownElements = UnknownElements, IfZeroInstallVersion = IfZeroInstallVersion, Path = Path};

        object ICloneable.Clone() => CloneRecipeStep();
        #endregion

        #region Equality
        /// <inheritdoc/>
        public bool Equals(RemoveStep other)
        {
            if (other == null) return false;
            return base.Equals(other) && other.Path == Path;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            var step = obj as RemoveStep;
            return step != null && Equals(step);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (Path ?? "").GetHashCode();
            }
        }
        #endregion
    }
}
