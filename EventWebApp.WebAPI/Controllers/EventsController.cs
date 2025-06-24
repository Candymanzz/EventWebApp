using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;
using EventWebApp.Core.Interfaces;
using EventWebApp.Application.UseCases.Event;
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
    private readonly UploadEventImageUseCase uploadEventImageUseCase;
    private readonly GetPagedEventsUseCase getPagedEventsUseCase;

    public EventsController(
        CreateEventUseCase createEventUseCase,
        GetAllEventsUseCase getAllEventsUseCase,
        GetEventByIdUseCase getEventByIdUseCase,
        UpdateEventUseCase updateEventUseCase,
        DeleteEventUseCase deleteEventUseCase,
        FilterEventsUseCase filterEventsUseCase,
        GetByTitleUseCase getByTitleUseCase,
        UploadEventImageUseCase uploadEventImageUseCase,
        GetPagedEventsUseCase getPagedEventsUseCase
    )
    {
      this.createEventUseCase = createEventUseCase;
      this.getAllEventsUseCase = getAllEventsUseCase;
      this.getEventByIdUseCase = getEventByIdUseCase;
      this.updateEventUseCase = updateEventUseCase;
      this.deleteEventUseCase = deleteEventUseCase;
      this.filterEventsUseCase = filterEventsUseCase;
      this.getByTitleUseCase = getByTitleUseCase;
      this.uploadEventImageUseCase = uploadEventImageUseCase;
      this.getPagedEventsUseCase = getPagedEventsUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var result = await getAllEventsUseCase.ExecuteAsync();
      return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
      var result = await getEventByIdUseCase.ExecuteAsync(id);
      return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("filter")]
    public async Task<IActionResult> Filter(
        [FromQuery] string? category,
        [FromQuery] string? location,
        [FromQuery] DateTime? dateTime,
        [FromQuery] string? title
    )
    {
      Console.WriteLine(
          $"Received filter request - Category: {category}, Location: {location}, DateTime: {dateTime}, Title: {title}"
      );
      var events = await filterEventsUseCase.ExecuteAsync(
          category,
          location,
          dateTime,
          title
      );
      Console.WriteLine($"Found {events.Count()} events");
      return Ok(events);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventRequest createEventRequest)
    {
      var result = await createEventUseCase.ExecuteAsync(createEventRequest);
      return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
      await deleteEventUseCase.ExecuteAsync(id);
      return NoContent();
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateEventRequest updateEventRequest)
    {
      await updateEventUseCase.ExecuteAsync(updateEventRequest);
      return NoContent();
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchByTitle([FromQuery] string title)
    {
      if (string.IsNullOrWhiteSpace(title))
      {
        return BadRequest("Search title cannot be empty");
      }

      var events = await getByTitleUseCase.ExecuteAsync(title);
      return Ok(events);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("{id}/upload-image")]
    public async Task<IActionResult> UploadImage(Guid id, IFormFile file)
    {
      if (file == null || file.Length == 0)
      {
        return BadRequest("No file uploaded");
      }

      var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
      var path = Path.Combine("wwwroot", "images", "events", fileName);
      var absolutePath = Path.Combine(Directory.GetCurrentDirectory(), path);

      using (var stream = new FileStream(absolutePath, FileMode.Create))
      {
        await file.CopyToAsync(stream);
      }

      var relativePath = $"/images/events/{fileName}";
      await uploadEventImageUseCase.ExecuteAsync(id, relativePath);

      return Ok(new { imageUrl = relativePath });
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged([FromQuery] PaginationRequest request)
    {
      var result = await getPagedEventsUseCase.ExecuteAsync(request);
      return Ok(result);
    }

    [HttpGet("test-email")]
    public async Task<IActionResult> TestEmail([FromServices] INotificationService notifier)
    {
      await notifier.NotifyUsersAsync(
          new[] { "Pavel91104@gmail.com" },
          "Здравствуйте, Павел",
          "Мы будем рады видеть вас на собеседовании на эливетор."
      );

      return Ok("Письмо отправлено");
    }
  }
}
