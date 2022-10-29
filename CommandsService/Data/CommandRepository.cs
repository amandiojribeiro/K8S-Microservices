using CommandsService.Models;

namespace CommandsService.Data
{
    public class CommandRepository : ICommandRepository
    {
        private readonly AppDbContext _appDbContext;

        public CommandRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public bool ExternalPlatformExists(int externalId)
        {
            return _appDbContext.Platforms.Any(p => p.ExternalId == externalId);
        }

        public void CreateCommand(int platformId, Command command)
        {
            if(command == null)
                throw new ArgumentNullException(nameof(command));

            command.PlatformID = platformId;
            _appDbContext.Commands.Add(command);
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
                throw new ArgumentNullException(nameof(platform));

            _appDbContext.Platforms.Add(platform);

        }

        public IEnumerable<Platform> GetAllPLatforms()
        {
            return _appDbContext.Platforms.ToList();
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _appDbContext.Commands
                .Where(c => c.PlatformID == platformId && c.Id == commandId).FirstOrDefault();
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _appDbContext.Commands
                .Where(c => c.PlatformID == platformId)
                .OrderBy(c => c.Platform.Name);
        }

        public bool PlatformExists(int platformId)
        {
            return _appDbContext.Platforms.Any(p => p.Id == platformId);
        }

        public bool SaveChanges()
        {
            return (_appDbContext.SaveChanges() >= 0);
        }
    }
}
