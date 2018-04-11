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

using System.Diagnostics;
using System.IO;
using FluentAssertions;
using Moq;
using NanoByte.Common;
using NanoByte.Common.Collections;
using NanoByte.Common.Native;
using NanoByte.Common.Storage;
using Xunit;
using ZeroInstall.Store;
using ZeroInstall.Store.Implementations;
using ZeroInstall.Store.Model;
using ZeroInstall.Store.Model.Selection;

namespace ZeroInstall.Services.Executors
{
    /// <summary>
    /// Contains test methods for <see cref="EnvironmentBuilder"/>.
    /// </summary>
    public class EnvironmentBuilderTest : TestWithMocksAndRedirect
    {
        private const string Test1Path = "test1 path", Test2Path = "test2 path";

        [Fact]
        public void TestExceptions()
        {
            var executor = new EnvironmentBuilder(new Mock<IStore>(MockBehavior.Loose).Object);
            executor.Invoking(x => x.Inject(new Selections {Command = Command.NameRun}))
                .Should().Throw<ExecutorException>(because: "Selections with no implementations should be rejected");
            executor.Invoking(x => x.Inject(new Selections {Implementations = {new ImplementationSelection()}}))
                .Should().Throw<ExecutorException>(because: "Selections with no start command should be rejected");
        }

        [Fact]
        public void TestExceptionMultipleInvalidBindings()
        {
            var selections = SelectionsTest.CreateTestSelections();
            selections.Implementations[1].Commands[0].Bindings.Add(new EnvironmentBinding()); // Missing name
            ExpectCommandException(selections);

            selections = SelectionsTest.CreateTestSelections();
            selections.Implementations[1].Commands[0].Bindings.Add(new EnvironmentBinding {Name = "test", Insert = "test1", Value = "test2"}); // Conflicting insert and value
            ExpectCommandException(selections);

            selections = SelectionsTest.CreateTestSelections();
            selections.Implementations[1].Commands[0].Bindings.Add(new ExecutableInVar()); // Missing name
            ExpectCommandException(selections);

            selections = SelectionsTest.CreateTestSelections();
            selections.Implementations[1].Commands[0].Bindings.Add(new ExecutableInPath()); // Missing name
            ExpectCommandException(selections);
        }

        [Fact]
        public void TestExceptionMultipleWorkingDirs()
        {
            var selections = SelectionsTest.CreateTestSelections();
            selections.Implementations[1].Commands[0].WorkingDir = new WorkingDir();
            ExpectCommandException(selections);
        }

        private static void ExpectCommandException(Selections selections)
        {
            var storeMock = new Mock<IStore>(MockBehavior.Loose);
            storeMock.Setup(x => x.GetPath(It.IsAny<ManifestDigest>())).Returns("test path");
            var executor = new EnvironmentBuilder(storeMock.Object);
            executor.Invoking(x => x.Inject(selections))
                .Should().Throw<ExecutorException>(because: "Invalid Selections should be rejected");
        }

        private static IStore GetMockStore(Selections selections)
        {
            var storeMock = new Mock<IStore>(MockBehavior.Loose);
            storeMock.Setup(x => x.GetPath(selections.Implementations[1].ManifestDigest)).Returns(Test1Path);
            storeMock.Setup(x => x.GetPath(selections.Implementations[2].ManifestDigest)).Returns(Test2Path);
            return storeMock.Object;
        }

