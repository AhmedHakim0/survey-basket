namespace SurveyBasket.Services;

public interface IPollService
{
   Task<IEnumerable<Poll>> GetAllAsync();
    Task<Poll?> GetAsync(int id);
   Task<Poll> AddAsync(Poll poll, CancellationToken cancellationToken = default);
    Task<bool> Update(int id, Poll poll, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
