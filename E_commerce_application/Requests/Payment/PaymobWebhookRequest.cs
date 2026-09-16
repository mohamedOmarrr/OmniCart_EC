namespace E_Commerce_persentation.HttpRequests.Payment;

public record PaymobWebhookRequest(
    string Type,
    PaymobTransaction Obj
);