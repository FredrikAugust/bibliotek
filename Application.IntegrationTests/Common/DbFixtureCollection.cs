using Xunit;

namespace Application.IntegrationTests.Common;

[CollectionDefinition("DbCollection")]
public class DbFixtureCollection : ICollectionFixture<DbFixture> { }