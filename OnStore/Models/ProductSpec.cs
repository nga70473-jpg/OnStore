using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace OnStore.Models
{
    [Table("ProductSpec")]
    public class ProductSpec
    {
        [Key]
        public int SpecId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required, StringLength(255)]
        public string Attribute { get; set; }
        [Required, StringLength(600)]
        public string Value { get; set; }
        // Navigation
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}