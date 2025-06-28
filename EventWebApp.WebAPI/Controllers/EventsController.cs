using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;
using EventWebApp.Core.Interfaces;
using EventWebApp.Application.UseCases.Event;
using EventWebApp.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventWebApp.WebAPI.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class EventsController : ControllerBase
  {
    private readonly CreateEventUseCase createEventUseCase;
    private readonly GetAllEventsUseCase getAllEventsUseCase;
    private readonly GetEventByIdUseCase getEventByIdUseCase;
    private readonly UpdateEventUseCase updateEventUseCase;
    private readonly DeleteEventUseCase deleteEventUseCase;
    private readonly FilterEventsUseCase filterEventsUseCase;
    private readonly GetByTitleUseCase getByTitleUseCase;
    private readonly GetPagedEventsUseCase getPagedEventsUseCase;
    private readonly SearchEventsUseCase searchEventsUseCase;
    private readonly UploadEventImageWithValidationUseCase uploadEventImageWithValidationUseCase;

    public EventsController(
        CreateEventUseCase createEventUseCase,
        GetAllEventsUseCase getAllEventsUseCase,
        GetEventByIdUseCase getEventByIdUseCase,
        UpdateEventUseCase updateEventUseCase,
        DeleteEventUseCase deleteEventUseCase,
        FilterEventsUseCase filterEventsUseCase,
        GetByTitleUseCase getByTitleUseCase,
        GetPagedEventsUseCase getPagedEventsUseCase,
        SearchEventsUseCase searchEventsUseCase,
        UploadEventImageWithValidationUseCase uploadEventImageWithValidationUseCase
    )
    {
      this.createEventUseCase = createEventUseCase;
      this.getAllEventsUseCase = getAllEventsUseCase;
      this.getEventByIdUseCase = getEventByIdUseCase;
      this.updateEventUseCase = updateEventUseCase;
      this.deleteEventUseCase = deleteEventUseCase;
      this.filterEventsUseCase = filterEventsUseCase;
      this.getByTitleUseCase = getByTitleUseCase;
      this.getPagedEventsUseCase = getPagedEventsUseCase;
      this.searchEventsUseCase = searchEventsUseCase;
      this.uploadEventImageWithValidationUseCase = uploadEventImageWithValidationUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
      var result = await getAllEventsUseCase.ExecuteAsync(cancellationToken);
      return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
      var result = await getEventByIdUseCase.ExecuteAsync(id, cancellationToken);
      return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("filter")]
    public async Task<IActionResult> Filter(
        [FromQuery] string? category,
        [FromQuery] string? location,
        [FromQuery] DateTime? dateTime,
        [FromQuery] string? title,
        CancellationToken cancellationToken
    )
    {
      Console.WriteLine(
          $"Received filter request - Category: {category}, Location: {location}, DateTime: {dateTime}, Title: {title}"
      );
      var events = await filterEventsUseCase.ExecuteAsync(
          category,
          location,
          dateTime,
          title,
          cancellationToken
      );
      Console.WriteLine($"Found {events.Count()} events");
      return Ok(events);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventRequest createEventRequest, CancellationToken cancellationToken)
    {
      var result = await createEventUseCase.ExecuteAsync(createEventRequest, cancellationToken);
      return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
      await deleteEventUseCase.ExecuteAsync(id, cancellationToken);
      return NoContent();
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateEventRequest updateEventRequest, CancellationToken cancellationToken)
    {
      await updateEventUseCase.ExecuteAsync(updateEventRequest, cancellationToken);
      return NoContent();
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchByTitle([FromQuery] string title, CancellationToken cancellationToken)
    {
      var result = await searchEventsUseCase.ExecuteAsync(title, cancellationToken);
      var (statusCode, data) = result.ToHttpResponse();

      return statusCode switch
      {
        200 => Ok(data),
        400 => BadRequest(data),
        _ => StatusCode(statusCode, data)
      };
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("{id}/upload-image")]
    public async Task<IActionResult> UploadImage(Guid id, IFormFile file, CancellationToken cancellationToken)
    {
      var result = await uploadEventImageWithValidationUseCase.ExecuteAsync(id, file, cancellationToken);
      var (statusCode, data) = result.ToHttpResponse();

      return statusCode switch
      {
        200 => Ok(data),
        400 => BadRequest(data),
        _ => StatusCode(statusCode, data)
      };
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged([FromQuery] PaginationRequest request, CancellationToken cancellationToken)
    {
      var result = await getPagedEventsUseCase.ExecuteAsync(request, cancellationToken);
      return Ok(result);
    }

    [HttpGet("test-email")]
    public async Task<IActionResult> TestEmail([FromServices] INotificationService notifier, CancellationToken cancellationToken)
    {
      await notifier.NotifyUsersAsync(
          new[] { "Pavel91104@gmail.com" },
          "Здравствуйте, Павел",
          "Текст письма",
          cancellationToken
      );

      return Ok("Письмо отправлено");
    }
  }
}
