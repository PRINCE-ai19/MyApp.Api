using MediatR;
using MyApp.Application.Model_DTO;
using MyApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Features.Categories.Commands
{
    public record CreateCategoryCommand(Category_DTO Category) : IRequest<SpResponse>;

}
