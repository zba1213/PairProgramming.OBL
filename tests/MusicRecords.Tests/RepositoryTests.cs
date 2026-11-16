using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MusicRecords.Api.Models;
using MusicRecords.Api.Repositories;

namespace MusicRecords.Tests;

[TestClass]
public class RepositoryTests
{
    private SqliteConnection connection = null!;
    private DbContextOptions<MusicRecordsDbContext> options = null!;
    private MusicRecordsDbContext database = null!;
    private MusicRecordRepository repository = null!;

    [TestInitialize]
    public void Initialize()
    {
        connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        options = new DbContextOptionsBuilder<MusicRecordsDbContext>().UseSqlite(connection).Options;
        database = new MusicRecordsDbContext(options);
        database.Database.Migrate();
        repository = new MusicRecordRepository(database);
    }

    [TestCleanup]
    public void Cleanup()
    {
        database?.Dispose();
        connection?.Dispose();
    }

    [TestMethod]
    public void GetAll_ReturnsSeedRecordsInOrder()
    {
        CollectionAssert.AreEqual(new[] { 1, 2, 3 }, repository.GetAll().Select(record => record.Id).ToArray());
    }

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
        Assert.AreEqual(count, repository.Search(title, artist, year).Count);
    }

    [TestMethod]
    public void Add_PersistsTrimmedRecordForAnotherContextAndSupportsUnicodeSearch()
    {
        var record = repository.Add(new NewMusicRecord("  Ét øjeblik  ", "  Søren  ", 125, 2024));
        Assert.IsTrue(record.Id > 3);

        using var readDatabase = new MusicRecordsDbContext(options);
        var reader = new MusicRecordRepository(readDatabase);
        var expected = new MusicRecord(record.Id, "Ét øjeblik", "Søren", 125, 2024);
        Assert.AreEqual(expected, reader.Find(record.Id));
        Assert.AreEqual(expected, reader.Search("ÉT ØJE", "søRen", 2024).Single());
    }

    [TestMethod]
    public void Update_SavesNewFieldsAndUpdatesSearch()
    {
        var input = new NewMusicRecord("  Nyt øjeblik  ", "  Søren  ", 150, 2025);
        Assert.IsTrue(repository.Update(1, input));

        using var readDatabase = new MusicRecordsDbContext(options);
        var reader = new MusicRecordRepository(readDatabase);
        var expected = new MusicRecord(1, "Nyt øjeblik", "Søren", 150, 2025);
        Assert.AreEqual(expected, reader.Find(1));
        Assert.AreEqual(expected, reader.Search("NYT ØJE", "søRen", 2025).Single());
        Assert.AreEqual(0, reader.Search("Kind of Blue", null, null).Count);
        Assert.AreEqual(3, reader.GetAll().Count);
    }

    [TestMethod]
    public void Update_ReturnsFalseWhenRecordDoesNotExist()
    {
        Assert.IsFalse(repository.Update(999, new NewMusicRecord("Titel", "Kunstner", 150, 2025)));
        Assert.AreEqual(3, repository.GetAll().Count);
    }

    [TestMethod]
    public void Delete_RemovesOnlyTheRequestedRecordAndDoesNotReseed()
    {
        Assert.IsTrue(repository.Delete(1));
        Assert.IsFalse(repository.Delete(1));
        Assert.IsNull(repository.Find(1));
        Assert.IsTrue(repository.Delete(2));
        Assert.IsTrue(repository.Delete(3));
        database.Database.Migrate();
        Assert.AreEqual(0, repository.GetAll().Count);
    }
}
