using McDonalds.Models.Orders;
using McDonalds.ViewModels.Api;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using McDonalds.Wpf;
using McDonalds.Wpf.Services;

namespace McDonalds.Models.Core
{
    public class ResourceManager
    {
        private static readonly Lazy<ResourceManager> _lazy = new(() =>
             App.Services.GetRequiredService<ResourceManager>()); // ← Тепер через DI!

        public static ResourceManager Instance => _lazy.Value;

        private readonly ApiService _apiService;
        private readonly IMapper _mapper;
        private readonly Dictionary<string, Resource> _cache = new();

        public event Action<string, int>? ResourceQuantityChanged;

        // КОНСТРУКТОР ТІЛЬКИ ДЛЯ DI
        public ResourceManager(ApiService apiService, IMapper mapper)
        {
            _apiService = apiService;
            _mapper = mapper;
        }

        public async Task InitializeAsync()
        {
            var apiResources = await _apiService.GetResourcesAsync();
            _cache.Clear();
            foreach (var apiRes in apiResources)
            {
                var resource = _mapper.Map<Resource>(apiRes); // ← МАПЕР!
                _cache[resource.Name] = resource;
            }
        }

        public async Task BuyResource(string name, int quantity)
        {
            if (_cache.TryGetValue(name, out var resource))
            {
                resource.Quantity += quantity;
                var apiModel = _mapper.Map<ResourceApiViewModel>(resource);
                await _apiService.UpdateResourceAsync(apiModel);
                ResourceQuantityChanged?.Invoke(name, resource.Quantity);
            }
        }

        public async Task SellResource(string name, int quantity)
        {
            if (_cache.TryGetValue(name, out var resource) && resource.Quantity >= quantity)
            {
                resource.Quantity -= quantity;
                var apiModel = _mapper.Map<ResourceApiViewModel>(resource);
                await _apiService.UpdateResourceAsync(apiModel);
                ResourceQuantityChanged?.Invoke(name, resource.Quantity);
            }
        }

        public bool HasResources(Product product)
        {
            if (product?.RequiredResources == null) return false;
            foreach (var (resName, required) in product.RequiredResources)
            {
                if (!_cache.TryGetValue(resName, out var res) || res.Quantity < required)
                    return false;
            }
            return true;
        }

        public async Task ConsumeResources(Product product)
        {
            if (product?.RequiredResources == null) return;
            foreach (var (resName, required) in product.RequiredResources)
            {
                if (_cache.TryGetValue(resName, out var res))
                {
                    res.Quantity -= required;
                    var apiModel = _mapper.Map<ResourceApiViewModel>(res);
                    await _apiService.UpdateResourceAsync(apiModel);
                    ResourceQuantityChanged?.Invoke(resName, res.Quantity);
                }
            }
        }

        public async Task SetResourcesByDifficulty(GameDifficulty difficulty)
        {
            var apiResources = await _apiService.GetDifficultyResourcesAsync(difficulty);
            _cache.Clear();
            foreach (var apiRes in apiResources)
            {
                var resource = _mapper.Map<Resource>(apiRes);
                _cache[resource.Name] = resource;
                ResourceQuantityChanged?.Invoke(resource.Name, resource.Quantity);
            }
        }

        public List<Resource> GetResourcesFromCache()
        {
            return _cache.Values.ToList();
        }

        public Resource? GetResourceByName(string name)
        {
            _cache.TryGetValue(name, out var resource);
            return resource;
        }
    }
}