using OnStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnStore.ViewModels
{
    public class BestSellerProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public Product Product { get; set; }
        public int TotalSold { get; set; } // Số lượng đã bán trong 30 ngày
        public decimal TotalRevenue { get; set; } // Doanh thu trong 30 ngày
        public int OrderCount { get; internal set; }
        public int Rank { get; internal set; }
        public string BadgeText
        {
            get
            {
                if (Rank == 1) return "🥇 Top 1";
                if (Rank == 2) return "🥈 Top 2";
                if (Rank == 3) return "🥉 Top 3";
                return $"#{Rank}";
            }
        }

        public string BadgeColor
        {
            get
            {
                if (Rank == 1) return "#FFD700";
                if (Rank == 2) return "#C0C0C0";
                if (Rank == 3) return "#CD7F32";
                return "#ffc107";
            }
        }
    }
}