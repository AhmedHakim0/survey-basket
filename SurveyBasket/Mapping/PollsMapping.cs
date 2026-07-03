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
            Description = pollRequest.Description
        };
    }

    public static PollResponse MapToResponse(this Poll poll)
    {
        return new PollResponse
        (
             poll.Id,
             poll.Title,
             poll.Description
        );
    }

    public static IEnumerable<PollResponse> MapToResponse(this IEnumerable<Poll> polls)
    {
        return polls.Select(MapToResponse);
    }
}
