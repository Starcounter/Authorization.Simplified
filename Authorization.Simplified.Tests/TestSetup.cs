using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Authorization.Database.Tests
{
    public class TestSetup
    {
        private readonly string DatabaseName;

        public TestSetup(string databaseName)
        {
            this.DatabaseName = databaseName;
        }

        public TestSetup StartServer()
        {
            RunStarAdmin("start server");
            return this;
        }

        public TestSetup StopHost()
        {
            RunStarAdmin($"-d={DatabaseName} stop host",
                KnownExitCodes.HostNotRunning,
                KnownExitCodes.DatabaseDoesNotExist
            );
            return this;
        }

        public TestSetup DeleteDatabase()
        {
            RunStarAdmin($"-d={DatabaseName} delete --force db");
            return this;
        }

        public TestSetup CreateDatabase()
        {
            RunStarAdmin($"-d={DatabaseName} new db");
            return this;
        }

        public TestSetup RecreateDatabase()
        {
            return DeleteDatabase().CreateDatabase();
        }

        private void RunStarAdmin(
            string args,
            params int[] allowedExits)
        {
            List<int> exits = new List<int>() { 0 };

            if (allowedExits != null)
            {
                exits.AddRange(allowedExits);
            }

            var processInfo = new ProcessStartInfo()
            {
                FileName = "staradmin",
                Arguments = args,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            var process = Process.Start(processInfo);
            if (process == null)
            {
                throw new Exception($"Could not start 'staradmin {args}': Process.Start returned null");
            }

            process.WaitForExit();

            if (!exits.Contains(process.ExitCode))
            {
                throw new Exception($"'staradmin {args}' exited with unexpected code {process.ExitCode}");
            }
        }

        /// Exit codes we might want to explicitly ignore
        private sealed class KnownExitCodes
        {
            public const int HostNotRunning = 10024;
            public const int DatabaseDoesNotExist = 10002;
        }
    }
}
