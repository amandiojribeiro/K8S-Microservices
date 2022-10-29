using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatfromRepository : IPlatfromRepository
    {
        private readonly AppDbContext _context;
        public PlatfromRepository(AppDbContext context)
        {
            _context = context;
        }

        public Platform CreatePlatform(Platform platform)
        {
            if (platform == null)
                throw new ArgumentNullException(nameof(Platform));

            _context.Add(platform);
            return platform;
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Platform GetPlatformById(int id)
        {
            return _context.Platforms.FirstOrDefault(x=>x.Id == id);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
