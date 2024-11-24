using Basket.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Basket.Commands
{
    public class DeleteShoppingCartCommand : IRequest<Unit>
    {
        public string UserName { get; set; }

        public DeleteShoppingCartCommand(string userName)
        {
            UserName = userName;
        }
    }

    public class DeleteShoppingCartCommandHandler : IRequestHandler<DeleteShoppingCartCommand, Unit>
    {
        private readonly IBasketRepository _basketRepository;

        public DeleteShoppingCartCommandHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<Unit> Handle(DeleteShoppingCartCommand request, CancellationToken cancellationToken)
        {
            await _basketRepository.Delete(request.UserName, cancellationToken);
            return Unit.Value;
        }
    }
}
