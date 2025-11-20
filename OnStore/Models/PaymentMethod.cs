using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnStore.Models
{
    [Table("PaymentMethod")]
    public class PaymentMethod
    {
        [Key]
        public int PaymentMethodId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public decimal Amount{ get; set; }

        [Required]
        public MethodType Method { get; set; } = MethodType.COD; // COD, CreditCard, VNPAY, Momo

        [Required]
        public PaymentStatus Status{ get; set; } // Pending, Success, Failed
        [Required, StringLength(100)]
        public string TransactionId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

    }
    public enum MethodType
    {
        COD,
        CreditCard,
        VNPAY,
        Momo
    }
    public enum PaymentStatus
    {
        Pending,
        Success,
        Failed
    }
}