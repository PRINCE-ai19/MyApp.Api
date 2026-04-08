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
        [Required(ErrorMessage = "Tên s?n ph?m không ???c ?? tr?ng babe ?i!")]
        [MaxLength(200, ErrorMessage = "Tên gì mà dài th?, d??i 200 ký t? thôi nè.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Ph?i có giá ch? babe.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá ph?i l?n h?n 0 nha.")]
        public decimal Price { get; set; }

        [MaxLength(500, ErrorMessage = "Mô t? ng?n g?n thôi, t?i ?a 500 ký t?.")]
        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "S? l??ng không ???c âm ?âu.")]
        public int? StockQuantity { get; set; }

        [Required(ErrorMessage = "S?n ph?m ph?i thu?c v? m?t danh m?c nào ?ó.")]
        public int? CategoryId { get; set; }

        public string CreatedAt { get; set; }

        public string? Img { get; set; }
        [Required(ErrorMessage = "Mã Code là b?t bu?c ?? qu?n lý.")]
        public string? Code { get; set; }
    }
}
