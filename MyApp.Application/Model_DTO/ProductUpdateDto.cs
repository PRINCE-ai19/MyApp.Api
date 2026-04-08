using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Model_DTO
{
    public class ProductUpdateDto
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống babe ơi!")]
        [MaxLength(200, ErrorMessage = "Tên gì mà dài thế, dưới 200 ký tự thôi nè.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phải có giá chứ babe.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0 nha.")]
        public decimal Price { get; set; }

        [MaxLength(500, ErrorMessage = "Mô tả ngắn gọn thôi, tối đa 500 ký tự.")]
        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng không được âm đâu.")]
        public int? StockQuantity { get; set; }

        [Required(ErrorMessage = "Sản phẩm phải thuộc về một danh mục nào đó.")]
        public int? CategoryId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? Img { get; set; }

        [Required(ErrorMessage = "Mã Code là bắt buộc để quản lý.")]
        public string? Code { get; set; }
    }
}
