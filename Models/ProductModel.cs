using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using web_app.entity;

namespace App.webui.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        // [Display(Name="Name" , Prompt="Enter Product Name")]
        // [Required(ErrorMessage="İsim zorunlu bir alan")]
        // [StringLength(60,MinimumLength=5,ErrorMessage="Ürün ismi 5 ile 10 karakter arasında olmalıdır")]
        public string Name { get; set; }
         [Required(ErrorMessage="Url zorunlu bir alan")]
        public string Url { get; set; }
         [Required(ErrorMessage="Price zorunlu bir alan")]
         [Range(1,10000,ErrorMessage="Fiyat için 1 ile 10000 arasında bir sayı giriniz")]
        public double Price { get; set; }

        public bool IsHome { get; set; }
        [Required(ErrorMessage="Description zorunlu bir alan")]
        [StringLength(100,MinimumLength=5,ErrorMessage="Ürün ismi 5 ile 100 karakter arasında olmalıdır")]
        public string Description { get; set; }
         
        [Required(ErrorMessage="ImageUrl zorunlu bir alan")]
        public string ImageUrl { get; set; }

        public bool IsApproved { get; set; }

        public List<Category> SelectedCategories { get; set; }

    }
}