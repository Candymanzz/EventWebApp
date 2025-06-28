using EventWebApp.Application.DTOs;
using EventWebApp.Application.UseCases.Event;
using EventWebApp.Core.Model;

namespace EventWebApp.Application.UseCases.Event
{
  public class SearchEventsUseCase
  {
    private readonly GetByTitleUseCase _getByTitleUseCase;

    public SearchEventsUseCase(GetByTitleUseCase getByTitleUseCase)
    {
      _getByTitleUseCase = getByTitleUseCase;
    }

    public async Task<SearchResult> ExecuteAsync(string title, CancellationToken cancellationToken)
    {
      if (string.IsNullOrWhiteSpace(title))
      {
        return SearchResult.Failure("Search title cannot be empty");
      }

      var events = await _getByTitleUseCase.ExecuteAsync(title, cancellationToken);
      return SearchResult.Success(new[] { events });
    }
  }
}