using AutoMapper;
using MediatR;
using MyApp.Domain.Common;
using MyApp.Domain.Interfaces_store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Features.Categories.Commands
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, SpResponse>
    {
        private readonly ICategoryRepository_store _repository;
        private readonly IMapper _mapper;
        public UpdateProductCommandHandler(ICategoryRepository_store categoryRepository , IMapper mapper)
        {
            _repository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<SpResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var existingCategory = await _repository.GetByIdAsync(request.Id);
            if (existingCategory == null)
            {
                return new SpResponse { Success = false, Message = "Category not found." };
            }
            _mapper.Map(request.Category, existingCategory);
            return await _repository.UpdateAsync(existingCategory);
        }
    }
}
