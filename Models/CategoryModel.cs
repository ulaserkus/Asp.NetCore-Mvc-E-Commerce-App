using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using web_app.entity;

namespace App.webui.Models
{
    public class CategoryModel
    {
        
        public int CategoryId { get; set; }
         
        [Required]
        [StringLength(100,MinimumLength=5,ErrorMessage="Kategori için 5-100 arasında bir İsim giriniz")] 
        public string Name { get; set; }


        public string Description { get; set; }
       
        [Required]
        public string Url { get; set; }

       public List<Product> Products { get; set; }
    }
}