using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using System.Text.Json;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
                default:
                    break;
            }
               
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determine Event...");
            var enventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            
            switch(enventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine($"--> Event Platform_Published");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("--> Could not determine Event...");
                    return EventType.Undetermined;
            }
        }
        private void AddPlatform(string platformPublishedMessage)
        {
            using var scope= _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepository>();
            var platfromPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);
            try
            {
                var plat = _mapper.Map<Platform>(platfromPublishedDto);
                if (repo.ExternalPlatformExists(plat.ExternalId))
                {
                    Console.WriteLine($"--> Platform already exists...");
                }
                else
                {
                    repo.CreatePlatform(plat);
                    repo.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not add Platform to DB {ex.Message}");
            }
        }
    }

    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}
