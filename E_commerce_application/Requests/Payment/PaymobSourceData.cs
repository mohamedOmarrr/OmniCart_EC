namespace E_Commerce_persentation.HttpRequests.Payment;

public record PaymobSourceData(
    string? Pan,
    string? Type,
    string? SubType
);