using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MusicRecords.Api.Models;
using MusicRecords.Api.Repositories;
namespace MusicRecords.Api.Controllers;
[ApiController]
[Route("api/musicrecords")]
public class MusicRecordsController(IMusicRecordRepository repository) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MusicRecord>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<MusicRecord>> GetAll(
        [FromQuery] string? title = null,
        [FromQuery] string? artist = null,
        [FromQuery, Range(1, 9999)] int? publicationYear = null)
        => Ok(repository.Search(title, artist, publicationYear));
}
