using EventWebApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventWebApp.Application.UseCases.Event
{
    public class UploadEventImageUseCase
    {
        private readonly IEventRepository eventRepository;

        public UploadEventImageUseCase(IEventRepository eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        public async Task ExecuteAsync(Guid evId, string relativeImagePath)
        {
            await eventRepository.UpdateImageAsync(evId, relativeImagePath);
        }
    }

}
