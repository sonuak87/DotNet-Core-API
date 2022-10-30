using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using VeggiFoodAPI.Data;
using VeggiFoodAPI.Extentions;
using VeggiFoodAPI.Helpers;
using VeggiFoodAPI.Migrations;
using VeggiFoodAPI.Models;
using VeggiFoodAPI.Models.DTOs;
using VeggiFoodAPI.Models.ViewModels;
using VeggiFoodAPI.RequestHelpers;
using VeggiFoodAPI.Services;

namespace VeggiFoodAPI.Controllers
{
    [Route("api/product")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ImageService _imageService;
        CustomResponse _customResponse = new CustomResponse();
        public ProductsController(ApplicationDbContext context, IMapper mapper, ImageService imageService)
        {
            _imageService = imageService;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Product>>> GetProducts([FromQuery] ProductParams productParams)
        {
            var query = _context.Products
                .Sort(productParams.OrderBy)
                .Search(productParams.SearchTerm)
                .Filter(productParams.Brands, productParams.Types)
                .AsQueryable();

            var products = await PagedList<Product>.ToPagedList(query,
                productParams.PageNumber, productParams.PageSize);

            Response.AddPaginationHeader(products.MetaData);

            return Ok(_customResponse.GetResponseModel(null, products));
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound();

            return Ok(_customResponse.GetResponseModel(null, product));
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetFilters()
        {
            var brands = await _context.Products.Select(p => p.Brand).Distinct().ToListAsync();
            var types = await _context.Products.Select(p => p.Type).Distinct().ToListAsync();

            return Ok(_customResponse.GetResponseModel(null, new { brands, types }));
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromForm] CreateProductModel productDto)
        {
            if (ModelState.IsValid)
            {
                var product = _mapper.Map<Product>(productDto);

                if (productDto.File != null)
                {
                    var imageResult = await _imageService.AddImageAsync(productDto.File);

                    if (imageResult.Error != null)
                        return BadRequest(new ProblemDetails { Title = imageResult.Error.Message });

                    product.PictureUrl = imageResult.SecureUrl.ToString();
                    product.PublicId = imageResult.PublicId;
                }

                _context.Products.Add(product);

                var result = await _context.SaveChangesAsync() > 0;

                if (result) return CreatedAtRoute("GetProduct", new { Id = product.Id }, product);
                return BadRequest(_customResponse.GetResponseModel(new string[] { "Problem creating new product" }, null));
            }
            return BadRequest(_customResponse.GetResponseModel(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)), null));

        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct([FromForm] UpdateProductModel productDto)
        {
            if (ModelState.IsValid)
            {
                var product = await _context.Products.FindAsync(productDto.Id);

            if (product == null) return NotFound();

            _mapper.Map(productDto, product);

            if (productDto.File != null)
            {
                var imageResult = await _imageService.AddImageAsync(productDto.File);

                if (imageResult.Error != null)
                    return BadRequest(new ProblemDetails { Title = imageResult.Error.Message });

                if (!string.IsNullOrEmpty(product.PublicId))
                    await _imageService.DeleteImageAsync(product.PublicId);

                product.PictureUrl = imageResult.SecureUrl.ToString();
                product.PublicId = imageResult.PublicId;
            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(product);

                return BadRequest(_customResponse.GetResponseModel(new string[] { "Problem updating product" }, null));
            }
            return BadRequest(_customResponse.GetResponseModel(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)), null));

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound(_customResponse.GetResponseModel(new string[] { "Product not found" }, null));

            if (!string.IsNullOrEmpty(product.PublicId))
                await _imageService.DeleteImageAsync(product.PublicId);

            _context.Products.Remove(product);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(_customResponse.GetResponseModel(null,"Product deleted"));

            return BadRequest(new ProblemDetails { Title = "Problem deleting product" });
        }
    }
}
