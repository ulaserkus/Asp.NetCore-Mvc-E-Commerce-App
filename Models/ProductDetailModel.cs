using System.Collections.Generic;
using web_app.entity;

namespace App.webui.Models
{
    public class ProductDetailModel
    {
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }
    }
}