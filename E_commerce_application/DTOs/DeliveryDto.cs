namespace E_commerce_application.DTOs;

public record DeliveryDto(
         Guid DeliveryId,
         string Name,
         string Description, 
         decimal Price, 
         string EstimatedDeliveryTime, 
         bool IsAvailable, 
         int DisplayOrder
    );