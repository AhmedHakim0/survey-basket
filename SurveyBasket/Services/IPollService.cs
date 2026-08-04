namespace SurveyBasket.Services;

public interface IPollService
{
    Task<Result<IEnumerable<PollResponse>>> GetAllAsync();
    Task<Result<PollResponse>> GetAsync(int id);
    Task<Result<PollResponse>> AddAsync(PollRequest poll, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<Result> TogglePublishAsync(int id, CancellationToken cancellationToken = default);
}
