using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatfromRepository _platfromRepository;
        private readonly IMapper _mapper;
        private readonly ICommandsDataClient _commandsDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformsController(
            IPlatfromRepository platfromRepository,
            IMapper mapper,
            ICommandsDataClient commandsDataClient,
            IMessageBusClient messageBusClient)
        {
            _platfromRepository = platfromRepository;
            _mapper = mapper;
            _commandsDataClient = commandsDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            var platformItems = _platfromRepository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpGet("{id}", Name="GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platformItem = _platfromRepository.GetPlatformById(id);
            return Ok(_mapper.Map<PlatformReadDto>(platformItem));
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform([FromBody] PlatformCreateDto platformCreateDto)
        {
            var platformModel = _platfromRepository.CreatePlatform(_mapper.Map<Platform>(platformCreateDto));
            _platfromRepository.SaveChanges();
            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            try
            {
                await _commandsDataClient.SendToPlatform(platformReadDto);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Could not send synchronously: {ex.Message}");
            }

            try
            {
                var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
                platformPublishedDto.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(platformPublishedDto);

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Could not send Asynchronously: {ex.Message}");
            }



            return CreatedAtRoute(nameof(GetPlatformById), new { id = platformReadDto.Id }, platformReadDto);

        }
    }
}
