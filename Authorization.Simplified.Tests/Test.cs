using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Starcounter;
using Xunit;

namespace Authorization.Database.Tests
{
    [Collection(nameof(TestContext))]
    public class Test
    {
        [Fact]
        public async Task TestIfTestsTestTheTests()
        {
            await Scheduling.RunTask(() => Db.Transact(() => {
                Assert.Equal(0, GetCurrentNumberOfItems());
                new FakeDbClass();
                Assert.Equal(1, GetCurrentNumberOfItems());
            }));
        }

        private static int GetCurrentNumberOfItems()
        {
            return Db.SQL<FakeDbClass>($"select a from {typeof(FakeDbClass).FullName} a").Count();
        }
    }

    [Database]
    public class FakeDbClass
    {
        public int Number { get; set; }
    }

    [CollectionDefinition(nameof(TestContext))]
    public class TestContextCollection : ICollectionFixture<TestContext>
    {
    }
}