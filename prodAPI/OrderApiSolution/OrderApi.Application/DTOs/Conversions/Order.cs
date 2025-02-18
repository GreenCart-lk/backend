
namespace OrderApi.Application.DTOs.Conversions
{
    public class Order
    {
        public int Id { get; internal set; }
        public int ClientId { get; internal set; }
        public int ProductId { get; internal set; }
        public DateTime OrderedDate { get; internal set; }
        public int PurchaseQuantity { get; internal set; }
    }
}