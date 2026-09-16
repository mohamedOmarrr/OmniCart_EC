namespace E_Commerce_persentation.HttpRequests;

public record BuyerDetailsRequest(
    string BuyerName,
    string MainPhoneNumber,
    string StepPhoneNumber,
    string Address
);