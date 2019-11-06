using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NorthwindConsole.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "** Name must be entered")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "** Description must be entered")]
        public string Description { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}
