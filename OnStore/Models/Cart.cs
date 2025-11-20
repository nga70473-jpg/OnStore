using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnStore.Models
{
    [Table("Cart")]
    public class Cart
    {
        [Key, ForeignKey("User")]
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation
        public virtual Users User { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }

        [NotMapped]
        public decimal TotalAmount
        {
            get
            {
                decimal total = 0;
                if (CartItems != null)
                {
                    foreach (var item in CartItems)
                    {
                        total += item.Price * item.Quantity;
                    }
                }
                return total;
            }
        }
    }
}