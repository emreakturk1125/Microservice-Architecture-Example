using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient httpClient,IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public string SendPlatformCommand()
        {
            // var httpContent = new StringContent(
            //     JsonSerializer.Serialize(item),
            //     Encoding.UTF8,
            //     "application/json"
            // );

            var response =  _httpClient.GetAsync($"{_configuration["CommandService"]}");
            var contents =  response.Result.Content.ReadAsStringAsync().Result;
            if(response.Result.IsSuccessStatusCode)
            {
                Console.WriteLine($"--> Sync GET to CommandService was OK! - {contents} ");
                return contents;
            }
            
             Console.WriteLine("--> Sync GET to CommandService was NOT OK!");
             return "No Content";
        }
    }
}
