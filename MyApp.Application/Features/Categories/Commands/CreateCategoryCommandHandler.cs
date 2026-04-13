using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using MyApp.Application.Model_DTO;
using MyApp.Application.Resources;
using MyApp.Domain.Common;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Interfaces_store;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Features.Categories.Commands
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, SpResponse>
    {
        private readonly ICategoryRepository_store _repository;

        private readonly IMapper _mapper;

        private readonly IStringLocalizer<SharedResource> _localizer;
        public CreateCategoryCommandHandler(ICategoryRepository_store categoryRepo , IMapper mapper , IStringLocalizer<SharedResource> localizer)
        {
            _repository = categoryRepo;
            _mapper = mapper;
            _localizer = localizer;

        }

        public async Task<SpResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var newCategory = _mapper.Map<Category>(request.Category);
             return await _repository.AddAsync(newCategory); 
        }
    }
}
