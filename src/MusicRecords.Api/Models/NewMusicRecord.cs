using System.ComponentModel.DataAnnotations;

namespace MusicRecords.Api.Models;

public sealed record NewMusicRecord(
    [Required, StringLength(200)] string Title,
    [Required, StringLength(200)] string Artist,
    [Required, Range(1, int.MaxValue)] int? Duration,
    [Required, Range(1, 9999)] int? PublicationYear);
