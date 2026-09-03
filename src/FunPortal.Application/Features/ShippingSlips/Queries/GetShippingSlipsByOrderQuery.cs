using AutoMapper;
using FunPortal.Application.DTOs.ShippingSlips;
using FunPortal.Application.Interfaces.Repositories;
using MediatR;

namespace FunPortal.Application.Features.ShippingSlips.Queries
{
    public record GetShippingSlipsByOrderQuery(int OrderId)
        : IRequest<IReadOnlyCollection<ShippingSlipDto>>;
    
    public class GetShippingSlipsByOrderQueryHandler(
        IShippingSlipRepository shippingSlipRepository,
        IMapper mapper)
        : IRequestHandler<GetShippingSlipsByOrderQuery, IReadOnlyCollection<ShippingSlipDto>>
    {
        public async Task<IReadOnlyCollection<ShippingSlipDto>> Handle(GetShippingSlipsByOrderQuery request, CancellationToken cancellationToken)
        {
            var shippingSlips = await shippingSlipRepository
                .GetByOrderIdAsync(request.OrderId, cancellationToken);

            return mapper.Map<IReadOnlyCollection<ShippingSlipDto>>(shippingSlips);
        }
    }
}
