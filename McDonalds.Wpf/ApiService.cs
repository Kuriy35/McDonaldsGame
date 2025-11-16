using System.Net.Http;
using System.Net.Http.Json;
using McDonalds.ViewModels.Api;
using McDonalds.Models.Core;

namespace McDonalds.Wpf.Services
{
    public class ApiService
    {
        private readonly HttpClient _client;

        public ApiService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<ResourceApiViewModel>> GetResourcesAsync()
        {
            var response = await _client.GetAsync("api/resources");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ResourceApiViewModel>>() ?? new();
        }

        public async Task<ResourceApiViewModel?> GetResourceAsync(string name)
        {
            var response = await _client.GetAsync($"api/resources/{name}");
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<ResourceApiViewModel>();
        }

        public async Task UpdateResourceAsync(ResourceApiViewModel resource)
        {
            var response = await _client.PutAsJsonAsync($"api/resources/{resource.Name}", resource);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<ResourceApiViewModel>> GetDifficultyResourcesAsync(GameDifficulty difficulty)
        {
            var response = await _client.GetAsync($"api/difficultyresources/{difficulty}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ResourceApiViewModel>>() ?? new();
        }
    }
}