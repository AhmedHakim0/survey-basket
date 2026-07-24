using SurveyBasket.Contracts.Requests;
using SurveyBasket.Mapping;

namespace SurveyBasket.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var polls = await _pollService.GetAllAsync();
        return Ok(polls.MapToResponse());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var poll = await _pollService.GetAsync(id);
        return poll is null ? NotFound() : Ok(poll.MapToResponse());
    }

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] PollRequest pollRequest,CancellationToken cancellationToken)
    {
        var poll = await _pollService.AddAsync(pollRequest.MapToPoll(), cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = poll.Id }, poll.MapToResponse());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest pollRequest, CancellationToken cancellationToken)
    {
        var isUpdated = await _pollService.Update(id, pollRequest.MapToPoll(), cancellationToken);

        return isUpdated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var isDeleted = await _pollService.DeleteAsync(id, cancellationToken);

        return isDeleted ? NoContent() : NotFound();
    }

}
