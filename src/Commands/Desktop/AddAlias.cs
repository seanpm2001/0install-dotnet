// Copyright Bastian Eicher et al.
// Licensed under the GNU Lesser Public License

using ZeroInstall.Commands.Basic;
using ZeroInstall.DesktopIntegration.AccessPoints;

namespace ZeroInstall.Commands.Desktop;

/// <summary>
/// Create an alias for a <see cref="Run"/> command.
/// </summary>
public class AddAlias : AppCommand
{
    public const string Name = "alias";
    public const string AltName = "add-alias";
    public override string Usage => "ALIAS [INTERFACE [COMMAND]]";
    public override string Description => Resources.DescriptionAddAlias;
    protected override int AdditionalArgsMax => 3;

    private bool _resolve;
    private bool _remove;

    /// <inheritdoc/>
    public AddAlias(ICommandHandler handler)
        : base(handler)
    {
        Options.Add("no-download", () => Resources.OptionNoDownload, _ => NoDownload = true);

        Options.Add("resolve", () => Resources.OptionAliasResolve, _ => _resolve = true);
        Options.Add("remove", () => Resources.OptionAliasRemove, _ => _remove = true);
    }

    /// <inheritdoc />
    protected override ExitCode ExecuteHelper()
    {
        string aliasName = AdditionalArgs[0];

        if (_resolve || _remove)
        {
            if (AdditionalArgs.Count > 1) throw new OptionException(Resources.TooManyArguments + Environment.NewLine + AdditionalArgs[1].EscapeArgument(), null);

            var match = IntegrationManager.AppList.FindAppAlias(aliasName);
            if (!match.HasValue)
            {
                Handler.Output(Resources.AppAlias, string.Format(Resources.AliasNotFound, aliasName));
                return ExitCode.NoChanges;
            }
            var (alias, appEntry) = match.Value;

            if (_resolve)
            {
                string result = appEntry.InterfaceUri.ToStringRfc();
                if (!string.IsNullOrEmpty(alias.Command)) result += $"{Environment.NewLine}Command: {alias.Command}";
                Handler.OutputLow(Resources.AppAlias, result);
            }
            if (_remove)
            {
                IntegrationManager.RemoveAccessPoints(appEntry, new AccessPoint[] {alias});

                Handler.OutputLow(Resources.AppAlias, string.Format(Resources.AliasRemoved, aliasName, appEntry.Name));
            }
            return ExitCode.OK;
        }
        else
        {
            if (AdditionalArgs.Count < 2 || string.IsNullOrEmpty(AdditionalArgs[1])) throw new OptionException(Resources.MissingArguments, null);
            string? command = (AdditionalArgs.Count >= 3) ? AdditionalArgs[2] : null;

            var appEntry = GetAppEntry(IntegrationManager, ref InterfaceUri);
            CreateAlias(appEntry, aliasName, command);
            return ExitCode.OK;
        }
    }
}
