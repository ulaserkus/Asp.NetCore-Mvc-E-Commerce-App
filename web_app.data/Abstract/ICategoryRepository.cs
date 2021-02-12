using System;
using System.Collections.Generic;
using web_app.entity;

namespace web_app.data.Abstract
{
    public interface ICategoryRepository:IRepository<Category>
    {


      

        Category GetByIdWithProducts(int categoryId);
        void DeleteFromCategory(int ProductId,int CategoryId);
    }
}
