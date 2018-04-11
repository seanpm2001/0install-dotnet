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
using System.IO;
using JetBrains.Annotations;
using ZeroInstall.Store.Model.Preferences;
using ZeroInstall.Store.Properties;

namespace ZeroInstall.Store.Model.Selection
{
    /// <summary>
    /// Represents an <see cref="Implementation"/> that is available to a solver for selection.
    /// </summary>
    public sealed class SelectionCandidate : IEquatable<SelectionCandidate>
    {
        /// <summary>
        /// The implementation this selection candidate references.
        /// </summary>
        [Browsable(false)]
        [NotNull]
        public Implementation Implementation { get; }

        /// <summary>
        /// The file name or URL of the feed listing the implementation.
        /// </summary>
        [NotNull]
        public FeedUri FeedUri { get; }

        /// <summary>
        /// The <see cref="FeedPreferences"/> for <see cref="FeedUri"/>.
        /// </summary>
        [Browsable(false)]
        [NotNull]
        public FeedPreferences FeedPreferences { get; }

        /// <summary>
        /// The version number of the implementation.
        /// </summary>
        [Description("The version number of the implementation.")]
        [NotNull]
        public ImplementationVersion Version => Implementation.Version;

        /// <summary>
        /// The date this implementation was made available. For development versions checked out from version control this attribute should not be present.
        /// </summary>
        [Description("The date this implementation was made available. For development versions checked out from version control this attribute should not be present.")]
        public DateTime Released => Implementation.Released;

        /// <summary>
        /// The default stability rating for this implementation.
        /// </summary>
        [Description("The default stability rating for this implementation.")]
        public Stability Stability => Implementation.Stability;

        /// <summary>The preferences controlling how the solver evaluates this candidate.</summary>
        [NotNull]
        private readonly ImplementationPreferences _implementationPreferences;

        /// <summary>
        /// A user-specified override for the <see cref="Stability"/> specified in the feed.
        /// </summary>
        [Description("A user-specified override for the stability specified in the feed.")]
        public Stability UserStability { get => _implementationPreferences.UserStability; set => _implementationPreferences.UserStability = value; }

        /// <summary>
        /// The <see cref="UserStability"/> if it is set, otherwise <see cref="Stability"/>.
        /// </summary>
        [Browsable(false)]
        public Stability EffectiveStability => (UserStability == Stability.Unset) ? Stability : UserStability;

        /// <summary>
        /// For platform-specific binaries, the platform for which an <see cref="Implementation"/> was compiled, in the form os-cpu. Either the os or cpu part may be *, which will make it available on any OS or CPU.
        /// </summary>
        [Description("For platform-specific binaries, the platform for which an implementation was compiled, in the form os-cpu. Either the os or cpu part may be *, which will make it available on any OS or CPU.")]
        public string Architecture => Implementation.Architecture.ToString();

        /// <summary>
        /// Human-readable notes about the implementation, e.g. "not suitable for this architecture".
        /// </summary>
        [Description("Human-readable notes about the implementation, e.g. \"not suitable for this architecture\".")]
        public string Notes { get; set; }

        /// <summary>
        /// Indicates wether this implementation fullfills all specified <see cref="Requirements"/>.
        /// </summary>
        [Browsable(false)]
        public bool IsSuitable { get; set; }

        /// <summary>
        /// Creates a new selection candidate.
        /// </summary>
        /// <param name="feedUri">The file name or URL of the feed listing the implementation.</param>
        /// <param name="feedPreferences">The <see cref="FeedPreferences"/> for <see cref="FeedUri"/>.</param>
        /// <param name="implementation">The implementation this selection candidate references.</param>
        /// <param name="requirements">A set of requirements/restrictions the <paramref name="implementation"/> needs to fullfill for <see cref="IsSuitable"/> to be <c>true</c>.</param>
        /// <param name="offlineUncached">Mark this candidate as unsuitable because it is uncached and <see cref="Config.NetworkUse"/> is set to <see cref="NetworkLevel.Offline"/>.</param>
        /// <exception cref="InvalidDataException"><paramref name="implementation"/>'s <see cref="ImplementationBase.ID"/> is empty.</exception>
        public SelectionCandidate([NotNull] FeedUri feedUri, [NotNull] FeedPreferences feedPreferences, [NotNull] Implementation implementation, [NotNull] Requirements requirements, bool offlineUncached = false)
        {
            FeedUri = feedUri ?? throw new ArgumentNullException(nameof(feedUri));
            FeedPreferences = feedPreferences ?? throw new ArgumentNullException(nameof(feedPreferences));
            Implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));

            if (string.IsNullOrEmpty(implementation.ID)) throw new InvalidDataException(string.Format(Resources.ImplementationMissingID, implementation, feedUri));

            _implementationPreferences = feedPreferences[implementation.ID];

            CheckSuitabilty(requirements ?? throw new ArgumentNullException(nameof(requirements)), offlineUncached);
        }

        private void CheckSuitabilty(Requirements requirements, bool offlineUncached)
        {
            if (Implementation.Architecture.Cpu == Cpu.Source && requirements.Architecture.Cpu != Cpu.Source)
                Notes = Resources.SelectionCandidateNoteSource;
            else if (!Implementation.Architecture.IsCompatible(requirements.Architecture))
                Notes = Resources.SelectionCandidateNoteIncompatibleArchitecture;
            else if (!Match(requirements, Version))
                Notes = Resources.SelectionCandidateNoteVersionMismatch;
            else if (EffectiveStability == Stability.Buggy)
                Notes = Resources.SelectionCandidateNoteBuggy;
            else if (EffectiveStability == Stability.Insecure)
                Notes = Resources.SelectionCandidateNoteInsecure;
            else if (!Implementation.ContainsCommand(requirements.Command))
                Notes = string.Format(Resources.SelectionCandidateNoteCommand, requirements.Command);
            else if (offlineUncached)
                Notes = Resources.SelectionCandidateNoteNotCached;
            else IsSuitable = true;
        }

        private static bool Match(Requirements requirements, ImplementationVersion version)
            => !requirements.ExtraRestrictions.TryGetValue(requirements.InterfaceUri, out var range) || range.Match(version);

        #region Conversion
        /// <summary>
        /// Returns the selection candidate in the form "SelectionCandidate: Implementation". Not safe for parsing!
        /// </summary>
        public override string ToString() => $"SelectionCandidate: {Implementation} from {FeedUri}";
        #endregion

        #region Equality
        /// <inheritdoc/>
        public bool Equals(SelectionCandidate other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Implementation, other.Implementation)
                && FeedUri.Equals(other.FeedUri)
                && IsSuitable == other.IsSuitable
                && Notes == other.Notes;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            return obj is SelectionCandidate candidate && Equals(candidate);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Implementation.GetHashCode();
                hashCode = (hashCode * 397) ^ FeedUri.GetHashCode();
                hashCode = (hashCode * 397) ^ IsSuitable.GetHashCode();
                if (Notes != null) hashCode = (hashCode * 397) ^ Notes.GetHashCode();
                return hashCode;
            }
        }
        #endregion
    }
}
