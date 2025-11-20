using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Drawing2D;
using System.Linq;

namespace OnStore.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(255)]
        public string ProductName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        // ========== GIÁ VÀ GIẢM GIÁ ==========

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } // Giá hiện tại

        [Column(TypeName = "decimal(18,2)")]
        public decimal? OriginalPrice { get; set; } // Giá gốc (trước khi giảm)

        [Range(0, 100)]
        public int? DiscountPercent { get; set; } // % giảm giá (0-100)

        // ========== THÔNG TIN SẢN PHẨM ==========

        [StringLength(500)]
        public string ImageUrl { get; set; }

        [StringLength(100)]
        public string Brand { get; set; } // Apple, Samsung, Dell, HP, Asus...

        [Required]
        public int Stock { get; set; } // Số lượng tồn kho

        [Required]
        public bool IsActive { get; set; } // Còn kinh doanh không

        public bool IsFeatured { get; set; } // Sản phẩm nổi bật

        // ========== FLASH SALE ==========

        public bool IsFlashSale { get; set; } // Có đang flash sale không

        public DateTime? FlashSaleStart { get; set; } // Thời gian bắt đầu flash sale

        public DateTime? FlashSaleEnd { get; set; } // Thời gian kết thúc flash sale

        // ========== THỐNG KÊ ==========

        public int ViewCount { get; set; } = 0; // Số lượt xem

        public int SoldCount { get; set; } = 0; // Tổng số lượng đã bán

        // ========== TIMESTAMP ==========

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // ========== NAVIGATION PROPERTIES ==========

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
       
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<ProductVariant> ProductVariants { get; set; }
        public virtual ICollection<ProductSpec> ProductSpecs { get; set; }
        public virtual ICollection<ProductTag> ProductTags { get; set; }
        public virtual ICollection<ProductReview> ProductReviews { get; set; }

        // ========== COMPUTED PROPERTIES (Không lưu vào DB) ==========

        /// <summary>
        /// Giá cuối cùng sau khi áp dụng giảm giá
        /// </summary>
        [NotMapped]
        public decimal FinalPrice
        {
            get
            {
                if (DiscountPercent.HasValue && DiscountPercent > 0)
                {
                    return Price * (1 - (decimal)DiscountPercent.Value / 100);
                }
                return Price;
            }
        }

        /// <summary>
        /// Kiểm tra có đang trong thời gian Flash Sale không
        /// </summary>
        [NotMapped]
        public bool IsInFlashSale
        {
            get
            {
                if (!IsFlashSale || !FlashSaleStart.HasValue || !FlashSaleEnd.HasValue)
                    return false;

                var now = DateTime.Now;
                return now >= FlashSaleStart && now <= FlashSaleEnd;
            }
        }

        /// <summary>
        /// Điểm đánh giá trung bình
        /// </summary>
        [NotMapped]
        public double AverageRating
        {
            get
            {
                if (ProductReviews != null && ProductReviews.Any())
                {
                    return ProductReviews.Average(r => r.Rating);
                }
                return 0;
            }
        }

        /// <summary>
        /// Tổng số đánh giá
        /// </summary>
        [NotMapped]
        public int TotalReviews
        {
            get
            {
                return ProductReviews?.Count ?? 0;
            }
        }

        public object BrandId { get; internal set; }
    }
}