using System;
using System.Collections.Generic;
using web_app.business.Abstract;
using web_app.data.Abstract;
using web_app.entity;

namespace web_app.business.Concrete
{
    public class CategoryManager : ICategoryService
    {

        private ICategoryRepository _CategoryRepository;
        public CategoryManager(ICategoryRepository categoryRepository)
        {
            _CategoryRepository = categoryRepository;
        }

        public string ErrorMessage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Create(Category entity)
        {
            _CategoryRepository.Create(entity);
        }

        public void Delete(Category entity)
        {
             _CategoryRepository.Delete(entity);
        }

        public void DeleteFromCategory(int ProductId, int CategoryId)
        {
            _CategoryRepository.DeleteFromCategory(ProductId,CategoryId);
        }

        public List<Category> GetAll()
        {
            return _CategoryRepository.GetAll();
        }

        public Category GetByIdWithProducts(int categoryId)
        {
           return _CategoryRepository.GetByIdWithProducts(categoryId);
        }

        public Category GetProductById(int id)
        {
            return _CategoryRepository.GetProductById(id);
        }

        public void Update(Category entity)
        {
          _CategoryRepository.Update(entity);
        }

        public bool Validation(Category entity)
        {
            throw new NotImplementedException();
        }
    }
}
