using AutoMapper;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Services
{
    public class ProductServiceWithNoCaching : Service<Product>, IProductService
    {

        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductServiceWithNoCaching(IUnitOfWork unitOfWork, IGenericRepository<Product> repository, IMapper mapper, IProductRepository productRepository) : base(unitOfWork, repository)
        {

            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<CustomResponseDto<List<ProductWithCategory>>> GetProductsWithCategory()
        {
            var product = await _productRepository.GetProductsWithCategory();

            var productDto=_mapper.Map<List<ProductWithCategory>>(product);
            return CustomResponseDto<List<ProductWithCategory>>.Succes(200, productDto);
        }
    }
}
