using System.Threading;
using System.Threading.Tasks;
using BaseProject.Core.Messages;
using BaseProject.Seller.API.Data.Repository;
using FluentValidation.Results;
using MediatR;

namespace BaseProject.Seller.API.Application.Commands
{
    public class SellerCommandHandler : CommandHandler,
        IRequestHandler<AddSellerCommand, ValidationResult>
    {
        private readonly ISellerRepository _sellerRep;

        public SellerCommandHandler(ISellerRepository sellerRepository)
        {
            _sellerRep = sellerRepository;
        }

        public async Task<ValidationResult> Handle(AddSellerCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;

            var seller = new Data.Seller(request.Name, request.Address, request.Neighbohood, request.Number, request.Reference, request.WorkingTime, request.Details, request.Image, request.HasParking);

            _sellerRep.Add(seller);

            return await PersistirDados(_sellerRep.UnitOfWork);
        }
    }
}
