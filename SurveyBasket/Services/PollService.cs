namespace SurveyBasket.Services;

public class PollService : IPollService
{
    private readonly List<Poll> _polls = [
       new Poll{
            Id =1,
            Title = "Poll 1",
            Description = "This is the first poll"
        }

       ];
    public IEnumerable<Poll> GetAll() => _polls;
    
    public Poll? Get(int id) => _polls.SingleOrDefault(p => p.Id == id);
    

    public Poll Add(Poll poll)
    {
        poll.Id = _polls.Count + 1;
        _polls.Add(poll);
        return poll;
    }


    public bool Update(int id, Poll poll)
    {
        var existingPoll = Get(id);
        if ( existingPoll != null)
        {
            existingPoll.Title = poll.Title;
            existingPoll.Description = poll.Description;
            return true;
        }
        return false;
    }

    public bool Delete(int id)
    {
        var poll = Get(id);
        if (poll != null)
        {
            _polls.Remove(poll);
            return true;
        }
        return false;
    }
}
