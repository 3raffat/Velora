using DeliveryService.Contracts;
using DeliveryService.Responces;
using DeliveryService.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Controllers;

[ApiController]
[Route("api/v1/shipments")]
public sealed class ShipmentsController(IShipmentService shipmentService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateShipmentRequest request,
        CancellationToken cancellationToken
    )
    {
        var shipment = await shipmentService.CreateAsync(request, cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            new StandardSuccessResponse<object>(
                shipment,
                StatusCodes.Status201Created,
                "Shipment created successfully"
            )
        );
    }

    [HttpPost("{id:guid}/deliver")]
    public async Task<IActionResult> Deliver(Guid id, CancellationToken cancellationToken)
    {
        await shipmentService.DeliverAsync(id, cancellationToken);

        return Ok(
            new StandardSuccessResponse<object>(
                null,
                StatusCodes.Status200OK,
                "Shipment delivered successfully"
            )
        );
    }
}
