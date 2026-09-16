namespace E_commerce_domain.Entities;

public class OrderShippingDetails :BaseEntity
{
    private OrderShippingDetails()
    {
    }
    
    public string BuyerName { get; private set; } = null!;
    public string MainPhoneNumber { get; private set; } = null!;
    public string StepPhoneNumber { get; private set; } = null!;
    public string Address {get; private set; } = null!;
    
    public static OrderShippingDetails Create(
        string buyerName,
        string mainPhoneNumber,
        string stepPhoneNumber,
        string address)
    {
        return new OrderShippingDetails
        {
            BuyerName = buyerName,
            MainPhoneNumber = mainPhoneNumber,
            StepPhoneNumber = stepPhoneNumber,
            Address = address
        };
    }
}