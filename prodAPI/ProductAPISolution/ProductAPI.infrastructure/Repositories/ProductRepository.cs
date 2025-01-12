using Lib.Logs;
using Lib.Responses;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Product> FindByIdAsync(int id)
        {
            try
            {
                var product = await context.Products.FindAsync(id);
                return product is not null ? product : null!;
            }
            catch (Exception ex)
            {
                //log the original exception
                LogException.LogExceptions(ex);

                //display scary free message to client
                throw new Exception("Error occurred retrieving product");
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var products = await context.Products.AsNoTracking().ToListAsync();
                return products is not null ? products : null!;
            }
            catch (Exception ex)
            {
                //log the original exception
                LogException.LogExceptions(ex);

                //display scary free message to client
                throw new InvalidOperationException("Error occurred retrieving product");
            }
        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                var product = await context.Products.Where(predicate).FirstOrDefaultAsync();
                return product is not null ? product : null!;
            }
            catch(Exception ex)
            {
                //log the original exception
                LogException.LogExceptions(ex);

                //display scary free message to client
                throw new InvalidOperationException("Error occurred retrieving product");
            }
        }

        public async Task<Response> UpdateAsync(Product entity)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id);
                if (product is null)
                    return new Response(false, $"{entity.Name} not found");

                context.Entry(product).State = EntityState.Detached;
                context.Products.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} is updated");
            }
            catch (Exception ex)
            {
                //log the original exception
                LogException.LogExceptions(ex);

                //display scary free message to client
                return new Response(false, "Error occurred retrieving product");
            }
        }
    }
}
