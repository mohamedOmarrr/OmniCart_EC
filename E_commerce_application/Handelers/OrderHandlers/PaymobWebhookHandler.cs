using E_commerce_application.Commands;
using E_commerce_application.Interfaces;
using E_commerce_domain.Entities;
using E_commerce_domain.Errors;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Handelers.OrderHandlers;

public class PaymobWebhookHandler(
    IOrderRepository orderRepository,
    IPaymentService paymentService)
    : IRequestHandler<WebhookCommand, Result>
{
    public async Task<Result> Handle(
        WebhookCommand request,
        CancellationToken cancellationToken)
    {
        var paymobTransaction = request.Request.Obj;
        
        var payOrder = await orderRepository.GetByPaymentTransactionIdAsync(paymobTransaction.Id.ToString(), cancellationToken);
        
        if (payOrder is null)
            return Result.Failure(
                OrderError.NotFound);

        var isValid =
            paymentService.VerifyWebhook(
                paymobTransaction,
                request.Hmac);

        if (!isValid)
            return Result.Failure(
                OrderError.InvalidWebhook);
        
        
        var processingResult =
            payOrder.MarkAsPaid(
                paymobTransaction.Id.ToString());

        if (processingResult.IsFailure)
            return processingResult;

        await orderRepository.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }
}