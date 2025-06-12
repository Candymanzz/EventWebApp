using EventWebApp.Application.Interfaces;

namespace EventWebApp.Application.UseCases.User
{
    public class CancelUserFromEventUseCase
    {
        private readonly IUserRepository userRepository;

        public CancelUserFromEventUseCase(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task ExecuteAsync(Guid uId, Guid evId)
        {
            await userRepository.CancelUserFromEvent(uId, evId);
        }
    }
}
