using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopCart.Models
{
    public class Page
    {
        public int Id { get; set; }
        [Required, MinLength(2, ErrorMessage = "Minimal length is 2")] // it provides basic validation for forms(html) etc
        //[Display(Name = "DUPA")]
        public string Title { get; set; }
        public string Slug { get; set; }
        [Required, MinLength(4, ErrorMessage = "Minimal length is 4")]
        public string Content { get; set; }
        public int Sorting { get; set; }
    }
}
