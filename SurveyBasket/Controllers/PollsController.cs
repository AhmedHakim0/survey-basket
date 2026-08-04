
namespace SurveyBasket.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var polls = await _pollService.GetAllAsync();
        return Ok(polls.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var poll = await _pollService.GetAsync(id);
        return poll.IsSuccess ? Ok(poll.Value) : NotFound(poll.Error);
    }

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] PollRequest pollRequest,CancellationToken cancellationToken)
    {
        var poll = await _pollService.AddAsync(pollRequest, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = poll.Value.Id }, poll.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest pollRequest, CancellationToken cancellationToken)
    {
        var result = await _pollService.UpdateAsync(id, pollRequest, cancellationToken);

        return result.IsSuccess ? NoContent() : NotFound(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.DeleteAsync(id, cancellationToken);

        return result.IsSuccess ? NoContent() : NotFound(result.Error);
    }

    [HttpPut("{id}/TogglePublish")]
    public async Task<IActionResult> TogglePublish( int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.TogglePublishAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(result.Error);
    }


}
