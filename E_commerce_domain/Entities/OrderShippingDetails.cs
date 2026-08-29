namespace E_commerce_domain.Entities;

public class OrderShippingDetails :BaseEntity
{
    public string BuyerName { get; private set; } = null!;
    public string MainPhoneNumber { get; private set; } = null!;
    public string StepPhoneNumber { get; private set; } = null!;
    public string Address {get; private set; } = null!;
}