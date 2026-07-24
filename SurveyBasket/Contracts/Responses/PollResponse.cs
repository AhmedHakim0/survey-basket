namespace SurveyBasket.Contracts.Responses;

public record PollResponse(
    int Id,
    string Title,
    string summary,
    bool IsPublished,
    DateOnly StartsAt,
    DateOnly EndsAt
);

