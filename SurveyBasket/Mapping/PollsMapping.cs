using SurveyBasket.Contracts.Requests;
using SurveyBasket.Contracts.Responses;
using System.Runtime.CompilerServices;

namespace SurveyBasket.Mapping;

public static class PollsMapping
{
    public static Poll MapToPoll(this PollRequest pollRequest) 
    {
        return new Poll
        {
            Title = pollRequest.Title,
            Summary = pollRequest.Summary,
            IsPublished = pollRequest.IsPublished,
            StartsAt = pollRequest.StartsAt,
            EndsAt = pollRequest.EndsAt
        };
    }

    public static PollResponse MapToResponse(this Poll poll)
    {
        return new PollResponse
        (
             poll.Id,
             poll.Title,
             poll.Summary,
             poll.IsPublished,
             poll.StartsAt,
             poll.EndsAt
        );
    }

    public static IEnumerable<PollResponse> MapToResponse(this IEnumerable<Poll> polls)
    {
        return polls.Select(MapToResponse);
    }
}
