using FunPortal.Application.DTOs.ShippingSlips;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Application.Mappers;
using MediatR;

namespace FunPortal.Application.ShippingSlips.Queries;

public record GetShippingSlipQuery(int Id) : IRequest<ShippingSlipDto?>;

public class GetShippingSlipQueryHandler(
    IShippingSlipRepository shippingSlipRepository)
    : IRequestHandler<GetShippingSlipQuery, ShippingSlipDto?>
{
    public async Task<ShippingSlipDto?> Handle(GetShippingSlipQuery query, CancellationToken cancellationToken)
    {
        var shippingSlip = await shippingSlipRepository.GetByIdAsync(query.Id, cancellationToken);
        if (shippingSlip == null)
            return null;

        return shippingSlip.ToDto();
    }
}
