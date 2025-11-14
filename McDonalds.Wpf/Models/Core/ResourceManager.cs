using McDonalds.Models.Orders;
using McDonalds.Repositories;
using Microsoft.Extensions.DependencyInjection;
using McDonalds.Wpf;

namespace McDonalds.Models.Core
{
    public class ResourceManager
    {

        private static ResourceManager? _instance;
        public static ResourceManager Instance => _instance ??= CreateInstance();

        private readonly ResourceRepository _repo;

        public static event Action<string, int> ResourceQuantityChanged = delegate { };

        private ResourceManager() : this(null!) { }

        private ResourceManager(ResourceRepository repo)
        {
            _repo = repo;
        }

        private static ResourceManager CreateInstance()
        {
            var repo = App.Services.GetRequiredService<ResourceRepository>();
            return new ResourceManager(repo);
        }

        public void SetResourcesByDifficulty(GameDifficulty difficulty)
        {
            var baseResources = _repo.GetDifficultyResources(difficulty);
            foreach (var br in baseResources)
            {
                _repo.UpdateResource(br.ResourceName, br.BaseQuantity, br.BuyPrice, br.SellPrice);
            }
        }

        public bool HasResources(Product product)
        {
            if (product == null || product.RequiredResources == null)
                return false;

            foreach (var kvp in product.RequiredResources)
            {
                var res = _repo.GetByName(kvp.Key);
                if (res == null || res.Quantity < kvp.Value)
                    return false;
            }
            return true;
        }

        public void ConsumeResources(Product product)
        {
            if (product == null || product.RequiredResources == null)
                return;

            foreach (var kvp in product.RequiredResources)
            {
                int newQuantity = _repo.Consume(kvp.Key, kvp.Value);
                if (newQuantity >= 0)
                {
                    ResourceQuantityChanged?.Invoke(kvp.Key, newQuantity);
                }
            }
        }

        public void BuyResource(string name, int quantity)
        {
            var res = _repo.GetByName(name);
            if (res != null)
            {
                res.Quantity += quantity;
                _repo.UpdateQuantity(name, res.Quantity);
                ResourceQuantityChanged?.Invoke(name, res.Quantity);
            }
        }

        public void SellResource(string name, int quantity)
        {
            var res = _repo.GetByName(name);
            if (res != null && res.Quantity >= quantity)
            {
                res.Quantity -= quantity;
                if (res.Quantity <= 0)
                {
                    res.Quantity = 0;
                }
                _repo.UpdateQuantity(name, res.Quantity);
                ResourceQuantityChanged?.Invoke(name, res.Quantity);
            }
        }
    }
}