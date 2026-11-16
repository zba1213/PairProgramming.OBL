using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MusicRecord), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<MusicRecord> GetById(int id)
        => repository.Find(id) is { } record ? Ok(record) : NotFound();

    [HttpPost]
    [Authorize(Roles = "Editor")]
    [ProducesResponseType(typeof(MusicRecord), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public ActionResult<MusicRecord> Create(NewMusicRecord input)
    {
        var record = repository.Add(input);
        return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Editor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, NewMusicRecord input)
    {
        if (!repository.Update(id, input))
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Editor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        if (!repository.Delete(id))
            return NotFound();

        return NoContent();
    }
}
