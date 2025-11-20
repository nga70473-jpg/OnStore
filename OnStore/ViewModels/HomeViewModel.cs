using OnStore.Models;
using OnStore.ViewModels;
using System;
using System.Collections.Generic;

namespace OnStore.ViewModels
{
    public class HomeViewModel
    {
        // Banner slides
        public List<BannerSlide> Banners { get; set; }

        // Flash Sale products
        public List<Product> FlashSaleProducts { get; set; }

        // Best Seller products (30 ngày gần nhất)
        public List<BestSellerProductViewModel> BestSellerProducts { get; set; }
        public Dictionary<string, List<BestSellerProductViewModel>> ProductsByBrand { get; set; }
        public Dictionary<string, List<Product>> FeaturedProductsByCategory { get; set; }
    }

    // Class cho Banner Slide
    public class BannerSlide
    {
        public int BannerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string LinkUrl { get; set; }
        public int DisplayOrder { get; set; }
    }

    // Class cho Best Seller
   
}
