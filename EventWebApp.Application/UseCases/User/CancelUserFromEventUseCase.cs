using EventWebApp.Application.Interfaces;

namespace EventWebApp.Application.UseCases.User
{
    public class CancelUserFromEventUseCase
    {
        private readonly IUserRepository userRepository;
        private readonly IEventRepository eventRepository;

        public CancelUserFromEventUseCase(
            IUserRepository userRepository,
            IEventRepository eventRepository
        )
        {
            this.userRepository = userRepository;
            this.eventRepository = eventRepository;
        }

        public async Task ExecuteAsync(Guid userId, Guid eventId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            var ev = await eventRepository.GetByIdAsync(eventId);

            if (user == null || ev == null)
            {
                throw new Exception("Пользователь или событие не найдено");
            }

            if (!ev.Users.Any(u => u.Id == userId))
            {
                throw new Exception("Пользователь не зарегистрирован на событие");
            }

            ev.Users.Remove(user);
            await eventRepository.UpdateAsync(ev);
        }
    }
}
