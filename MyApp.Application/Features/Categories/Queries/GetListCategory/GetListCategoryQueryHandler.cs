using AutoMapper;
using MediatR;
using MyApp.Application.Model_DTO;
using MyApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp.Application.Features.Categories.Queries.GetListCategory
{
    public class GetListCategoryQueryHandler : IRequestHandler<GetListCategoryQuery, IEnumerable<Category_DTO>>
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;

        public GetListCategoryQueryHandler(ICategoryRepository categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Category_DTO>> Handle(GetListCategoryQuery request, CancellationToken cancellationToken)
        {
            var result = await _categoryRepo.GetAllCategoriesAsync();
            return _mapper.Map<IEnumerable<Category_DTO>>(result);
        }


    }



}
