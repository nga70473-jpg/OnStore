using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using OnStore.Models;
using OnStore.ViewModels;

namespace OnStore.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // Action Index - Trang chủ
        public ActionResult Index()
        {
            var viewModel = new HomeViewModel
            {
                // 1. Banner Slides
                Banners = GetBanners(),

                // 2. Flash Sale Products
                FlashSaleProducts = GetFlashSaleProducts(),

                // 3. Best Seller Products (30 ngày)
                BestSellerProducts = GetBestSellerProducts(30),

                // 4. Sản phẩm nổi bật theo Category
                FeaturedProductsByCategory = GetFeaturedProductsByCategory()
            };
            System.Diagnostics.Debug.WriteLine($"Best Seller Count: {viewModel.BestSellerProducts.Count}");

            if (viewModel.BestSellerProducts != null)
            {
                for (int i = 0; i < viewModel.BestSellerProducts.Count; i++)
                {
                    viewModel.BestSellerProducts[i].Rank = i + 1;
                }
            }

            return View(viewModel);
        }

        #region Private Helper Methods

        /// <summary>
        /// Lấy danh sách Banner cho slideshow
        /// </summary>
        private List<BannerSlide> GetBanners()
        {
            return new List<BannerSlide>
            {
                new BannerSlide
                {
                    BannerId = 1,
                    Title = "iPhone 15 Pro Max",
                    Description = "Giảm giá đến 20% - Chỉ hôm nay!",
                    ImageUrl = "/Images/banners/banner1.jpg",
                    LinkUrl = "/Product/Details/1",
                    DisplayOrder = 1
                },
                new BannerSlide
                {
                    BannerId = 2,
                    Title = "Samsung Galaxy S24 Ultra",
                    Description = "Flagship mới nhất - Trả góp 0%",
                    ImageUrl ="/Images/banners/banner2.jpg",
                    LinkUrl = "/Product/Details/2",
                    DisplayOrder = 2
                },
                new BannerSlide
                {
                    BannerId = 3,
                    Title = "MacBook Pro M3",
                    Description = "Hiệu năng vượt trội - Giá tốt nhất",
                    ImageUrl = "/Images/banners/banner3.jpg",
                    LinkUrl = "/Product/Details/3",
                    DisplayOrder = 3
                }
            };
        }

        /// <summary>
        /// Lấy sản phẩm Flash Sale đang diễn ra
        /// </summary>
        private List<Product> GetFlashSaleProducts()
        {
            try
            {
                var now = DateTime.Now;

                // ✅ BỎ Include - Lấy Products về memory trước
                var allProducts = db.Products
                    .Where(p => p.IsActive == true)
                    .ToList();  // ← Load về memory ngay

                // Filter trong memory
                var products = allProducts
                    .Where(p => p.IsFlashSale == true
                        && p.FlashSaleStart <= now
                        && p.FlashSaleEnd >= now
                        && p.Stock > 0)
                    .OrderByDescending(p => p.DiscountPercent)
                    .Take(8)
                    .ToList();

                // Nếu không có Flash Sale, lấy sản phẩm có giảm giá
                if (!products.Any())
                {
                    products = allProducts
                        .Where(p => p.Stock > 0 && p.DiscountPercent > 0)
                        .OrderByDescending(p => p.DiscountPercent)
                        .Take(8)
                        .ToList();
                }

                return products;
            }
            catch
            {
                return new List<Product>();
            }
        }
        /// <summary>
        /// Thuật toán Best Seller: Sản phẩm bán chạy nhất trong X ngày
        /// </summary>
        private List<BestSellerProductViewModel> GetBestSellerProducts(int days)
        {
            try
            {
                var fromDate = DateTime.Now.AddDays(-days);

                // Lấy từ OrderItems
                var bestSellers = db.OrderItems
                    .Where(oi => oi.Order.CreatedAt >= fromDate)
                    .ToList()
                    .GroupBy(oi => oi.ProductId)
                    .Select(g => new
                    {
                        ProductId = g.Key,
                        TotalSold = g.Sum(oi => oi.Quantity),
                        TotalRevenue = g.Sum(oi => oi.Quantity * oi.Price),
                        OrderCount = g.Count()
                    })
                    .OrderByDescending(x => x.TotalSold)
                    .Take(10)
                    .ToList();

                if (bestSellers.Any())
                {
                    var productIds = bestSellers.Select(bs => bs.ProductId).ToList();
                    var products = db.Products
                        .Where(p => productIds.Contains(p.ProductId) && p.IsActive == true)
                        .ToDictionary(p => p.ProductId, p => p);

                    var result = bestSellers
                        .Where(bs => products.ContainsKey(bs.ProductId))
                        .Select(bs => new BestSellerProductViewModel
                        {
                            ProductId = bs.ProductId,
                            ProductName = products[bs.ProductId].ProductName,
                            ImageUrl = products[bs.ProductId].ImageUrl,
                            Price = products[bs.ProductId].Price,
                            TotalSold = bs.TotalSold,
                            TotalRevenue = bs.TotalRevenue,
                            OrderCount = bs.OrderCount
                        }).ToList();

                    return result;
                }

                // Fallback - Lấy từ SoldCount
                var fallbackProducts = db.Products
                    .Where(p => p.IsActive == true && p.Stock > 0)
                    .OrderByDescending(p => p.SoldCount)
                    .Take(10)
                    .ToList();

                return fallbackProducts.Select(p => new BestSellerProductViewModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    TotalSold = p.SoldCount,
                    TotalRevenue = p.SoldCount * p.Price,
                    OrderCount = 0
                }).ToList();
            }
            catch
            {
                return new List<BestSellerProductViewModel>();
            }
        }

        /// <summary>
        /// Lấy sản phẩm nổi bật theo từng Category
        /// </summary>
        private Dictionary<string, List<Product>> GetFeaturedProductsByCategory()
        {
            try
            {
                // ✅ Lấy tất cả sản phẩm về memory
                var products = db.Products
                    .Where(p => p.IsActive == true && p.IsFeatured == true)
                    .OrderByDescending(p => p.ViewCount)
                    .Take(50)
                    .ToList();  // ← Load về memory

                // Load Category cho mỗi product
                foreach (var product in products)
                {
                    if (product.CategoryId > 0)
                    {
                        product.Category = db.Categories.Find(product.CategoryId);
                    }
                }

                // Group trong memory
                var featuredProducts = products
                    .GroupBy(p => p.Category?.CategoryName ?? "Khác")
                    .ToDictionary(
                        g => g.Key,
                        g => g.Take(10).ToList()
                    );

                return featuredProducts;
            }
            catch
            {
                return new Dictionary<string, List<Product>>();
            }
        }

        #endregion

        // Dispose DbContext khi Controller bị destroy
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}