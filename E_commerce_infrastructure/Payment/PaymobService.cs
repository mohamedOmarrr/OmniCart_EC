using System.Security.Cryptography;
using System.Text;
using E_commerce_application.Interfaces;
using E_commerce_application.Response_Patterns;
using E_commerce_domain.Entities;
using E_Commerce_persentation.HttpRequests.Payment;

namespace E_commerce_infrastructure.Payment;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;

public sealed class PaymobService(
    HttpClient httpClient,
    IOptions<PaymobSettings> options)
    : IPaymentService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly PaymobSettings _settings = options.Value;

    public async Task<PaymobPaymentResponse> CreatePaymentAsync(
        Guid orderId,
        decimal totalAmount,
        decimal deliveryPrice,
        IReadOnlyCollection<OrderItem> items,
        string firstName,
        string lastName,
        string email,
        CancellationToken cancellationToken)
    {
        // Convert order total to the smallest currency unit
        var totalAmountInCents = ConvertToCents(totalAmount);
        

        // Map our order items to PaymobItems
        var paymobItems = items
            .Select(item => new PaymobItem(
                Name: item.ProductName,
                Amount: ConvertToCents(
                    item.UnitPrice * item.Quantity),
                Quantity: item.Quantity))
            .ToList();

        //Make sure the PaymobItems total matches the final order total
        var itemsTotalInCents =
            paymobItems.Sum(item => item.Amount) + ConvertToCents(deliveryPrice);

        if (itemsTotalInCents != totalAmountInCents)
        {
            return new PaymobPaymentResponse(
                IsSuccess: false,
                Response: null,
                ErrorMessage: "There are mismatch in Process Between Total Amount and ItemsTotalInCents."
            );
        }

        //Create Paymob intention request
        var request = new PaymobIntentionRequest(
            Amount: totalAmountInCents,
            Currency: "EGP",
            PaymentMethods: [_settings.IntegrationId],
            Items: paymobItems,
            BillingData: new PaymobBillingData(
                FirstName: firstName,
                LastName: lastName,
                Email: email),
            SpecialReference: orderId.ToString(),
            NotificationUrl: _settings.NotificationUrl,
            RedirectionUrl: _settings.RedirectionUrl);

        // Create HTTP request
        using var requestMessage = new HttpRequestMessage(
            HttpMethod.Post,
            "/v1/intention/");

       
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Token",
                _settings.SecretKey);

       
        requestMessage.Content =
            JsonContent.Create(request);

        
        using var response = await httpClient.SendAsync(
            requestMessage,
            cancellationToken);

        
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse =
                await response.Content.ReadAsStringAsync(
                    cancellationToken);
            
            return new PaymobPaymentResponse(
                IsSuccess: false,
                Response: null,
                ErrorMessage: $"Paymob payment creation failed. " +
                              $"StatusCode: {response.StatusCode}. " +
                              $"Response: {errorResponse}");
        }

        //Read Paymob JSON response
        var paymobResponse =
            await response.Content.ReadFromJsonAsync<PaymobIntentionResponse>(
                cancellationToken: cancellationToken);
        
        if (paymobResponse is null)
        {
            return new PaymobPaymentResponse(
                IsSuccess: false,
                Response: null,
                ErrorMessage: "There are no Response From PayGate"
            );
        }

        
        return new PaymobPaymentResponse(true, paymobResponse, null);
    }
    
    public bool VerifyWebhook(
        PaymobTransaction transaction,
        string? hmac)
    {
        if (string.IsNullOrWhiteSpace(hmac))
            return false;

        var calculatedHmac =
            CalculateWebhookHmac(transaction);

        return string.Equals(
            calculatedHmac,
            hmac,
            StringComparison.OrdinalIgnoreCase);
    }
    

    private static int ConvertToCents(decimal amount)
    {
        return checked(
            (int)decimal.Round(
                amount * 100,
                0,
                MidpointRounding.AwayFromZero));
    }
    
    private string CalculateWebhookHmac(
        PaymobTransaction transaction)
    {
        var data =
            string.Concat(
                transaction.AmountCents,
                transaction.CreatedAt,
                transaction.Currency,
                transaction.ErrorOccured.ToString().ToLowerInvariant(),
                transaction.HasParentTransaction.ToString().ToLowerInvariant(),
                transaction.Id,
                transaction.IntegrationId,
                transaction.Is3DSecure.ToString().ToLowerInvariant(),
                transaction.IsAuth.ToString().ToLowerInvariant(),
                transaction.IsCapture.ToString().ToLowerInvariant(),
                transaction.IsRefunded.ToString().ToLowerInvariant(),
                transaction.IsStandalonePayment.ToString().ToLowerInvariant(),
                transaction.IsVoided.ToString().ToLowerInvariant(),
                transaction.Order.Id,
                transaction.Owner,
                transaction.Pending.ToString().ToLowerInvariant(),
                transaction.SourceData.Pan,
                transaction.SourceData.SubType,
                transaction.SourceData.Type,
                transaction.Success.ToString().ToLowerInvariant());
    
        using var hmac =
            new HMACSHA512(
                Encoding.UTF8.GetBytes(_settings.HmacSecret));
    
        var hash =
            hmac.ComputeHash(
                Encoding.UTF8.GetBytes(data));
    
        return Convert.ToHexString(hash)
            .ToLowerInvariant();
    }
}