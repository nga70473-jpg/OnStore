using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnStore.Models
{ 
    [Table("CartItem")]
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        [Required]
        public int CartId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required] public string ProductName { get; set; }
        [Required] public string ImageUrl { get; set; }


        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; } = 1;

        [Required]
        public decimal Price { get; set; }
        //Navigation properties
        [ForeignKey("CartId")]
        public virtual Cart Cart { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [NotMapped]
        public decimal Total
        {
            get { return Price * Quantity; }
        }

        [NotMapped]
        public decimal Subtotal => Price * Quantity;
    }
}