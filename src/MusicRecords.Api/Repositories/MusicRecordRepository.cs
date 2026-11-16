using Microsoft.EntityFrameworkCore;
using MusicRecords.Api.Models;

namespace MusicRecords.Api.Repositories;

public sealed class MusicRecordRepository(MusicRecordsDbContext database) : IMusicRecordRepository
{
    public IReadOnlyList<MusicRecord> GetAll() => Search(null, null, null);

    public IReadOnlyList<MusicRecord> Search(string? title, string? artist, int? publicationYear)
    {
        var searchTitle = title?.Trim().ToUpperInvariant() ?? string.Empty;
        var searchArtist = artist?.Trim().ToUpperInvariant() ?? string.Empty;

        return database.MusicRecords.AsNoTracking()
            .Where(record => publicationYear == null || record.PublicationYear == publicationYear)
            .Where(record => EF.Property<string>(record, "SearchTitle").Contains(searchTitle))
            .Where(record => EF.Property<string>(record, "SearchArtist").Contains(searchArtist))
            .OrderBy(record => record.Id)
            .ToArray();
    }

    public MusicRecord? Find(int id) =>
        database.MusicRecords.AsNoTracking().SingleOrDefault(record => record.Id == id);

    public MusicRecord Add(NewMusicRecord input)
    {
        var record = new MusicRecord(
            Id: 0,
            Title: input.Title.Trim(),
            Artist: input.Artist.Trim(),
            Duration: input.Duration!.Value,
            PublicationYear: input.PublicationYear!.Value);

        var entry = database.MusicRecords.Add(record);
        // Store uppercase copies for case-insensitive search on both databases.
        entry.Property<string>("SearchTitle").CurrentValue = record.Title.ToUpperInvariant();
        entry.Property<string>("SearchArtist").CurrentValue = record.Artist.ToUpperInvariant();
        database.SaveChanges();
        return record;
    }

    public bool Delete(int id)
    {
        var deletedRows = database.MusicRecords.Where(record => record.Id == id).ExecuteDelete();
        return deletedRows > 0;
    }
}
