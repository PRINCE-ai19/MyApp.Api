using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Model_DTO
{
    public class Product_DTO
    {
        [Required(ErrorMessage = "Product_Name_Required")]
        [MaxLength(200, ErrorMessage = "Product_Name_Length")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Product_Price_Required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Product_Price_Range")]
        public decimal Price { get; set; }

        [MaxLength(500, ErrorMessage = "Product_Desc_Length")]
        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Product_Stock_Range")]
        public int? StockQuantity { get; set; }

        [Required(ErrorMessage = "Product_Category_Required")]
        public int? CategoryId { get; set; }

        public string CreatedAt { get; set; }

        public string? Img { get; set; }

        [Required(ErrorMessage = "Product_Code_Required")]
        public string? Code { get; set; }
    }
}
