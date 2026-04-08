using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Model_DTO
{
    public class Category_DTO
    {
      
        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên phải từ 3 đến 100 ký tự")]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Mã Code không được để trống.")]
        public string code { get; set; } = null!;
    }
}
