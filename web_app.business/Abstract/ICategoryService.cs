using System;
using System.Collections.Generic;
using web_app.entity;

namespace web_app.business.Abstract
{
    public interface ICategoryService:IValidator<Category>
    {

        Category GetProductById(int id);

        List<Category> GetAll();


        void Create(Category entity);

        void Update(Category entity);

        void Delete(Category entity);
        Category GetByIdWithProducts(int categoryId);
        void DeleteFromCategory(int ProductId, int CategoryId);

    }
}
