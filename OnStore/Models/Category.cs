using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnStore.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required, StringLength(100)]
        public string CategoryName { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(500)]
        public string ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public int DisplayOrder { get; set; } = 0;
        // Khóa ngoại
        public virtual ICollection<Product> Products { get; set; }
    }
}