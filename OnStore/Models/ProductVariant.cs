using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnStore.Models
{
    [Table("ProductVariant")]
    public class ProductVariant
    {
        [Key]
        public int VariantId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required, StringLength(100)]
        public string Color { get; set; }

        [StringLength(100)]
        public string Storage { get; set; }

        [StringLength(100)]
        public string RAM { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Stock { get; set; } = 0;

        public ProductStatus Status { get; set; } = ProductStatus.OutOfStock;

        [Required, StringLength(255)]
        public string ImageUrl { get; set; }

        // Navigation

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

    }
    public enum ProductStatus
    {
        InStock = 1,
        OutOfStock = 2,
        Discontinued = 3
    }
}