using MusicRecords.Api.Models;
using MusicRecords.Api.Repositories;
namespace MusicRecords.Tests;
[TestClass]
public class RepositoryTests
{
    [DataTestMethod]
    [DataRow(null, null, null, 3)]
    [DataRow("   ", "", null, 3)]
    [DataRow(" bLuE ", null, null, 1)]
    [DataRow(null, "BEATLES", null, 1)]
    [DataRow(null, null, 1985, 1)]
    [DataRow("blue", "miles", 1959, 1)]
    [DataRow("blue", "miles", 1969, 0)]
    [DataRow("blue", "beatles", null, 0)]
    public void Search_CombinesOptionalFilters(string? title, string? artist, int? year, int count)
    {
        Assert.AreEqual(count, new MusicRecordRepository().Search(title, artist, year).Count);
    }

    [TestMethod]
    public void GetAll_DefaultRepository_ContainsThreeSampleRecords()
    {
        Assert.AreEqual(3, new MusicRecordRepository().GetAll().Count);
    }
    [TestMethod]
    public void GetAll_EmptyRepository_ReturnsEmptyList()
    {
        var repository = new MusicRecordRepository([]);
        Assert.AreEqual(0, repository.GetAll().Count);
    }
    [TestMethod]
    public void GetAll_PreservesAllRecordFields()
    {
        var record = new MusicRecord(7, "Test", "Kunstner", 125, 2024);
        var repository = new MusicRecordRepository([record]);
        CollectionAssert.AreEqual(new[] { record }, repository.GetAll().ToArray());
    }
    [TestMethod]
    public void GetAll_CallerCannotModifyRepositoryThroughResultOrInput()
    {
        var original = new MusicRecord(1, "Original", "Artist", 100, 2000);
        var input = new List<MusicRecord> { original };
        var repository = new MusicRecordRepository(input);
        input.Clear();
        var snapshot = (MusicRecord[])repository.GetAll();
        snapshot[0] = original with { Title = "Changed" };
        Assert.AreEqual(original, repository.GetAll().Single());
    }
}
