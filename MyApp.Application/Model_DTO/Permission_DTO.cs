using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Model_DTO
{
    public class Permission_DTO
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? CreatedPer { get; set; }
    }
}
