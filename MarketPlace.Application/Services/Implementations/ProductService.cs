using MarketPlace.Application.Services.Interfaces;
using MarketPlace.DataLayer.Entities.Products;
using MarketPlace.DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Implementations
{
    public class ProductService : IProductService
    {
        #region cunstructor
        public readonly IGenericRepository<Product> _productRepository;
        public ProductService(IGenericRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }
        #endregion

        #region
        public async ValueTask DisposeAsync()
        {
            await _productRepository.DisposeAsync();
        }
        #endregion
    }
}
