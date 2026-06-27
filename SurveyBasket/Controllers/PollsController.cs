namespace SurveyBasket.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;

    [HttpGet("")]
    public IActionResult GetAll()
    {
        return Ok(_pollService.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var poll = _pollService.Get(id);
        return poll is null ? NotFound() : Ok(poll);
    }

    [HttpPost("")]
    public IActionResult Create(Poll Poll)
    {
        _pollService.Add(Poll);
        return CreatedAtAction(nameof(Get), new { id = Poll.Id }, Poll);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id ,Poll poll)
    {
        var isUpdated =_pollService.Update(id, poll);
        return isUpdated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var isDeleted = _pollService.Delete(id);
        return isDeleted ? NoContent() : NotFound();
    }

}
