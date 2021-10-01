// Copyright Bastian Eicher et al.
// Licensed under the GNU Lesser Public License

using FluentAssertions;
using Xunit;
using ZeroInstall.Model.Preferences;

namespace ZeroInstall.Model.Selection
{
    /// <summary>
    /// Contains test methods for <see cref="SelectionCandidate"/>.
    /// </summary>
    public class SelectionCandidateTest
    {
        [Fact]
        public void IsSuitable()
        {
            var implementation = ImplementationTest.CreateTestImplementation();
            var candidate = Build(implementation);
            candidate.IsSuitable.Should().BeTrue();
        }

        [Fact]
        public void IsSuitableArchitecture()
        {
            var implementation = ImplementationTest.CreateTestImplementation();
            var candidate = Build(implementation, requirements: new(FeedTest.Test1Uri, Command.NameRun, implementation.Architecture));
            candidate.IsSuitable.Should().BeTrue();
        }

        [Fact]
        public void IsNotSuitableArchitecture()
        {
            var implementation = ImplementationTest.CreateTestImplementation();
            var candidate = Build(implementation, requirements: new(FeedTest.Test1Uri, Command.NameRun, new Architecture(OS.FreeBsd, Cpu.Ppc)));
            candidate.IsSuitable.Should().BeFalse();
        }

        [Fact]
        public void IsSuitableVersion()
        {
            var implementation = ImplementationTest.CreateTestImplementation();
            var candidate = Build(implementation, requirements: new(FeedTest.Test1Uri, Command.NameRun)
            {
                ExtraRestrictions = {{FeedTest.Test1Uri, new VersionRange("..!1.1")}}
            });
            candidate.IsSuitable.Should().BeTrue();
        }

        [Fact]
        public void IsNotSuitableVersion()
        {
            var implementation = ImplementationTest.CreateTestImplementation();
            var candidate = Build(implementation, requirements: new(FeedTest.Test1Uri, Command.NameRun)
            {
                ExtraRestrictions = {{FeedTest.Test1Uri, new VersionRange("..!1.0")}}
            });
            candidate.IsSuitable.Should().BeFalse();
        }

        [Fact]
        public void IsNotSuitableBuggy()
        {
            var implementation = ImplementationTest.CreateTestImplementation();
            implementation.Stability = Stability.Buggy;
            var candidate = Build(implementation);
            candidate.IsSuitable.Should().BeFalse();
        }

        [Fact]
        public void IsNotSuitableInsecure()
        {
            var implementation = ImplementationTest.CreateTestImplementation();
            implementation.Stability = Stability.Insecure;
            var candidate = Build(implementation);
            candidate.IsSuitable.Should().BeFalse();
        }

        [Fact]
        public void UserStability()
        {
            var implementation = ImplementationTest.CreateTestImplementation();
            var preferences = new FeedPreferences
            {
                [implementation.ID] = {UserStability = Stability.Preferred}
            };
            var candidate = Build(implementation, preferences);
            candidate.EffectiveStability.Should().Be(Stability.Preferred);
        }

        private static SelectionCandidate Build(Implementation implementation, FeedPreferences? preferences = null, Requirements? requirements = null)
            => new(FeedTest.Test1Uri, preferences ?? new(), implementation, requirements ?? new(FeedTest.Test1Uri, Command.NameRun));
    }
}
