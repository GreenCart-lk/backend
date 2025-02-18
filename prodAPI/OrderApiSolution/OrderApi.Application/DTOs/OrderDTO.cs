using System.ComponentModel.DataAnnotations;
namespace OrderApi.Application.DTOs
{
    public record OrderDTO
    (
        int Id,
        [Required, Range(1, int.MaxValue)] int ProductId,
        [Required, Range(1, int.MaxValue)] int ClientId,
        [Required, Range(1, int.MaxValue)] int PurchaseQuantity,
        DateTime OrderedDate
        )
    {
        private (int Id, int ClientId, int ProductId, int PurchaseQuantity, DateTime OrderedDate) value;

        public OrderDTO((int Id, int ClientId, int ProductId, int PurchaseQuantity, DateTime OrderedDate) value)
        {
            this.value = value;
        }
    }
}
