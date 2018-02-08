using System;
using System.Reflection;
using Starcounter;
using Starcounter.Hosting;

namespace Authorization.Database.Tests
{
    public class TestContext : IDisposable
    {
        readonly ICodeHost host;

        // Retrieve the name from the test assembly
        // You can use any name as long as it's not occupied
        public string DatabaseName { get; } =
            typeof(TestContext)
                .Assembly.GetName().Name
                .Replace(".", string.Empty)
            .Replace("_", string.Empty)
            ;

        public TestContext()
        {
            // Start server to interact with host 
            // Stop host in case a database is already running
            // Recreate the database to start clean
            var setup = new TestSetup(DatabaseName)
                .StartServer()
                .StopHost()
                .RecreateDatabase();

            // Load the database into the app host and
            // point to the assembly to test
            host = new AppHostBuilder()
                .UseDatabase(DatabaseName)
                .UseApplication(Assembly.GetExecutingAssembly())
                .Build();

            host.Start();
        }

        // Clean up the host after the test are finished
        void IDisposable.Dispose()
        {
            host.Dispose();
        }
    }
}