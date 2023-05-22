// Copyright Bastian Eicher et al.
// Licensed under the GNU Lesser Public License

using Microsoft.Win32.TaskScheduler;
using NanoByte.Common.Native;

namespace ZeroInstall.Commands.Desktop;

partial class SelfManager
{
    private const string
        TaskSchedulerFolder = "Zero Install",
        TaskSchedulerSelfUpdate = "Self update",
        TaskSchedulerUpdateApps = "Update apps";

    /// <summary>
    /// Adds scheduled tasks for automatic maintenance.
    /// </summary>
    /// <param name="libraryMode">Deploy Zero Install as a library for use by other applications without its own desktop integration.</param>
    private void TaskSchedulerApply(bool libraryMode)
    {
        if (!WindowsUtils.IsWindowsNT) return;

        Handler.RunTask(new ActionTask("Configuring Windows Task Scheduler", () =>
        {
            TaskSchedulerAddTask(TaskSchedulerSelfUpdate, Resources.DescriptionSelfUpdate,
                Self.Name, Self.Update.Name, "--batch");

            if (libraryMode)
                TaskSchedulerAddTask(TaskSchedulerUpdateApps, Resources.DescriptionUpdateApps,
                    UpdateApps.Name, "--batch", "--machine", "--clean");
            else
                TaskSchedulerRemoveTask(TaskSchedulerUpdateApps);
        }));
    }

    /// <summary>
    /// Removes scheduled tasks for automatic maintenance.
    /// </summary>
    private void TaskSchedulerRemove()
    {
        if (!WindowsUtils.IsWindowsNT) return;

        Handler.RunTask(new ActionTask("Configuring Windows Task Scheduler", () =>
        {
            TaskSchedulerRemoveTask(TaskSchedulerSelfUpdate);
            TaskSchedulerRemoveTask(TaskSchedulerUpdateApps);
            TaskSchedulerRemoveFolder();
        }));
    }

    private void TaskSchedulerAddTask(string name, string description, params string[] arguments)
    {
        string path = Path.Combine(TargetDir, "0install-win.exe");
        if (!File.Exists(path)) return;
        try
        {
            var task = TaskService.Instance.NewTask();
            task.RegistrationInfo.Description = description;
            task.Principal.LogonType = TaskLogonType.ServiceAccount;
            task.Principal.UserId = "SYSTEM";
            task.Actions.Add(new ExecAction(path, arguments.JoinEscapeArguments()));
            task.Settings.MaintenanceSettings.Period = TimeSpan.FromDays(7);
            task.Settings.MaintenanceSettings.Deadline = TimeSpan.FromDays(14);
            task.Settings.RunOnlyIfNetworkAvailable = true;
            task.Settings.IdleSettings.StopOnIdleEnd = false;
            task.Settings.DisallowStartIfOnBatteries = true;
            task.Settings.StopIfGoingOnBatteries = false;
            task.Settings.AllowHardTerminate = false;

            Log.Info($"Adding task '{TaskSchedulerFolder}\\{name}' to Windows Task Scheduler");
            TaskService.Instance
                       .RootFolder.CreateFolder(TaskSchedulerFolder, exceptionOnExists: false)
                       .RegisterTaskDefinition(name, task);
        }
        #region Error handling
        catch (Exception ex)
        {
            Log.Error($"Error adding task '{TaskSchedulerFolder}\\{name}' to Windows Task Scheduler", ex);
        }
        #endregion
    }

    private static void TaskSchedulerRemoveTask(string name)
    {
        try
        {
            var subFolders = TaskService.Instance.RootFolder.SubFolders;
            if (subFolders.Exists(TaskSchedulerFolder))
            {
                Log.Info($"Removing task '{TaskSchedulerFolder}\\{name}' from Windows Task Scheduler");
                subFolders[TaskSchedulerFolder].DeleteTask(name, exceptionOnNotExists: false);
            }
        }
        #region Error handling
        catch (Exception ex)
        {
            Log.Warn($"Error removing task '{TaskSchedulerFolder}\\{name}' from Windows Task Scheduler", ex);
        }
        #endregion
    }

    private static void TaskSchedulerRemoveFolder()
    {
        try
        {
            TaskService.Instance
                       .RootFolder.DeleteFolder(TaskSchedulerFolder, exceptionOnNotExists: false);
        }
        #region Error handling
        catch (Exception ex)
        {
            Log.Warn($"Error removing folder '{TaskSchedulerFolder}' from Windows Task Scheduler", ex);
        }
        #endregion
    }
}