        private static void VerifyEnvironment(ProcessStartInfo startInfo, Selections selections)
        {
            startInfo.EnvironmentVariables["TEST1_PATH_SELF"].Should().Be("default" + Path.PathSeparator + Test1Path, because: "Should append implementation path");
            startInfo.EnvironmentVariables["TEST1_VALUE"].Should().Be("test1", because: "Should directly set value");
            startInfo.EnvironmentVariables["TEST1_EMPTY"].Should().Be("", because: "Should set empty environment variables");
            startInfo.EnvironmentVariables["TEST2_PATH_SELF"].Should().Be(Test2Path + Path.PathSeparator + "default", because: "Should prepend implementation path");
            startInfo.EnvironmentVariables["TEST2_VALUE"].Should().Be("test2", because: "Should directly set value");
            startInfo.EnvironmentVariables["TEST2_PATH_SUB_DEP"].Should().Be("default" + Path.PathSeparator + Path.Combine(Test2Path, "sub"), because: "Should append implementation sub-path");
            startInfo.EnvironmentVariables["TEST1_PATH_COMMAND"].Should().Be(Test1Path, because: "Should set implementation path");
            startInfo.EnvironmentVariables["TEST1_PATH_COMMAND_DEP"].Should().Be(Test1Path + Path.PathSeparator + Test1Path, because: "Should set implementation path for command dependency for each reference");
            startInfo.WorkingDirectory.Should().Be(Path.Combine(Test1Path, "bin"), because: "Should set implementation path");

            string execFile = Path.Combine(Test2Path, FileUtils.UnifySlashes(selections.Implementations[2].Commands[0].Path));
            var execArgs = new[]
            {
                selections.Implementations[2].Commands[0].Arguments[0].ToString(),
                selections.Implementations[1].Commands[1].Runner.Arguments[0].ToString(),
                Path.Combine(Test1Path, FileUtils.UnifySlashes(selections.Implementations[1].Commands[1].Path)),
                selections.Implementations[1].Commands[1].Arguments[0].ToString()
            };
            if (WindowsUtils.IsWindows)
            {
                startInfo.EnvironmentVariables["ZEROINSTALL_RUNENV_FILE_exec-in-var"].Should().Be(execFile);
                startInfo.EnvironmentVariables["ZEROINSTALL_RUNENV_ARGS_exec-in-var"].Should().Be(execArgs.JoinEscapeArguments());
                startInfo.EnvironmentVariables["ZEROINSTALL_RUNENV_FILE_exec-in-path"].Should().Be(execFile);
                startInfo.EnvironmentVariables["ZEROINSTALL_RUNENV_ARGS_exec-in-path"].Should().Be(execArgs.JoinEscapeArguments());
            }
            else
            {
                startInfo.EnvironmentVariables["ZEROINSTALL_RUNENV_exec-in-var"].Should().Be(execArgs.Prepend(execFile).JoinEscapeArguments());
                startInfo.EnvironmentVariables["ZEROINSTALL_RUNENV_exec-in-path"].Should().Be(execArgs.Prepend(execFile).JoinEscapeArguments());
            }
            startInfo.EnvironmentVariables["PATH"].Should().Be(Locations.GetCacheDirPath("0install.net", false, "injector", "executables", "exec-in-path") + Path.PathSeparator + new ProcessStartInfo().EnvironmentVariables["PATH"]);
        }

        /// <summary>
        /// Ensures <see cref="EnvironmentBuilder.ToStartInfo"/> handles complex <see cref="Selections"/>.
        /// </summary>
        [Fact]
        public void TestBaseline()
        {
            var selections = SelectionsTest.CreateTestSelections();
            selections.Implementations.Insert(0, new ImplementationSelection {InterfaceUri = new FeedUri("http://0install.de/feeds/test/dummy.xml")}); // Should be ignored by Executor

            var startInfo = new EnvironmentBuilder(GetMockStore(selections))
                .Inject(selections)
                .AddArguments("--custom$arg")
                .ToStartInfo();
            startInfo.FileName.Should().Be(
                Path.Combine(Test2Path, FileUtils.UnifySlashes(selections.Implementations[2].Commands[0].Path)),
                because: "Should combine runner implementation directory with runner command path");
            startInfo.Arguments.Should().Be(
                new[]
                {
                    selections.Implementations[2].Commands[0].Arguments[0].ToString(),
                    selections.Implementations[1].Commands[0].Runner.Arguments[0].ToString(),
                    Path.Combine(Test1Path, FileUtils.UnifySlashes(selections.Implementations[1].Commands[0].Path)),
                    selections.Implementations[1].Commands[0].Arguments[0].ToString(),
                    "--custom$arg"
                }.JoinEscapeArguments(),
                because: "Should combine core and additional runner arguments with application implementation directory, command path and arguments");

            VerifyEnvironment(startInfo, selections);
        }

