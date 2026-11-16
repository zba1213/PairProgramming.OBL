using Microsoft.EntityFrameworkCore;
using MusicRecords.Api.Models;

namespace MusicRecords.Api.Repositories;

public sealed class MusicRecordRepository(MusicRecordsDbContext database) : IMusicRecordRepository
{
    public IReadOnlyList<MusicRecord> GetAll() => Search(null, null, null);

    public IReadOnlyList<MusicRecord> Search(string? title, string? artist, int? publicationYear)
    {
        title = title?.Trim().ToUpperInvariant() ?? string.Empty;
        artist = artist?.Trim().ToUpperInvariant() ?? string.Empty;

        return database.MusicRecords.AsNoTracking()
            .Where(record => (publicationYear == null || record.PublicationYear == publicationYear)
                && EF.Property<string>(record, "SearchTitle").Contains(title)
                && EF.Property<string>(record, "SearchArtist").Contains(artist))
            .OrderBy(record => record.Id)
            .ToArray();
    }

    public MusicRecord? Find(int id) =>
        database.MusicRecords.AsNoTracking().SingleOrDefault(record => record.Id == id);

    public MusicRecord Add(NewMusicRecord input)
    {
        var record = new MusicRecord(0, input.Title.Trim(), input.Artist.Trim(),
            input.Duration!.Value, input.PublicationYear!.Value);
        var entry = database.MusicRecords.Add(record);
        entry.Property("SearchTitle").CurrentValue = record.Title.ToUpperInvariant();
        entry.Property("SearchArtist").CurrentValue = record.Artist.ToUpperInvariant();
        database.SaveChanges();
        return record;
    }

    public bool Delete(int id) => database.MusicRecords.Where(record => record.Id == id).ExecuteDelete() > 0;
}
