namespace E_Commerce_persentation.HttpRequests.Payment;

public record PaymobTransaction(
    long Id,
    bool Pending,
    long AmountCents,
    bool Success,
    bool IsAuth,
    bool IsCapture,
    bool IsStandalonePayment,
    bool IsVoided,
    bool IsRefunded,
    bool Is3DSecure,
    long IntegrationId,
    long ProfileId,
    bool HasParentTransaction,
    PaymobOrder Order,
    DateTimeOffset CreatedAt,
    string Currency,
    bool ErrorOccured,
    long Owner,
    PaymobSourceData SourceData
);