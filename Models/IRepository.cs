namespace u21442453_HW03_API.Models
{
    public interface IRepository
    {
        Task<bool> SaveChangesAsync();
        
        void Add<T>(T entity) where T : class;
        Task<Product[]> GetAllProductsAsync();
    }
}
