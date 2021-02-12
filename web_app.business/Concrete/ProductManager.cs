using System;
using System.Collections.Generic;
using web_app.business.Abstract;
using web_app.data.Abstract;
using web_app.entity;

namespace web_app.business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductRepository _productRepository;
        public ProductManager(IProductRepository productRepository)
        {
            this._productRepository = productRepository;

        }


        public bool Create(Product entity)
        {
            if (Validation(entity))
            {
                _productRepository.Create(entity);
                return true;
            }
            //iş kurallarını  uygula
            return false;
        }

        public void Delete(Product entity)
        {
            _productRepository.Delete(entity);
        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public int GetCountByCategory(string category)
        {
            return _productRepository.GetCountByCategory(category);
        }

        public List<Product> GetHomePageProducts()
        {
            return _productRepository.GetHomePageProducts();
        }

        public Product GetProductById(int id)
        {
            return _productRepository.GetProductById(id);
        }

        public Product GetProductByIdWithCategories(int id)
        {
            return _productRepository.GetProductByIdWithCategories(id);
        }

        public Product GetProductDetails(string url)
        {
            return _productRepository.GetProductDetails(url);
        }

        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            return _productRepository.GetProductByCategory(name, page, pageSize);
        }

        public List<Product> GetSearchResult(string searchString)
        {
            return _productRepository.GetSearchResult(searchString);
        }

        public void Update(Product entity)
        {
            _productRepository.Update(entity);
        }

        public bool Update(Product entity, int[] categoryIds)
        {
            if (Validation(entity))
            {

                if (categoryIds.Length == 0)
                {
                    ErrorMessage += "Ürün için en az bir kategori seçmelisiniz\n";
                    return false;
                }
                _productRepository.Update(entity, categoryIds);
                return true;
            }
            return false;
        }
        public string ErrorMessage { get; set; }

        public bool Validation(Product entity)
        {
            var IsValid = true;
            if (string.IsNullOrEmpty(entity.Name))
            {
                ErrorMessage += "ürün ismi girmelisiniz. \n";
                IsValid = false;
            }
            if (entity.Price < 0)
            {
                ErrorMessage += "fiyat alanı negatif olamaz. \n";
                IsValid = false;
            }

            return IsValid;
        }
    }
}
