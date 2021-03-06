﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.TestModels.GearsOfWarModel;
using Microsoft.EntityFrameworkCore.TestUtilities.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Npgsql.EntityFrameworkCore.PostgreSQL.Query
{
    public class AsyncGearsOfWarQuerySqlServerTest : AsyncGearsOfWarQueryTestBase<GearsOfWarQueryNpgsqlFixture>
    {
        public AsyncGearsOfWarQuerySqlServerTest(GearsOfWarQueryNpgsqlFixture fixture, ITestOutputHelper testOutputHelper)
        : base(fixture)
        {
            Fixture.TestSqlLoggerFactory.Clear();
            //Fixture.TestSqlLoggerFactory.SetTestOutputHelper(testOutputHelper);
        }

        // The following was copied from the EF Core source code at commit c360e894f74f9c295bb0021328a2489e6e329835.
        // The CI nuget (2.1.0-rc1-30667) still contains an older version with a bug, it should be possible to
        // remove this as soon as a new nuget is available.
        [Fact]
        public override async Task Correlated_collection_order_by_constant()
        {
            await AssertQuery<Gear>(
                gs => gs.OrderByDescending(s => 1).Select(g => new { g.Nickname, Weapons = g.Weapons.Select(w => w.Name).ToList() }),
                elementSorter: e => e.Nickname,
                elementAsserter: (e, a) =>
                {
                    Assert.Equal(e.Nickname, a.Nickname);
                    CollectionAsserter<string>(ee => ee)(e.Weapons, a.Weapons);
                });
        }
    }
}
