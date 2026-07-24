using System.Reflection.Metadata.Ecma335;

namespace SurveyBasket.Services;

public class PollService(ApplicationDbContext context) : IPollService
{
   private readonly ApplicationDbContext _context = context;
    public async  Task<IEnumerable<Poll>> GetAllAsync() =>
             await _context.Polls.AsNoTracking().ToListAsync();

    public async Task<Poll?> GetAsync(int id) => await _context.Polls.FindAsync(id);


    public async Task<Poll> AddAsync(Poll poll, CancellationToken cancellationToken= default)
    {
        await _context.Polls.AddAsync(poll,cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return poll;
    }


    public async Task<bool> Update(int id, Poll poll, CancellationToken cancellationToken = default)
    {
        var existingPoll = await GetAsync(id);
        if (existingPoll != null)
        {
            existingPoll.Title = poll.Title;
            existingPoll.Summary = poll.Summary;
            existingPoll.IsPublished =poll.IsPublished;
            existingPoll.StartsAt = poll.StartsAt;
            existingPoll.EndsAt = poll.EndsAt;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        return false;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await GetAsync(id);
        if (poll != null)
        {
            _context.Polls.Remove(poll);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        return false;
    }
}
