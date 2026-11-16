using MusicRecords.Api.Models;

namespace MusicRecords.Api.Repositories;

public interface IMusicRecordRepository
{
    IReadOnlyList<MusicRecord> GetAll();
    IReadOnlyList<MusicRecord> Search(string? title, string? artist, int? publicationYear);
    MusicRecord? Find(int id);
    MusicRecord Add(NewMusicRecord input);
    bool Update(int id, NewMusicRecord input);
    bool Delete(int id);
}
