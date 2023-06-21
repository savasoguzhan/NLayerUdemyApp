using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : CustomBaseController
    {
        private readonly IMapper _mapper;

        private readonly IProductService _service;

        

        public ProductController(IService<Product> service, IMapper mapper, IProductService productService)
        {
           
            _mapper = mapper;
            _service = productService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductWtihCategory()
        {
            return CreateActionResult(await _service.GetProductsWithCategory());
        }

        

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var products = await _service.GetAllAsync();

            var productsDtoos = _mapper.Map<List<ProductDto>>(products.ToList());

            return CreateActionResult(CustomResponseDto<List<ProductDto>>.Succes(200, productsDtoos));

            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);

            var productDtos = _mapper.Map<ProductDto>(product);

            return CreateActionResult(CustomResponseDto<ProductDto>.Succes(200, productDtos));


        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductUpdateDto productDto)
        {
            var product = await _service.AddAsync(_mapper.Map<Product>(productDto));

            var productsDtos = _mapper.Map<ProductDto>(product);

            return CreateActionResult(CustomResponseDto<ProductDto>.Succes(201, productsDtos));


        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
             await _service.UpdateAsync(_mapper.Map<Product>(productDto));

            

            return CreateActionResult(CustomResponseDto<NoContentDto>.Succes(204));


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _service.GetByIdAsync(id);
            await _service.RemevoAsync(product);

            

            return CreateActionResult(CustomResponseDto<NoContentDto>.Succes(204));


        }


    }
}