        /// <summary>
        /// Ensures <see cref="EnvironmentBuilder.ToStartInfo"/> handles complex <see cref="Selections"/>.
        /// </summary>
        [SkippableFact]
        public void TestWrapper()
        {
            Skip.IfNot(WindowsUtils.IsWindows, "Wrapper command-line parsing relies on a Win32 API and therefore will not work on non-Windows platforms");

            var selections = SelectionsTest.CreateTestSelections();
            selections.Implementations.Insert(0, new ImplementationSelection {InterfaceUri = new FeedUri("http://0install.de/feeds/test/dummy.xml")}); // Should be ignored by Executor

            var startInfo = new EnvironmentBuilder(GetMockStore(selections))
                .Inject(selections)
                .AddWrapper("wrapper --wrapper")
                .AddArguments("--custom")
                .ToStartInfo();
            startInfo.FileName.Should().Be("wrapper");
            startInfo.Arguments.Should().Be(
                new[]
                {
                    "--wrapper",
                    Path.Combine(Test2Path, FileUtils.UnifySlashes(selections.Implementations[2].Commands[0].Path)),
                    selections.Implementations[2].Commands[0].Arguments[0].ToString(),
                    selections.Implementations[1].Commands[0].Runner.Arguments[0].ToString(),
                    Path.Combine(Test1Path, FileUtils.UnifySlashes(selections.Implementations[1].Commands[0].Path)),
                    selections.Implementations[1].Commands[0].Arguments[0].ToString(),
                    "--custom"
                }.JoinEscapeArguments(),
                because: "Should combine wrapper arguments, runner and application");

            VerifyEnvironment(startInfo, selections);
        }

        /// <summary>
        /// Ensures <see cref="EnvironmentBuilder.ToStartInfo"/> handles complex <see cref="Selections"/> and "main" overrides with relative paths.
        /// </summary>
        [Fact]
        public void TestMainRelative()
        {
            var selections = SelectionsTest.CreateTestSelections();
            selections.Implementations.Insert(0, new ImplementationSelection {InterfaceUri = new FeedUri("http://0install.de/feeds/test/dummy.xml")}); // Should be ignored by Executor

            var startInfo = new EnvironmentBuilder(GetMockStore(selections))
                .Inject(selections, overrideMain: "main")
                .AddArguments("--custom")
                .ToStartInfo();
            startInfo.FileName.Should().Be(
                Path.Combine(Test2Path, FileUtils.UnifySlashes(selections.Implementations[2].Commands[0].Path)),
                because: "Should combine runner implementation directory with runner command path");
            startInfo.Arguments.Should().Be(
                new[]
                {
                    selections.Implementations[2].Commands[0].Arguments[0].ToString(),
                    selections.Implementations[1].Commands[0].Runner.Arguments[0].ToString(),
                    Path.Combine(Test1Path, "dir 1", "main"),
                    "--custom"
                }.JoinEscapeArguments(),
                because: "Should combine core and additional runner arguments with application implementation directory, command directory and main binary override");

            VerifyEnvironment(startInfo, selections);
        }

        /// <summary>
        /// Ensures <see cref="EnvironmentBuilder.ToStartInfo"/> handles complex <see cref="Selections"/> and "main" overrides with absolute paths.
        /// </summary>
        [Fact]
        public void TestMainAbsolute()
        {
            var selections = SelectionsTest.CreateTestSelections();
            selections.Implementations.Insert(0, new ImplementationSelection {InterfaceUri = new FeedUri("http://0install.de/feeds/test/dummy.xml")}); // Should be ignored by Executor

            var startInfo = new EnvironmentBuilder(GetMockStore(selections))
                .Inject(selections, overrideMain: "/main")
                .AddArguments("--custom")
                .ToStartInfo();
            startInfo.FileName.Should().Be(
                Path.Combine(Test2Path, FileUtils.UnifySlashes(selections.Implementations[2].Commands[0].Path)),
                because: "Should combine runner implementation directory with runner command path");
            startInfo.Arguments.Should().Be(
                new[]
                {
                    selections.Implementations[2].Commands[0].Arguments[0].ToString(),
                    selections.Implementations[1].Commands[0].Runner.Arguments[0].ToString(),
                    Path.Combine(Test1Path, "main"),
                    "--custom"
                }.JoinEscapeArguments(),
                because: "Should combine core and additional runner arguments with application implementation directory and main binary override");

            VerifyEnvironment(startInfo, selections);
        }

