using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using OrderApi.Domain.Entities;
using Polly;
using Polly.Registry;
using System.Net.Http.Json;
namespace OrderApi.Application.Services
{
    public class OrderService(IOrder orderInterface, HttpClient httpClient,
       ResiliencePipelineProvider<string> resiliencePipeline) : IOrderService
    {
        private object retryPipeline;
        private object _orders;

        //get product
        public async Task<ProductDTO> GetProduct(int productId)
        {
            //call product API using HttpClient
            //Redirect this call to the API Gateway since product API is noit response to outsiders
            var getProduct = await httpClient.GetAsync($"/api/products/{productId}");
            if (!getProduct.IsSuccessStatusCode)
                return null!;
            var product = await getProduct.Content.ReadFromJsonAsync<ProductDTO>();
            return product!;
        }

        //get user
        public async Task<AppUserDTO> GetUser(int userId)
        {
            //call product API using HttpClient
            //Redirect this call to the API Gateway since product API is noit response to outsiders
            var getUser = await httpClient.GetAsync($"http://localhost:5000/api/Authentication/{userId}");
            if (!getUser.IsSuccessStatusCode)
                return null!;
            var product = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();
            return product!;
        }

        //get order details by Id
        public async Task<OrderDetailsDTO> GetOrderDetails(int orderId)
        {
            //prepare order
            var order = await orderInterface.FindByIdAsync(orderId);
            if (order is null || order!.Id <= 0)
                return null!;

            //get retry pipeline
            var retryPipline = resiliencePipeline.GetPipeline("my-retry-pipeline");

            //prepare products
            var productDTO = await retryPipline.ExecuteAsync(async token => await GetProduct(order.ProductId));

            //prepare client
            var appUserDTO = await retryPipline.ExecuteAsync(async token => await GetUser(order.ClientId));

            //populate order details
            return new OrderDetailsDTO(
                order.Id,
                productDTO.Id,
                appUserDTO.Id,
                appUserDTO.Name,
                appUserDTO.Email,
                appUserDTO.Address,
                appUserDTO.TelephoneNumber,
                productDTO.Name,
                order.PurchaseQuantity,
                productDTO.Price,
                productDTO.Quantity * order.PurchaseQuantity,
                order.OrderedDate

                );
        }

        //get orders by client ID


        public async Task<IEnumerable<OrderDTO>> GetOrdersByClientsID(int clientId)
        {
            //get all clients orders
            var orders = await orderInterface.GetOrdersAsync(o=> o.ClientId == clientId);
            if(!orders.Any()) return null!;

            //convert from entity to DTO
            var (_, _orders) = OrderConversion.FromEntity(null, orders);
            return _orders!;
        }
    }
}
