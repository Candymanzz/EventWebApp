using EventWebApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventWebApp.Application.UseCases.User
{
    public class RegisterUserToEventUseCase
    {
        private readonly IUserRepository userRepository;

        public RegisterUserToEventUseCase(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task ExecuteAsync(Guid uId, Guid evId)
        {
            await userRepository.RegisterUserToEventAsync(uId, evId);
        }
    }
}
