using MillionBackend.Core.Models;
using MillionBackend.Core.Repositories;
using MillionBackend.Infrastructure.Repositories;
using MongoDB.Driver;
using Moq;

namespace MillionBackend.Tests.Repositories;

[TestFixture]
public class PropertyRepositoryTests
{
    private Mock<IMongoDatabase> _mockDatabase;
    private Mock<IMongoCollection<Property>> _mockCollection;
    private PropertyRepository _repository;

    [SetUp]
    public void Setup()
    {
        _mockDatabase = new Mock<IMongoDatabase>();
        _mockCollection = new Mock<IMongoCollection<Property>>();

        _mockDatabase.Setup(db => db.GetCollection<Property>("properties", null))
            .Returns(_mockCollection.Object);
        _mockDatabase.Setup(db => db.GetCollection<Owner>("owners", null))
            .Returns(Mock.Of<IMongoCollection<Owner>>());
        _mockDatabase.Setup(db => db.GetCollection<PropertyImage>("propertyImages", null))
            .Returns(Mock.Of<IMongoCollection<PropertyImage>>());
        _mockDatabase.Setup(db => db.GetCollection<PropertyTrace>("propertyTraces", null))
            .Returns(Mock.Of<IMongoCollection<PropertyTrace>>());

        _repository = new PropertyRepository(_mockDatabase.Object);
    }

    [Test]
    public void Constructor_InitializesCollections()
    {
        Assert.That(_repository, Is.Not.Null);
        _mockDatabase.Verify(db => db.GetCollection<Property>("properties", null), Times.Once);
    }
}
