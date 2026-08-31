using FunPortal.Application.DTOs.ShippingSlips;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Application.Mappers;
using MediatR;

namespace FunPortal.Application.Features.ShippingSlips.Queries
{
    public record GetShippingSlipsByOrderQuery(int OrderId)
        : IRequest<IReadOnlyCollection<ShippingSlipDto>>;
    
    public class GetShippingSlipsByOrderQueryHandler(
        IShippingSlipRepository shippingSlipRepository)
        : IRequestHandler<GetShippingSlipsByOrderQuery, IReadOnlyCollection<ShippingSlipDto>>
    {
        public async Task<IReadOnlyCollection<ShippingSlipDto>> Handle(GetShippingSlipsByOrderQuery request, CancellationToken cancellationToken)
        {
            var shippingSlips = await shippingSlipRepository
                .GetByOrderIdAsync(request.OrderId, cancellationToken);

            return shippingSlips.ToDtos();
        }
    }
}
