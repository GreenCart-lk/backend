
namespace OrderApi.Domain.Entities
{
    public class Oder
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ClientId { get; set; }
        public int PurchaseQuantity { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    }
}
