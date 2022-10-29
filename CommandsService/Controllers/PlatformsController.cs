using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/commands/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private ICommandRepository _commandRepository;
        private readonly IMapper _mapper;

        public PlatformsController(
            ICommandRepository commandRepository,
            IMapper mapper)
        {
            _commandRepository = commandRepository;
            _mapper = mapper;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting platforms from Command Service");
            var platformItems = _commandRepository.GetAllPLatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound Post # Command Service");
            return Ok("Inbound test ok");
        }
    }
}
