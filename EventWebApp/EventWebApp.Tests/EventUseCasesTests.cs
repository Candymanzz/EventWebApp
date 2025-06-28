using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Core.Interfaces;
using EventWebApp.Application.UseCases.Event;
using EventWebApp.Core.Model;
using EventWebApp.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace EventWebApp.Tests
{
  public class EventUseCasesTests
  {
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IEventRepository> _mockEventRepository;
    private readonly Mock<IValidator<CreateEventRequest>> _mockCreateValidator;
    private readonly Mock<IValidator<UpdateEventRequest>> _mockUpdateValidator;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateEventUseCase _createEventUseCase;
    private readonly GetEventByIdUseCase _getEventByIdUseCase;
    private readonly UpdateEventUseCase _updateEventUseCase;
    private readonly DeleteEventUseCase _deleteEventUseCase;
    private readonly GetAllEventsUseCase _getAllEventsUseCase;
    private readonly GetByTitleUseCase _getByTitleUseCase;
    private readonly FilterEventsUseCase _filterEventsUseCase;
    private readonly GetPagedEventsUseCase _getPagedEventsUseCase;
    private readonly UploadEventImageUseCase _uploadEventImageUseCase;

    public EventUseCasesTests()
    {
      _mockUnitOfWork = new Mock<IUnitOfWork>();
      _mockEventRepository = new Mock<IEventRepository>();
      _mockCreateValidator = new Mock<IValidator<CreateEventRequest>>();
      _mockUpdateValidator = new Mock<IValidator<UpdateEventRequest>>();
      _mockMapper = new Mock<IMapper>();

      // Настраиваем UnitOfWork для возврата mock EventRepository
      _mockUnitOfWork.Setup(uow => uow.Events).Returns(_mockEventRepository.Object);

      _createEventUseCase = new CreateEventUseCase(
          _mockUnitOfWork.Object,
          _mockCreateValidator.Object,
          _mockMapper.Object
      );

      _getEventByIdUseCase = new GetEventByIdUseCase(
          _mockUnitOfWork.Object,
          _mockMapper.Object
      );

      _updateEventUseCase = new UpdateEventUseCase(
          _mockUnitOfWork.Object,
          _mockUpdateValidator.Object,
          _mockMapper.Object
      );

      _deleteEventUseCase = new DeleteEventUseCase(_mockUnitOfWork.Object);

      _getAllEventsUseCase = new GetAllEventsUseCase(
          _mockUnitOfWork.Object,
          _mockMapper.Object
      );

      _getByTitleUseCase = new GetByTitleUseCase(
          _mockUnitOfWork.Object,
          _mockMapper.Object
      );

      _filterEventsUseCase = new FilterEventsUseCase(
          _mockUnitOfWork.Object,
          _mockMapper.Object
      );

      _getPagedEventsUseCase = new GetPagedEventsUseCase(
          _mockUnitOfWork.Object,
          _mockMapper.Object
      );

      _uploadEventImageUseCase = new UploadEventImageUseCase(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task CreateEvent_ValidRequest_ReturnsEventDto()
    {
      // Arrange
      var request = new CreateEventRequest
      {
        Title = "Test Event",
        Description = "Test Description",
        DateTime = DateTime.UtcNow,
        Location = "Test Location",
        Category = "Test Category",
        MaxParticipants = 100,
      };

      var validationResult = new FluentValidation.Results.ValidationResult();
      _mockCreateValidator
          .Setup(v => v.ValidateAsync(It.IsAny<CreateEventRequest>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(validationResult);

      var eventEntity = new Core.Model.Event
      {
        Id = Guid.NewGuid(),
        Title = request.Title,
        Description = request.Description,
        DateTime = request.DateTime,
        Location = request.Location,
        Category = request.Category,
        MaxParticipants = request.MaxParticipants,
      };

      var expectedDto = new EventDto
      {
        Id = eventEntity.Id,
        Title = eventEntity.Title,
        Description = eventEntity.Description,
        DateTime = eventEntity.DateTime,
        Location = eventEntity.Location,
        Category = eventEntity.Category,
        MaxParticipants = eventEntity.MaxParticipants,
      };

      _mockMapper.Setup(m => m.Map<Core.Model.Event>(request)).Returns(eventEntity);
      _mockMapper.Setup(m => m.Map<EventDto>(eventEntity)).Returns(expectedDto);
      _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

      // Act
      var result = await _createEventUseCase.ExecuteAsync(request);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(expectedDto.Id, result.Id);
      Assert.Equal(expectedDto.Title, result.Title);
      _mockEventRepository.Verify(r => r.AddAsync(It.IsAny<Core.Model.Event>(), It.IsAny<CancellationToken>()), Times.Once);
      _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateEvent_InvalidRequest_ThrowsValidationException()
    {
      // Arrange
      var request = new CreateEventRequest();
      var validationResult = new FluentValidation.Results.ValidationResult(
          new[] { new ValidationFailure("Title", "Title is required") }
      );

      _mockCreateValidator
          .Setup(v => v.ValidateAsync(It.IsAny<CreateEventRequest>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(validationResult);

      // Act & Assert
      await Assert.ThrowsAsync<FluentValidation.ValidationException>(
          () => _createEventUseCase.ExecuteAsync(request)
      );
    }

    [Fact]
    public async Task GetEventById_ExistingEvent_ReturnsEventDto()
    {
      // Arrange
      var eventId = Guid.NewGuid();
      var eventEntity = new Core.Model.Event
      {
        Id = eventId,
        Title = "Test Event",
        Description = "Test Description",
        DateTime = DateTime.UtcNow,
        Location = "Test Location",
        Category = "Test Category",
        MaxParticipants = 100,
      };

      var expectedDto = new EventDto
      {
        Id = eventEntity.Id,
        Title = eventEntity.Title,
        Description = eventEntity.Description,
        DateTime = eventEntity.DateTime,
        Location = eventEntity.Location,
        Category = eventEntity.Category,
        MaxParticipants = eventEntity.MaxParticipants,
      };

      _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, It.IsAny<CancellationToken>())).ReturnsAsync(eventEntity);
      _mockMapper.Setup(m => m.Map<EventDto>(eventEntity)).Returns(expectedDto);

      // Act
      var result = await _getEventByIdUseCase.ExecuteAsync(eventId);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(expectedDto.Id, result.Id);
      Assert.Equal(expectedDto.Title, result.Title);
    }

    [Fact]
    public async Task GetEventById_NonExistingEvent_ReturnsNull()
    {
      // Arrange
      var eventId = Guid.NewGuid();
      _mockEventRepository
          .Setup(r => r.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
          .ReturnsAsync((Core.Model.Event?)null);

      // Act
      var result = await _getEventByIdUseCase.ExecuteAsync(eventId);

      // Assert
      Assert.Null(result);
    }

    [Fact]
    public async Task UpdateEvent_ValidRequest_UpdatesEvent()
    {
      // Arrange
      var eventId = Guid.NewGuid();
      var request = new UpdateEventRequest
      {
        Id = eventId,
        Title = "Updated Event",
        Description = "Updated Description",
        DateTime = DateTime.UtcNow,
        Location = "Updated Location",
        Category = "Updated Category",
        MaxParticipants = 150,
      };

      var validationResult = new FluentValidation.Results.ValidationResult();
      _mockUpdateValidator
          .Setup(v => v.ValidateAsync(It.IsAny<UpdateEventRequest>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(validationResult);

      var existingEvent = new Core.Model.Event
      {
        Id = eventId,
        Title = "Original Event",
        Description = "Original Description",
        DateTime = DateTime.UtcNow.AddDays(-1),
        Location = "Original Location",
        Category = "Original Category",
        MaxParticipants = 100,
      };

      var updatedEvent = new Core.Model.Event
      {
        Id = eventId,
        Title = request.Title,
        Description = request.Description,
        DateTime = request.DateTime,
        Location = request.Location,
        Category = request.Category,
        MaxParticipants = request.MaxParticipants,
      };

      _mockEventRepository.Setup(r => r.GetByIdForUpdateAsync(eventId, It.IsAny<CancellationToken>())).ReturnsAsync(existingEvent);
      _mockMapper.Setup(m => m.Map<Core.Model.Event>(request)).Returns(updatedEvent);
      _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

      // Act
      await _updateEventUseCase.ExecuteAsync(request);

      // Assert
      _mockEventRepository.Verify(r => r.GetByIdForUpdateAsync(eventId, It.IsAny<CancellationToken>()), Times.Once);
      _mockEventRepository.Verify(
          r => r.UpdateAsync(It.IsAny<Core.Model.Event>(), It.IsAny<CancellationToken>()),
          Times.Once
      );
      _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteEvent_ExistingEvent_DeletesSuccessfully()
    {
      // Arrange
      var eventId = Guid.NewGuid();
      var existingEvent = new Core.Model.Event { Id = eventId, Title = "Test Event" };

      _mockEventRepository.Setup(r => r.GetByIdForUpdateAsync(eventId, It.IsAny<CancellationToken>())).ReturnsAsync(existingEvent);
      _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

      // Act
      await _deleteEventUseCase.ExecuteAsync(eventId);

      // Assert
      _mockEventRepository.Verify(r => r.DeleteAsync(eventId, It.IsAny<CancellationToken>()), Times.Once);
      _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllEvents_ReturnsAllEvents()
    {
      // Arrange
      var events = new List<Core.Model.Event>
            {
                new Core.Model.Event { Id = Guid.NewGuid(), Title = "Event 1" },
                new Core.Model.Event { Id = Guid.NewGuid(), Title = "Event 2" },
            };

      var expectedDtos = events
          .Select(e => new EventDto { Id = e.Id, Title = e.Title })
          .ToList();

      _mockEventRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(events);
      _mockMapper.Setup(m => m.Map<IEnumerable<EventDto>>(events)).Returns(expectedDtos);

      // Act
      var result = await _getAllEventsUseCase.ExecuteAsync();

      // Assert
      Assert.NotNull(result);
      Assert.Equal(2, result.Count());
      Assert.Equal(expectedDtos[0].Title, result.First().Title);
    }

    [Fact]
    public async Task GetByTitle_ExistingEvent_ReturnsEventDto()
    {
      // Arrange
      var title = "Test Event";
      var eventEntity = new Core.Model.Event { Id = Guid.NewGuid(), Title = title };

      var expectedDto = new EventDto { Id = eventEntity.Id, Title = eventEntity.Title };

      _mockEventRepository
          .Setup(r => r.GetByTitleAsync(title, It.IsAny<CancellationToken>()))
          .ReturnsAsync(new[] { eventEntity });
      _mockMapper.Setup(m => m.Map<EventDto>(eventEntity)).Returns(expectedDto);

      // Act
      var result = await _getByTitleUseCase.ExecuteAsync(title);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(expectedDto.Title, result.Title);
    }

    [Fact]
    public async Task FilterEvents_ReturnsFilteredEvents()
    {
      // Arrange
      var category = "Test Category";
      var location = "Test Location";
      var dateTime = DateTime.UtcNow;
      var title = "Test Event";

      var events = new List<Core.Model.Event>
            {
                new Core.Model.Event
                {
                    Id = Guid.NewGuid(),
                    Title = title,
                    Category = category,
                    Location = location,
                    DateTime = dateTime,
                },
            };

      var expectedDtos = events
          .Select(e => new EventDto
          {
            Id = e.Id,
            Title = e.Title,
            Category = e.Category,
            Location = e.Location,
            DateTime = e.DateTime,
          })
          .ToList();

      _mockEventRepository
          .Setup(r => r.GetByFiltersAsync(category, location, dateTime, title, It.IsAny<CancellationToken>()))
          .ReturnsAsync(events);
      _mockMapper.Setup(m => m.Map<IEnumerable<EventDto>>(events)).Returns(expectedDtos);

      // Act
      var result = await _filterEventsUseCase.ExecuteAsync(
          category,
          location,
          dateTime,
          title
      );

      // Assert
      Assert.NotNull(result);
      Assert.Single(result);
      Assert.Equal(expectedDtos[0].Title, result.First().Title);
    }

    [Fact]
    public async Task GetPagedEvents_ReturnsPagedEvents()
    {
      // Arrange
      var request = new PaginationRequest { PageNumber = 1, PageSize = 10 };
      var events = new List<Core.Model.Event>
            {
                new Core.Model.Event { Id = Guid.NewGuid(), Title = "Event 1" },
                new Core.Model.Event { Id = Guid.NewGuid(), Title = "Event 2" },
            };

      var paginatedResult = new PaginatedResult<Core.Model.Event>
      {
        Items = events,
        TotalCount = 2,
        PageNumber = request.PageNumber,
        PageSize = request.PageSize,
      };

      var expectedDtos = events
          .Select(e => new EventDto { Id = e.Id, Title = e.Title })
          .ToList();

      var expectedPaginatedResult = new PaginatedResult<EventDto>
      {
        Items = expectedDtos,
        TotalCount = 2,
        PageNumber = request.PageNumber,
        PageSize = request.PageSize,
      };

      _mockEventRepository
          .Setup(r => r.GetPagedAsync(request.PageNumber, request.PageSize, It.IsAny<CancellationToken>()))
          .ReturnsAsync(paginatedResult);
      _mockMapper.Setup(m => m.Map<IEnumerable<EventDto>>(events)).Returns(expectedDtos);

      // Act
      var result = await _getPagedEventsUseCase.ExecuteAsync(request);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(2, result.Items.Count());
      Assert.Equal(2, result.TotalCount);
      Assert.Equal(request.PageNumber, result.PageNumber);
      Assert.Equal(request.PageSize, result.PageSize);
    }

    [Fact]
    public async Task UploadEventImage_ValidImage_UpdatesEventImage()
    {
      // Arrange
      var eventId = Guid.NewGuid();
      var imageUrl = "https://example.com/image.jpg";
      var existingEvent = new Core.Model.Event
      {
        Id = eventId,
        Title = "Test Event",
        ImageUrl = "old-image.jpg"
      };

      _mockEventRepository
          .Setup(r => r.GetByIdForUpdateAsync(eventId, It.IsAny<CancellationToken>()))
          .ReturnsAsync(existingEvent);
      _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

      var useCase = new UploadEventImageUseCase(_mockUnitOfWork.Object);

      // Act
      await useCase.ExecuteAsync(eventId, imageUrl);

      // Assert
      _mockEventRepository.Verify(r => r.GetByIdForUpdateAsync(eventId, It.IsAny<CancellationToken>()), Times.Once);
      _mockEventRepository.Verify(r => r.UpdateAsync(It.Is<Core.Model.Event>(e =>
          e.Id == eventId && e.ImageUrl == imageUrl), It.IsAny<CancellationToken>()), Times.Once);
      _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
  }
}
