using McDonalds.Data;
using McDonalds.Models;
using McDonalds.Models.Core;

namespace McDonalds.Repositories
{
    public class ResourceRepository
    {
        private readonly McDonaldsContext _context;

        public ResourceRepository(McDonaldsContext context)
        {
            _context = context;
        }
        public IQueryable<Resource> GetAll() => _context.Resources;

        public Resource GetByName(string name) {
            var res = _context.Resources.FirstOrDefault(r => r.Name == name);
            if (res != null)
            {
                return res;
            }
            return null!;
        }

        public void Add(string name, int quantity, decimal buyPrice, decimal sellPrice)
        {
            _context.Resources.Add(new Resource
            {
                Name = name,
                Quantity = quantity,
                BuyPrice = buyPrice,
                SellPrice = sellPrice
            });
            _context.SaveChanges();
        }

        public void UpdatePrices(string name, decimal buyPrice, decimal sellPrice)
        {
            var res = GetByName(name);
            if (res != null)
            {
                res.BuyPrice = buyPrice;
                res.SellPrice = sellPrice;
                _context.SaveChanges();
            }
        }

        public void UpdateQuantity(string name, int newQuantity)
        {
            var res = _context.Resources.FirstOrDefault(r => r.Name == name);
            if (res != null)
            {
                res.Quantity = newQuantity;
            }
            else
            {
                var difficultyRes = _context.DifficultyResources
                    .Where(dr => dr.ResourceName == name && dr.Difficulty == GameDifficulty.Easy)
                    .FirstOrDefault();
                
                if (difficultyRes != null)
                {
                    _context.Resources.Add(new Resource 
                    { 
                        Name = name, 
                        Quantity = newQuantity,
                        BuyPrice = difficultyRes.BuyPrice > 0 ? difficultyRes.BuyPrice : 0.01m,
                        SellPrice = difficultyRes.SellPrice > 0 ? difficultyRes.SellPrice : 0.01m
                    });
                }
                else
                {
                    _context.Resources.Add(new Resource 
                    { 
                        Name = name, 
                        Quantity = newQuantity,
                        BuyPrice = 0.01m,
                        SellPrice = 0.01m
                    });
                }
            }
            _context.SaveChanges();
        }

        public void UpdateResource(string name, int quantity, decimal buyPrice, decimal sellPrice)
        {
            var res = _context.Resources.FirstOrDefault(r => r.Name == name);
            if (res != null)
            {
                res.Quantity = quantity;
                res.BuyPrice = buyPrice > 0 ? buyPrice : 0.01m;
                res.SellPrice = sellPrice > 0 ? sellPrice : 0.01m;
            }
            else
            {
                _context.Resources.Add(new Resource
                {
                    Name = name,
                    Quantity = quantity,
                    BuyPrice = buyPrice > 0 ? buyPrice : 0.01m,
                    SellPrice = sellPrice > 0 ? sellPrice : 0.01m
                });
            }
            _context.SaveChanges();
        }

        public void Delete(string name)
        {
            var res = GetByName(name);
            if (res != null)
            {
                _context.Resources.Remove(res);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var res = _context.Resources.FirstOrDefault(r => r.Id == id);
            if (res != null)
            {
                _context.Resources.Remove(res);
                _context.SaveChanges();
            }
        }

        public int Consume(string name, int amount)
        {
            var res = _context.Resources.FirstOrDefault(r => r.Name == name);
            if (res != null && res.Quantity >= amount)
            {
                res.Quantity -= amount;
                _context.SaveChanges();
                return res.Quantity;
            }
            return -1;
        }

        public List<DifficultyResource> GetDifficultyResources(GameDifficulty difficulty)
        {
            return _context.DifficultyResources
                .Where(dr => dr.Difficulty == difficulty)
                .ToList();
        }
    }
}