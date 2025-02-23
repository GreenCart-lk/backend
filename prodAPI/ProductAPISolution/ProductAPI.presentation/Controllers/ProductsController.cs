using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Application.DTOs;
using ProductAPI.Application.DTOs.Conversions;
using ProductAPI.Application.Interfaces;

namespace ProductAPI.presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductsController(IProduct productInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            //get all sproducts from repo
            var products = await productInterface.GetAllAsync();
            if (!products.Any())
                return NotFound("No product detected in the database");

            //convert data from entity to DTO
            var (_, list) = ProductConversions.FromEntity(null!, products);
            return list!.Any() ? Ok(list) : NotFound("No product found");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>>GetProduct(int id)
        {
            //get single product
            var product = await productInterface.FindByIdAsync(id);
            if (product is null)
                return NotFound("Product Requested not found");

            //convert from entity to DTO
            var (_product, _) = ProductConversions.FromEntity(product, null!);
            return _product is not null ? Ok(_product) : NotFound("Product not found");
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<Response>> CreateProduct(ProductDTO product)
        {
            //check model state is all data passed
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //convert to entity
            var getEntity = ProductConversions.ToEntity(product);
            var response = await productInterface.CreateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);

        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> UpdateProduct(ProductDTO product)
        {
            //check model state is all data passed
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //convert to entity
            var getEntity = ProductConversions.ToEntity(product);
            var response = await productInterface.UpdateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> DeleteProduct(ProductDTO product)
        {
            //convert to entity
            var getEntity = ProductConversions.ToEntity(product);
            var response = await productInterface.DeleteAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
    }
}
