using Lib.Logs;
using Lib.Responses;
using Microsoft.Extensions.Options;
using ProductAPI.Application.Interfaces;
using ProductAPI.domain.Entity;
using ProductAPI.infrastructure.Data;
using System.Linq.Expressions;
namespace ProductAPI.infrastructure.Repositories
{
    internal class ProductRepository(ProductDbContext context) : IProduct
    {
        public async Task<Response> CreateAsync(Product entity)
        {
            try
            {
                //check if the product already
                var getProduct = await GetByAsync(_ => _.Name!.Equals(entity.Name));
                if (getProduct is not null && !string.IsNullOrEmpty(getProduct.Name))
                    return new Response(false, $"{entity.Name} already added");

                var currentEntity = context.Products.Add(entity).Entity;
                await context.SaveChangesAsync();
                if (currentEntity is not null && currentEntity.Id > 0)
                    return new Response(true, $"{entity.Name} added to the database");
                else
                    return new Response(false, $"Error occured while adding {entity.Name}");
            }catch(Exception ex)
            {
                //log the original exception
                LogException.LogExceptions(ex);

                //display scary free message to client
                return new Response(false, "Error occurred adding new product");
            }
        }

        public async Task<Response> DeleteAsync(Product entity)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id);
                if (product is null)
                    return new Response(false, $"{entity.Name} not found"); 
                await context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} is deleted"); 
            }
            catch(Exception ex)
            {
                //log the original exception
                LogException.LogExceptions(ex);

                //display scary free message to client
                return new Response(false, "Error occurred deleting product");
    }
}

        public Task<Product> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Product entity)
        {
            throw new NotImplementedException();
        }
    }
}
