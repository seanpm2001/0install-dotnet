// Copyright Bastian Eicher et al.
// Licensed under the GNU Lesser Public License

using ZeroInstall.Model.Preferences;

namespace ZeroInstall.Commands.Basic;

/// <summary>
/// Register an additional source of implementations (versions) of a program.
/// </summary>
public class AddFeed(ICommandHandler handler) : AddRemoveFeedCommand(handler)
{
    public const string Name = "add-feed";
    public override string Description => Resources.DescriptionAddFeed;

    /// <inheritdoc/>
    protected override ExitCode ExecuteHelper(IEnumerable<FeedUri> interfaces, FeedReference source, Stability suggestedStabilityPolicy)
    {
        #region Sanity checks
        if (interfaces == null) throw new ArgumentNullException(nameof(interfaces));
        if (source == null) throw new ArgumentNullException(nameof(source));
        #endregion

        var modifiedInterfaces = new List<FeedUri>();
        foreach (var interfaceUri in interfaces)
        {
            var preferences = InterfacePreferences.LoadFor(interfaceUri);
            if (preferences.Feeds.AddIfNew(source))
                modifiedInterfaces.Add(interfaceUri);

            var effectiveStabilityPolicy = (preferences.StabilityPolicy == Stability.Unset)
                ? (Config.HelpWithTesting ? Stability.Testing : Stability.Stable)
                : preferences.StabilityPolicy;
            if (effectiveStabilityPolicy < suggestedStabilityPolicy)
            {
                string stabilityMessage = string.Format(Resources.StabilityPolicySuggested, source.Source.ToStringRfc(), suggestedStabilityPolicy);
                if (Handler.Ask(
                        stabilityMessage + Environment.NewLine + string.Format(Resources.StabilityPolicyAutoSet, interfaceUri.ToStringRfc()),
                        defaultAnswer: false, alternateMessage: stabilityMessage))
                    preferences.StabilityPolicy = suggestedStabilityPolicy;
            }
            preferences.SaveFor(interfaceUri);
        }

        if (modifiedInterfaces.Count == 0)
        {
            Handler.OutputLow(Resources.FeedManagement, Resources.FeedAlreadyRegistered);
            return ExitCode.NoChanges;
        }
        else
        {
            Handler.OutputLow(Resources.FeedManagement,
                Resources.FeedRegistered + Environment.NewLine +
                string.Join(Environment.NewLine, modifiedInterfaces.Select(x => x.ToStringRfc())));
            return ExitCode.OK;
        }
    }
}
