using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using McDonalds.ViewModels.Api;

public class ApiService
{
    private readonly HttpClient _client;
    public ApiService(IConfiguration config)
    {
        _client = new HttpClient { BaseAddress = new Uri(config["ApiUrl"]) };
    }

    public async Task<List<ResourceApiViewModel>> GetResources()
    {
        var response = await _client.GetAsync("api/resources");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<ResourceApiViewModel>>();
    }
}
