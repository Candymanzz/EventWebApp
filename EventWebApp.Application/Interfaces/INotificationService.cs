namespace EventWebApp.Application.Interfaces
{
  public interface INotificationService
  {
    Task NotifyUsersAsync(IEnumerable<string> userEmails, string subject, string message, CancellationToken cancellationToken = default);
  }
}
