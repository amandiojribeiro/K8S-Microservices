using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http
{
    public interface ICommandsDataClient
    {
        Task SendToPlatform(PlatformReadDto plat);
    }
}
