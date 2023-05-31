using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace u21442453_HW03_API.Models
{
    public class Product : BaseEntity
    {
        public int? ProductId { get; set; }

        public decimal Price { get; set; }

        public string? Image { get; set; }
        public int BrandId { get; set; }
        public int ProductTypeId { get; set; }

        public ProductType? ProductType { get; set; }
        public Brand? Brand { get; set; }
    }
}
