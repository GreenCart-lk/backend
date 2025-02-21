using Microsoft.EntityFrameworkCore;
using OrderApi.Application.DTOs.Conversions;


namespace OrderApi.Infrastructure.Data
{ 
        public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options) 
        { 
            public DbSet<Order> Orders {  get; set; }           
        }
    
}
