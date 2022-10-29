using CommandsService.Models;
using CommandsService.SyncDataServices;

namespace CommandsService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
            var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
            var platforms = grpcClient.ReturnAllPlatforms();
            SeedData(serviceScope.ServiceProvider.GetService<ICommandRepository>(), platforms);
        }

        private static void SeedData(ICommandRepository repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("--> Seeding new Platforms...");
            foreach (var platform in platforms)
            {
                if (!repo.ExternalPlatformExists(platform.ExternalId))
                {
                    repo.CreatePlatform(platform);
                }
                repo.SaveChanges();
            }
        }
    }
}
