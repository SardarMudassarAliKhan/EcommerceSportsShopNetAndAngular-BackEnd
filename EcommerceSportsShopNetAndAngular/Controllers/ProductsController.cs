using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using EcommerceSportsShopNetAndAngular.Dtos.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceSportsShopNetAndAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public IGenericRepository<Product> GenericRepositoryProducts;
        public IGenericRepository<ProductType> GenericRepositoryProductType;
        public IGenericRepository<ProductBrand> GenericRepositoryProductBrand;
        private readonly IMapper mapper;

        public ProductsController(
            IGenericRepository<Product> genericRepositoryProducs,
            IGenericRepository<ProductType> genericRepositoryProductType,
            IGenericRepository<ProductBrand> genericRepositoryProductBrand,
            IMapper mapper)
        {
            GenericRepositoryProducts = genericRepositoryProducs;
            GenericRepositoryProductType = genericRepositoryProductType;
            GenericRepositoryProductBrand = genericRepositoryProductBrand;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await GenericRepositoryProducts.ListAsync(spec);
            return Ok(mapper.Map<IEnumerable<Product>,IEnumerable<ProductToReturnDto>>(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await GenericRepositoryProducts.GetEntityWithSpec(spec);
            if (product == null)
            {
                return NotFound();
            }
            return mapper.Map<Product,ProductToReturnDto>(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            try
            {
                await GenericRepositoryProducts.Update(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpPost]
        public void PostProduct(Product product)
        {
            try
            {
                var newProduct = GenericRepositoryProducts.Add(product);
                CreatedAtAction("GetProduct", new { id = newProduct.Id }, newProduct);
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(Product product)
        {
            var productres = await GenericRepositoryProducts.GetByIdAsync(product.Id);
            if (productres == null)
            {
                return NotFound();
            }
            try
            {
                await GenericRepositoryProducts.Delete(productres);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var brands = await GenericRepositoryProductBrand.ListAllAsync();
            return Ok(brands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var types = await GenericRepositoryProductType.ListAllAsync();
            return Ok(types);
        }

        private bool ProductExists(int id)
        {
            return GenericRepositoryProducts.GetByIdAsync(id) != null;
        }
    }
}
