using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using EcommerceSportsShopNetAndAngular.Dtos.API.Dtos;
using EcommerceSportsShopNetAndAngular.Errors;
using EcommerceSportsShopNetAndAngular.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceSportsShopNetAndAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseApiController
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
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var countSpec = new ProductsWithFiltersForCountSpecification(productParams);

            var totalItems = await GenericRepositoryProducts.CountAsync(countSpec);

            var products = await GenericRepositoryProducts.ListAsync(spec);

            if (products == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var data = mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await GenericRepositoryProducts.GetEntityWithSpec(spec);
            if (product == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return mapper.Map<Product,ProductToReturnDto>(product);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> PutProduct(int id, Product product)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var products = await GenericRepositoryProducts.GetEntityWithSpec(spec);

            if (id != product.Id) return BadRequest(new ApiResponse(400));

            if (products == null) return NotFound(new ApiResponse(404));

            return mapper.Map<Product, ProductToReturnDto>(product);
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
            return Ok(await GenericRepositoryProductBrand.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await GenericRepositoryProductType.ListAllAsync());
        }

        private bool ProductExists(int id)
        {
            return GenericRepositoryProducts.GetByIdAsync(id) != null;
        }
    }
}
