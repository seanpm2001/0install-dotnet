// Copyright Bastian Eicher et al.
// Licensed under the GNU Lesser Public License

namespace ZeroInstall.Store.Configuration;

/// <summary>
/// Contains test methods for <see cref="Config"/>.
/// </summary>
public class ConfigTest : TestWithRedirect
{
    /// <summary>
    /// Creates test <see cref="Config"/>.
    /// </summary>
    public static Config CreateTestConfig() => new()
    {
        HelpWithTesting = true,
        Freshness = TimeSpan.FromHours(12),
        NetworkUse = NetworkLevel.Minimal,
        AutoApproveKeys = false,
        SyncServerPassword = "pw123"
    };

    [Fact] // Ensures that the class can be correctly cloned.
    public void TestClone()
    {
        var config1 = CreateTestConfig();
        object config2 = config1.Clone();

        // Ensure data stayed the same
        config2.Should().Be(config1, because: "Cloned objects should be equal.");
        config2.GetHashCode().Should().Be(config1.GetHashCode(), because: "Cloned objects' hashes should be equal.");
        config2.Should().NotBeSameAs(config1, because: "Cloning should not return the same reference.");
    }

    /// <summary>
    /// Ensures that the class is correctly serialized and deserialized.
    /// </summary>
    [Fact]
    public void SaveLoad()
    {
        Config config1;
        object config2;
        using (var tempFile = new TemporaryFile("0install-test-config"))
        {
            // Write and read file
            config1 = CreateTestConfig();
            config1.Save(tempFile);
            config2 = Config.Load(tempFile);
        }

        // Ensure data stayed the same
        config2.Should().Be(config1, because: "Serialized objects should be equal.");
        config2.GetHashCode().Should().Be(config1.GetHashCode(), because: "Serialized objects' hashes should be equal.");
        config2.Should().NotBeSameAs(config1, because: "Serialized objects should not return the same reference.");
    }

    /// <summary>
    /// Ensures <see cref="Config.GetOption"/> and <see cref="Config.SetOption"/> properly access the settings properties.
    /// </summary>
    [Fact]
    public void GetSetValue()
    {
        var config = new Config();
        Assert.Throws<KeyNotFoundException>(() => config.SetOption("Test", "Test"));

        config.HelpWithTesting.Should().BeFalse();
        config.GetOption("help_with_testing").Should().Be("False");
        config.SetOption("help_with_testing", "True");
        Assert.Throws<FormatException>(() => config.SetOption("help_with_testing", "Test"));
        config.HelpWithTesting.Should().BeTrue();
        config.GetOption("help_with_testing").Should().Be("True");

        config.SetOption("freshness", "10");
        config.Freshness.Should().Be(TimeSpan.FromSeconds(10));
        config.GetOption("freshness").Should().Be("10");
    }

    /// <summary>
    /// Ensures <see cref="Config.Save(string)"/> preserves unknown properties loaded in <see cref="Config.Load(string)"/>.
    /// </summary>
    [Fact]
    public void RetainUnknownProperties()
    {
        string testIniData = "[global]" + Environment.NewLine + "test = test" + Environment.NewLine;

        using var tempFile = new TemporaryFile("0install-test-config");
        File.WriteAllText(tempFile, testIniData);
        Config.Load(tempFile).Save(tempFile);
        File.ReadAllText(tempFile).Should().Be(testIniData);
    }

    [Fact]
    public void LoadStressTest()
    {
        new Config().Save();

        StressTest.Run(() => _ = Config.Load());
    }
}
