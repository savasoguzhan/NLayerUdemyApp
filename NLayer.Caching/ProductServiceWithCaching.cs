﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Caching
{
    public class ProductServiceWithCaching : IProductService
    {
        private const string CacheProductKey = "productsCache";
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductServiceWithCaching(IMapper mapper, IMemoryCache memoryCache, IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _repository = repository;
            _unitOfWork = unitOfWork;

            if(!_memoryCache.TryGetValue(CacheProductKey, out _))
            {
                _memoryCache.Set(CacheProductKey,_repository.GetAll().ToList());
            }
        }






        public async Task<Product> AddAsync(Product entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.ComitAsync();
            await CacheAllProductAsync();
            return entity;

        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.ComitAsync();
            await CacheAllProductAsync();
            return entities;
        }

        public Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult(_memoryCache.Get<IEnumerable<Product>>(CacheProductKey));
        }

        public Task<Product> GetByIdAsync(int id)
        {
            return Task.FromResult(_memoryCache.Get<List<Product>>(CacheProductKey).FirstOrDefault(x =>x.Id == id));
        }

        public Task<CustomResponseDto<List<ProductWithCategory>>> GetProductsWithCategory()
        {
            throw new NotImplementedException();
        }

        public  async Task RemevoAsync(Product entity)
        {
            _repository.Remevo(entity);
            await _unitOfWork.ComitAsync();
            await CacheAllProductAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<Product> entities)
        {
            _repository.RemoveRange(entities);
            await _unitOfWork.ComitAsync();
            await CacheAllProductAsync();
        }

        public async Task UpdateAsync(Product entity)
        {
            _repository.Update(entity);
            await _unitOfWork.ComitAsync();
            await CacheAllProductAsync();
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            return _memoryCache.Get<List<Product>>(CacheProductKey).Where(expression.Compile()).AsQueryable();
        }


        public async Task CacheAllProductAsync()
        {
            _memoryCache.Set(CacheProductKey,await _repository.GetAll().ToListAsync());
        }
    }
}