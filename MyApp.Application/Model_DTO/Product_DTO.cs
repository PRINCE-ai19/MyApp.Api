using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Model_DTO
{
    public class Product_DTO
    {
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public string? Description { get; set; }

        public int? StockQuantity { get; set; }

        public int? CategoryId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
