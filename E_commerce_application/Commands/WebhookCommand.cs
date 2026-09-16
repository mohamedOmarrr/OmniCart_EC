using E_commerce_domain.shared;
using E_Commerce_persentation.HttpRequests.Payment;
using MediatR;

namespace E_commerce_application.Commands;

public record WebhookCommand(
        PaymobWebhookRequest Request,
        string? Hmac
    ): IRequest<Result>;