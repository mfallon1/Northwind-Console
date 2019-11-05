using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NorthwindConsole.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "YO - Enter the name!")]
        public string CategoryName { get; set; }

 //       [StringLength(50)]   // limits the description to 50 char
        public string Description { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}
