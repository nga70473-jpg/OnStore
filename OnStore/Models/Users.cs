using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnStore.Models
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress, StringLength(255)]
        public string Email { get; set; }

        [Required, StringLength(255)]
        public string Password { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        public string Address { get; set; }

        [Required, StringLength(20)]
        public string Role { get; set; } = "Customer";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation
        [ForeignKey("UserId")]
        public virtual Cart Cart { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ProductReview> ProductReviews { get; set; }  
    }
}