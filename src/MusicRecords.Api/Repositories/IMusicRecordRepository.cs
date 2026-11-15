using MusicRecords.Api.Models;
namespace MusicRecords.Api.Repositories;
public interface IMusicRecordRepository
{
    IReadOnlyList<MusicRecord> GetAll();
}
