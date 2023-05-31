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
            var products = _appDbContext.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductType);
            return await products.ToArrayAsync();
        }

        public async Task<ProductType[]> GetAllProductTypesAsync()
        {
            var productTypes = _appDbContext.ProductTypes;
            return await productTypes.ToArrayAsync();
        }

        public async Task<Brand[]> GetAllBrandsAsync()
        {
            var brands = _appDbContext.Brands;
            return await brands.ToArrayAsync();
        }
    }
}
