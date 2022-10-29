using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using System.Text;
using System.Text.Json;

namespace PlatformService.SyncDataServices.Http
{
    public class CommandsDataClient : ICommandsDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CommandsDataClient(HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendToPlatform(PlatformReadDto plat)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(plat),
                Encoding.UTF8, 
                "application/json");

            var response = await _httpClient.PostAsync($"{_configuration["CommandsService"]}/api/commands/Platforms", httpContent);
            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync post was ok");
            }
            else
            {
                Console.WriteLine("--> Sync post was not ok");
            }  


        }
    }
}
