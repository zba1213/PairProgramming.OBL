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
    public ActionResult<IEnumerable<MusicRecord>> GetAll() => Ok(repository.GetAll());
}
