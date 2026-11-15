using MusicRecords.Api.Models;
namespace MusicRecords.Api.Repositories;
public sealed class MusicRecordRepository : IMusicRecordRepository
{
    private readonly List<MusicRecord> records;
    public MusicRecordRepository() : this(new[]
    {
        new MusicRecord(1, "Kind of Blue", "Miles Davis", 2757, 1959),
        new MusicRecord(2, "Abbey Road", "The Beatles", 2813, 1969),
        new MusicRecord(3, "Hounds of Love", "Kate Bush", 2847, 1985)
    }) { }
    public MusicRecordRepository(IEnumerable<MusicRecord> initialRecords)
    {
        records = initialRecords.ToList();
    }
    // Return a snapshot so callers cannot change repository contents.
    public IReadOnlyList<MusicRecord> GetAll() => records.ToArray();
}
