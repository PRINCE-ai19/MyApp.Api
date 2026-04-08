using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Model_DTO
{
    public class CategoryUpdateDto
    {
        [Required(ErrorMessage = "NameRequired")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "NameLength")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "CodeRequired")]
        public string code { get; set; }
    }
}
