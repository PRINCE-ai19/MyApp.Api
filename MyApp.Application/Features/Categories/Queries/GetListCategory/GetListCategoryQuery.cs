using MediatR;
using MyApp.Application.Model_DTO;
using System.Collections.Generic;

namespace MyApp.Application.Features.Categories.Queries.GetListCategory
{
    public record GetListCategoryQuery() : IRequest<IEnumerable<Category_DTO>>;

}
