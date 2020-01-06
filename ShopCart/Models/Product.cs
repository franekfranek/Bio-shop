using Microsoft.AspNetCore.Http;
using ShopCart.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopCart.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required, MinLength(2, ErrorMessage = "Minimal length is 2")]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please use letters only")]
        public string Name { get; set; }
        public string Slug { get; set; }
        [Required, MinLength(2, ErrorMessage = "Minimal length is 4")]
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Display(Name ="Category")]
        [Range(1, int.MaxValue, ErrorMessage = "Please choose a category")]
        public int CategoryId { get; set; }
        [FileExtension]
        public string Image { get; set; }
        
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [NotMapped] // means this has nothing to do with the table in db
        public IFormFile ImageUpload { get; set; }
    }
}