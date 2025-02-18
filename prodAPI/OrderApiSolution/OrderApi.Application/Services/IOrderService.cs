using OrderApi.Application.DTOs;
namespace OrderApi.Application.Services
{
    internal interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetOrdersByClientsID(int clientId);
        Task<OrderDetailsDTO> GetOrderDetails(int orderId);


    }
}
