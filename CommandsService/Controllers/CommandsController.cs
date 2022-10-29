using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/commands/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private ICommandRepository _commandRepository;
        private readonly IMapper _mapper;

        public CommandsController(
            ICommandRepository commandRepository,
            IMapper mapper)
        {
            _commandRepository = commandRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Getting Commands for Platform {platformId} from Command Service");
            if(!_commandRepository.PlatformExists(platformId))
                return NotFound();
            
            var commands = _commandRepository.GetCommandsForPlatform(platformId);
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> Getting Command {commandId} for Platform {platformId} from Command Service");
            if (!_commandRepository.PlatformExists(platformId))
                return NotFound();

            var command = _commandRepository.GetCommand(platformId, commandId);
            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
        {
            Console.WriteLine($"--> Create Command for Platform {platformId} from Command Service");
            if (!_commandRepository.PlatformExists(platformId))
                return NotFound();

            var command = _mapper.Map<Command>(commandDto);
            _commandRepository.CreateCommand(platformId, command);
            _commandRepository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);
            return CreatedAtRoute(nameof(GetCommandForPlatform), new { platformId = commandReadDto.PlatformID, commandId = commandReadDto.Id }, commandReadDto);
        }
    }
}
