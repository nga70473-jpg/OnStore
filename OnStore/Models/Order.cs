using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace OnStore.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public int UserId { get; set; }

        [Required, StringLength(100)]
        public string CustomerName { get; set; }

        [Required, StringLength(255)]
        public string CustomerAddress { get; set; }

        [Required, StringLength(100), EmailAddress]
        public string CustomerEmail { get; set; }

        [Required, StringLength(20)]
        public string CustomerPhone { get; set; }

        // Tổng tiền đơn hàng
        [Required]
        public decimal TotalPrice { get; set; }
        [StringLength(50)]
        public string Status { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<PaymentMethod> PaymentMethods { get; set; }

        [ForeignKey("UserId")]
        public virtual Users User { get; set; }
    }
}