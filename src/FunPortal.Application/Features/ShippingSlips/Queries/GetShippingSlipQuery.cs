using AutoMapper;
using FunPortal.Application.DTOs.ShippingSlips;
using FunPortal.Application.Interfaces.Repositories;
using MediatR;

namespace FunPortal.Application.Features.ShippingSlips.Queries;

public record GetShippingSlipQuery(int Id) : IRequest<ShippingSlipDto?>;

public class GetShippingSlipQueryHandler(
    IShippingSlipRepository shippingSlipRepository,
    IMapper mapper)
    : IRequestHandler<GetShippingSlipQuery, ShippingSlipDto?>
{
    public async Task<ShippingSlipDto?> Handle(GetShippingSlipQuery query, CancellationToken cancellationToken)
    {
        var shippingSlip = await shippingSlipRepository.GetByIdAsync(query.Id, cancellationToken);
        if (shippingSlip == null)
            return null;

        return mapper.Map<ShippingSlipDto>(shippingSlip);
    }
}
