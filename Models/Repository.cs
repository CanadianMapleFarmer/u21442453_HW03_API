using Microsoft.EntityFrameworkCore;

namespace u21442453_HW03_API.Models
{
    public class Repository:IRepository
    {
        private readonly AppDbContext _appDbContext;

        public Repository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Add<T>(T entity) where T : class
        {
            _appDbContext.Add(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _appDbContext.SaveChangesAsync() > 0;
        }

        public async Task<Product[]> GetAllProductsAsync()
        {
            var products = _appDbContext.Products;
            return await products.ToArrayAsync();
        }
    }
}
