using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Model_DTO
{
    public class CategoryUpdateDto
    {
        public int Id { get; set; } // Cần Id để biết sửa cái nào
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
