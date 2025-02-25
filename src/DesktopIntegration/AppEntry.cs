// Copyright Bastian Eicher et al.
// Licensed under the GNU Lesser Public License

using NanoByte.Common.Dispatch;
using ZeroInstall.DesktopIntegration.AccessPoints;

namespace ZeroInstall.DesktopIntegration;

/// <summary>
/// Represents an application in the <see cref="AppList"/> identified by its interface URI.
/// </summary>
[XmlType("app", Namespace = AppList.XmlNamespace)]
[Equatable]
public sealed partial class AppEntry : XmlUnknown, IMergeable<AppEntry>, ICloneable<AppEntry>
{
    /// <summary>
    /// The URI or local path of the interface defining the application or the pet-name if <see cref="Requirements"/> is set.
    /// </summary>
    [DisplayName("URI"), Description("The URI or local path of the interface defining the application or the pet-name if Requirements is set.")]
    [XmlIgnore]
    public required FeedUri InterfaceUri { get; set; }

    [IgnoreEquality]
    string IMergeable<AppEntry>.MergeID => InterfaceUri.ToStringRfc();

    /// <summary>
    /// The name of the application. Usually equal to <see cref="Feed.Name"/>.
    /// </summary>
    [Description("The name of the application. Usually equal to the Name specified in the Feed.")]
    [XmlAttribute("name")]
    public required string Name { get; set; }

    /// <summary>
    /// A set of requirements/restrictions imposed by the user on the implementation selection process.
    /// </summary>
    [Browsable(false)]
    [XmlIgnore]
    public Requirements? Requirements { get; set; }

    /// <summary>
    /// The <see cref="Requirements"/> if it is set, otherwise a basic reference to <see cref="InterfaceUri"/>.
    /// </summary>
    [Browsable(false), XmlIgnore, IgnoreEquality]
    public Requirements EffectiveRequirements => Requirements ?? InterfaceUri;

    #region XML serialization
    /// <summary>Used for XML serialization.</summary>
    /// <seealso cref="InterfaceUri"/>
    [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Used for XML serialization")]
    [XmlAttribute("interface"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never), IgnoreEquality]
    // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
    public string InterfaceUriString { get => InterfaceUri?.ToStringRfc()!; set => InterfaceUri = new(value); }

    /// <summary>Used for XML+JSON serialization.</summary>
    /// <seealso cref="Requirements"/>
    [XmlElement("requirements-json"), DefaultValue(""), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never), IgnoreEquality]
    public string? RequirementsJson { get => Requirements?.ToJsonString(); set => Requirements = value?.To(JsonStorage.FromJsonString<Requirements>); }
    #endregion

    /// <summary>
    /// A set of <see cref="Capability"/> lists to be registered in the desktop environment. Only compatible architectures are handled.
    /// </summary>
    [Browsable(false)]
    [XmlElement("capabilities", Namespace = CapabilityList.XmlNamespace)]
    [OrderedEquality]
    public List<CapabilityList> CapabilityLists { get; } = [];

    /// <summary>
    /// A set of <see cref="AccessPoints"/>s to be registered in the desktop environment. Is <c>null</c> if no desktop integration has been performed yet.
    /// </summary>
    [Description("A set of AccessPoints to be registered in the desktop environment. Is null if no desktop integration has been performed yet.")]
    [XmlElement("access-points")]
    public AccessPointList? AccessPoints { get; set; }

    /// <summary>
    /// Set to <c>true</c> to automatically download the newest available version of the application as a regular background task. Update checks will still be performed when the application is launched when set to <c>false</c>.
    /// </summary>
    [DisplayName("Auto-update"), Description("Set to true to automatically download the newest available version of the application as a regular background task. Update checks will still be performed when the application is launched when set to false.")]
    [XmlAttribute("auto-update"), DefaultValue(true)]
    public bool AutoUpdate { get; set; } = true;

    /// <summary>
    /// A regular expression a computer's hostname must match for this entry to be applied. Enables machine-specific entry filtering.
    /// </summary>
    [Description("A regular expression a computer's hostname must match for this entry to be applied. Enables machine-specific entry filtering.")]
    [XmlAttribute("hostname"), DefaultValue("")]
    public string? Hostname { get; set; }

    /// <inheritdoc/>
    [Browsable(false), XmlIgnore, IgnoreEquality]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// The time this entry was last modified encoded as Unix time (number of seconds since the epoch).
    /// </summary>
    /// <remarks>This value is ignored by clone and equality methods.</remarks>
    [Browsable(false), IgnoreEquality]
    [XmlAttribute("timestamp")]
    public long TimestampUnix { get => (UnixTime)Timestamp; set => Timestamp = (UnixTime)value; }

    /// <summary>
    /// Retrieves the first <see cref="Capability"/> that matches a specific type and ID and is compatible with <see cref="Architecture.CurrentSystem"/>.
    /// </summary>
    /// <typeparam name="T">The capability type to match.</typeparam>
    /// <param name="id">The <see cref="Capability.ID"/> to match.</param>
    /// <returns>The first matching <see cref="Capability"/>.</returns>
    /// <exception cref="KeyNotFoundException">No capability matching <paramref name="id"/> and <typeparamref name="T"/> was found.</exception>
    public T LookupCapability<T>(string id) where T : Capability
        => CapabilityLists.CompatibleCapabilities()
                          .OfType<T>()
                          .FirstOrDefault(specificCapability => specificCapability.ID == id)
        ?? throw new KeyNotFoundException(string.Format(Resources.UnableToFindTypeID, typeof(T).Name, id));

    #region Conversion
    /// <summary>
    /// Creates string representation suitable for console output.
    /// </summary>
    public override string ToString()
        => $"{InterfaceUri}: {Name} [{AccessPoints}]";
    #endregion

    #region Clone
    /// <summary>
    /// Creates a deep copy of this <see cref="AppEntry"/> instance.
    /// </summary>
    /// <returns>The new copy of the <see cref="AppEntry"/>.</returns>
    public AppEntry Clone() => new()
    {
        UnknownAttributes = UnknownAttributes,
        UnknownElements = UnknownElements,
        Name = Name,
        InterfaceUri = InterfaceUri,
        Requirements = Requirements?.Clone(),
        AccessPoints = AccessPoints?.Clone(),
        CapabilityLists = {CapabilityLists.CloneElements()}
    };
    #endregion
}
