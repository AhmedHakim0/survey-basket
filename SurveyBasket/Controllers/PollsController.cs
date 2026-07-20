using SurveyBasket.Contracts.Requests;
using SurveyBasket.Mapping;

namespace SurveyBasket.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;

    [HttpGet("")]
    public IActionResult GetAll()
    {
        var polls = _pollService.GetAll();
        return Ok(polls.MapToResponse());
    } 

    [HttpGet("{id}")]
    public IActionResult Get([FromRoute] int id)
    {
        var poll = _pollService.Get(id);
        return poll is null ? NotFound() : Ok(poll.MapToResponse());
    }

    [HttpPost("")]
    public IActionResult Create([FromBody] PollRequest pollRequest)
    {
       var poll =  _pollService.Add(pollRequest.MapToPoll());

        return CreatedAtAction(nameof(Get), new { id = poll.Id }, poll.MapToResponse());
    }

    [HttpPut("{id}")]
    public IActionResult Update([FromRoute] int id, [FromBody] PollRequest pollRequest)
    {
        var isUpdated = _pollService.Update(id, pollRequest.MapToPoll());

        return isUpdated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var isDeleted = _pollService.Delete(id);

        return isDeleted ? NoContent() : NotFound();
    }

}
