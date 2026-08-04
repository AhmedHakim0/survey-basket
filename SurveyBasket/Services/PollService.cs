
namespace SurveyBasket.Services;

public class PollService(ApplicationDbContext context) : IPollService
{
    private readonly ApplicationDbContext _context = context;
    public async Task<Result<IEnumerable<PollResponse>>> GetAllAsync()
    {
        var polls = await _context.Polls.AsNoTracking().ToListAsync();
        return Result.Success(polls.MapToResponse());
    }
            
    public async Task<Result<PollResponse>> GetAsync(int id)
    {
        var poll = await _context.Polls.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

        if(poll is null) 
            return Result.Failure<PollResponse>(PollErrors.PollNotFound);

        return Result.Success(poll.MapToResponse());
    }
    public async Task<Result<PollResponse>> AddAsync(PollRequest poll, CancellationToken cancellationToken = default)
    {
      var newPoll =  await _context.Polls.AddAsync(poll.MapToPoll(),cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success(newPoll.Entity.MapToResponse());
    }
    public async Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellationToken = default)
    {
        var existingPoll = await _context.Polls.FirstOrDefaultAsync(p => p.Id == id);
        if (existingPoll is not null)
        {
            existingPoll.Title = poll.Title;
            existingPoll.Summary = poll.Summary;
            existingPoll.StartsAt = poll.StartsAt;
            existingPoll.EndsAt = poll.EndsAt;
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        return Result.Failure(PollErrors.PollNotFound);
    }
    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.FirstOrDefaultAsync(p => p.Id == id);
        if (poll is not null)
        {
            _context.Polls.Remove(poll);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        return Result.Failure(PollErrors.PollNotFound);
    }
    public async Task<Result> TogglePublishAsync(int id, CancellationToken cancellationToken = default)
    {
        var CurrentPoll = await _context.Polls.FirstOrDefaultAsync(p => p.Id == id);
        if (CurrentPoll != null)
        {
            CurrentPoll.IsPublished = !CurrentPoll.IsPublished;
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        return Result.Failure(PollErrors.PollNotFound);
    }


}
