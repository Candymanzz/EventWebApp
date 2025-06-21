using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;

namespace EventWebApp.Application.UseCases.User
{
    public class GetUserByIdUseCase
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetUserByIdUseCase(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<UserDto?> ExecuteAsync(Guid id)
        {
            var user = await userRepository.GetByIdAsync(id);
            return user == null ? null : mapper.Map<UserDto>(user);
        }
    }
}
