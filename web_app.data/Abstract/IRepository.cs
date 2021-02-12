using System;
using System.Collections.Generic;

namespace web_app.data.Abstract
{
    public interface IRepository<T>
    {

        T GetProductById(int id);

        List<T> GetAll();


        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);



    }
}