        /// <summary>
        /// Ensures <see cref="EnvironmentBuilder.ToStartInfo"/> handles <see cref="Selections"/> with <see cref="Command.Path"/>s that are empty.
        /// </summary>
        [Fact]
        public void TestPathlessCommand()
        {
            var selections = SelectionsTest.CreateTestSelections();
            selections.Implementations.Insert(0, new ImplementationSelection {InterfaceUri = new FeedUri("http://0install.de/feeds/test/dummy.xml")}); // Should be ignored by Executor
            selections.Implementations[1].Commands[0].Path = null;

            var startInfo = new EnvironmentBuilder(GetMockStore(selections))
                .Inject(selections)
                .AddArguments("--custom")
                .ToStartInfo();
            startInfo.FileName.Should().Be(
                Path.Combine(Test2Path, FileUtils.UnifySlashes(selections.Implementations[2].Commands[0].Path)));
            startInfo.Arguments.Should().Be(
                new[]
                {
                    selections.Implementations[2].Commands[0].Arguments[0].ToString(),
                    selections.Implementations[1].Commands[0].Runner.Arguments[0].ToString(),
                    selections.Implementations[1].Commands[0].Arguments[0].ToString(),
                    "--custom"
                }.JoinEscapeArguments());

            VerifyEnvironment(startInfo, selections);
        }

        /// <summary>
        /// Ensures <see cref="EnvironmentBuilder.ToStartInfo"/> handles <see cref="Selections"/> with <see cref="ForEachArgs"/>.
        /// </summary>
        [Fact]
        public void TestForEachArgs()
        {
            var selections = SelectionsTest.CreateTestSelections();
            selections.Implementations.Insert(0, new ImplementationSelection {InterfaceUri = new FeedUri("http://0install.de/feeds/test/dummy.xml")}); // Should be ignored by Executor

            selections.Implementations[1].Commands[0].Arguments.Add(new ForEachArgs
            {
                ItemFrom = "SPLIT_ARG",
                Arguments = {"pre1 $item post1", "pre2 $item post2"}
            });
            selections.Implementations[2].Bindings.Add(new EnvironmentBinding {Name = "SPLIT_ARG", Value = "split1" + Path.PathSeparator + "split2"});

            var startInfo = new EnvironmentBuilder(GetMockStore(selections))
                .Inject(selections)
                .ToStartInfo();
            startInfo.FileName.Should().Be(
                Path.Combine(Test2Path, FileUtils.UnifySlashes(selections.Implementations[2].Commands[0].Path)),
                because: "Should combine runner implementation directory with runner command path");
            startInfo.Arguments.Should().Be(
                new[]
                {
                    selections.Implementations[2].Commands[0].Arguments[0].ToString(),
                    selections.Implementations[1].Commands[0].Runner.Arguments[0].ToString(),
                    Path.Combine(Test1Path, FileUtils.UnifySlashes(selections.Implementations[1].Commands[0].Path)),
                    selections.Implementations[1].Commands[0].Arguments[0].ToString(),
                    "pre1 split1 post1",
                    "pre2 split1 post2",
                    "pre1 split2 post1",
                    "pre2 split2 post2"
                }.JoinEscapeArguments(),
                because: "Should combine core and additional runner arguments with application implementation directory and command path");

            VerifyEnvironment(startInfo, selections);
        }
    }
}
