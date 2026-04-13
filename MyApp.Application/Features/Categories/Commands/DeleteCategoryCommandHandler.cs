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
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommad, SpResponse>
    {
        private readonly ICategoryRepository_store _repository;

        public DeleteCategoryCommandHandler(ICategoryRepository_store repository)
        {
            _repository = repository;
        }

        public async Task<SpResponse> Handle(DeleteCategoryCommad request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteAsync(request.Id);
        }
    }

}
