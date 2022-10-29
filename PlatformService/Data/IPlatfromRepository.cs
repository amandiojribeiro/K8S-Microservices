using PlatformService.Models;

namespace PlatformService.Data
{
    public interface IPlatfromRepository
    {
        bool SaveChanges();
        IEnumerable<Platform> GetAllPlatforms();
        Platform GetPlatformById(int id);
        Platform CreatePlatform(Platform platform);
    }
}
