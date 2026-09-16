using System.Security.Cryptography;
using Asp.Versioning;
using Asp.Versioning.Builder;
using E_commerce_application.Commands;
using E_commerce_application.DTOs;
using E_commerce_application.Queries;
using E_commerce_application.Response_Patterns.Payment;
using E_Commerce_persentation.Extensions;
using E_Commerce_persentation.HttpRequests.Payment;
using E_commerce_persentation.ResponseShapes;
using MediatR;

namespace E_Commerce_persentation.EndPoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(
        this IEndpointRouteBuilder endpoints,
        ApiVersionSet apiVersionSet)
    {
        var group = endpoints
            .MapGroup("/api/v{version:apiVersion}/order")
            .WithTags("Products")
            .WithApiVersionSet(apiVersionSet)
            .HasApiVersion(new ApiVersion(1, 0));

        group.MapPost("/", async (
                CreateOrderCommand command,
                ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(command, ct);

                return result.CommandResult("Your Order has been Created successfully", StatusCodes.Status201Created);
            })
            .WithSummary("Create a Order")
            .WithDescription("Returns Receipt details if Order will Pay by Cash , or Returns OrderId, ClientSecret and TransactionId if Order will Pay by Card")
            .Produces<ApiResponse<BaseOrderResponse>>(StatusCodes.Status201Created);
        
        
        group.MapGet("/delivery", async (
                ISender sender,
                CancellationToken ct) =>
            {
                var query = new DeliveryOrderQuery();
                var result = await sender.Send(query, ct);

                return result.GetListedResults("Delivery Details retrieved successfully");
            })
            .WithSummary("Gets all Delivery Methods")
            .WithDescription("Returns a list of  Delivery Method Details")
            .Produces<ApiResponse<IReadOnlyList<DeliveryOrderQuery>>>(StatusCodes.Status200OK);

        group.MapPost("/webhook", async (
                    PaymobWebhookRequest data,
                    string Hmac,
                    ISender sender,
                    CancellationToken ct) =>
            {
                var request = new WebhookCommand(data, Hmac);
                    
                    var result = await sender.Send(request, ct);

                    return result.CommandResult("Payment Process Completed successfully", StatusCodes.Status200OK);
                })
            .AllowAnonymous()
            .WithSummary("payment webhook")
            .WithDescription("Receives transaction payment status updates from Paymob Gate.")
            .Produces(StatusCodes.Status200OK);
        
        group.MapGet("/my-orders", async (
                ISender sender,
                CancellationToken ct) =>
            {
                var query = new GetMyOrdersQuery();

                var result = await sender.Send(query, ct);

                return result.GetListedResults("Your orders retrieved successfully");
            })
            .WithSummary("Gets current user's orders")
            .WithDescription("Returns all orders created by the current user.")
            .Produces<ApiResponse<IReadOnlyList<MyOrdersDto>>>(StatusCodes.Status200OK);
        
        
        group.MapGet("/{orderId:guid}/receipt", async (
                [AsParameters] GetOrderReceiptQuery query,
                ISender sender,
                CancellationToken ct) =>
            {
              

                var result = await sender.Send(query, ct);

                return result.GetSpecificItem("Order receipt retrieved successfully");
            })
            .WithSummary("Gets order receipt")
            .WithDescription("Returns the receipt details of a specific order.")
            .Produces<ApiResponse<OrderReceiptDto>>(StatusCodes.Status200OK);
        
        
        group.MapGet("/admin", async (
                [AsParameters] GetAllOrdersToAdminQuery query,
                ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(query, ct);

                return result.GetListedResults(
                    "Orders retrieved successfully");
            })
            .RequireAuthorization(policy => policy.RequireRole("Admin"))
            .WithSummary("Gets all orders")
            .WithDescription(
                "Returns all orders for administrators, with optional status filtering.")
            .Produces<ApiResponse<IReadOnlyList<AllOrdersDto>>>(
                StatusCodes.Status200OK);

        return endpoints;
    }
}