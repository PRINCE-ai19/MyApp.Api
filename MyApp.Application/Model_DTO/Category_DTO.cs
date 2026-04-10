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

       // [Required(ErrorMessage = "NameRequired")]
     
       //[StringLength(100, MinimumLength = 3, ErrorMessage = "NameLength")]
      
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "CodeRequired")]
        public string code { get; set; } = null!;

    }
}
