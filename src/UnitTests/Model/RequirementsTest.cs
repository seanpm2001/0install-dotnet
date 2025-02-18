// Copyright Bastian Eicher et al.
// Licensed under the GNU Lesser Public License

namespace ZeroInstall.Model;

/// <summary>
/// Contains test methods for <see cref="Requirements"/>.
/// </summary>
public class RequirementsTest
{
    /// <summary>
    /// Creates test <see cref="Requirements"/>.
    /// </summary>
    public static Requirements CreateTestRequirements() => new(FeedTest.Test1Uri, "command", new Architecture(OS.Windows, Cpu.I586))
    {
        //Languages = {"de-DE", "en-US"},
        ExtraRestrictions =
        {
            {FeedTest.Test1Uri, new VersionRange("1.0..!2.0")},
            {FeedTest.Test2Uri, new VersionRange("2.0..!3.0")}
        }
    };

    [Fact] // Ensures that the class can be correctly cloned.
    public void TestClone()
    {
        var requirements1 = CreateTestRequirements();
        requirements1.Languages.Add("fr");
        var requirements2 = requirements1.Clone();

        // Ensure data stayed the same
        requirements2.Should().Be(requirements1, because: "Cloned objects should be equal.");
        requirements2.GetHashCode().Should().Be(requirements1.GetHashCode(), because: "Cloned objects' hashes should be equal.");
        requirements2.Should().NotBeSameAs(requirements1, because: "Cloning should not return the same reference.");
    }

    [Fact] // Ensures that the class can be serialized to a command-line argument string
    public void TestToCommandLineArgs()
        => CreateTestRequirements()
          .ToCommandLineArgs()
          .Should().Equal("--command", "command", "--os", "Windows", "--cpu", "i586", "--version-for", "http://example.com/test1.xml", "1.0..!2.0", "--version-for", "http://example.com/test2.xml", "2.0..!3.0", "http://example.com/test1.xml");

    [Fact]
    public void Json()
        => CreateTestRequirements()
          .ToJsonString()
          .Should().Be("""{"interface":"http://example.com/test1.xml","command":"command","source":false,"os":"Windows","cpu":"i586","extra_restrictions":{"http://example.com/test1.xml":"1.0..!2.0","http://example.com/test2.xml":"2.0..!3.0"}}""");

    [Fact]
    public void Xml()
    {
        var requirements = new Requirements(FeedTest.Test1Uri, "command", new Architecture(OS.Windows, Cpu.I586));
        string xml = requirements.ToXmlString();
        XmlStorage.FromXmlString<Requirements>(xml).Should().Be(requirements);
    }
}
