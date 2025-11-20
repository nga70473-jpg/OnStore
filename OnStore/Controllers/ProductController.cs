using OnStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnStore.Controllers
{
    public class ProductController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // /Product
        public ActionResult Index()
        {
            var products = db.Products.ToList();
            return View("ProductList", products);
        }

        // /Product/Detail/5
        public ActionResult ProductDetail(int id)
        {
            var product = db.Products
                            .FirstOrDefault(x => x.ProductId == id);

            if (product == null) return HttpNotFound();

            // Lấy Tag của sản phẩm
            var tags = db.ProductTags
                         .Where(pt => pt.ProductId == id)
                         .Select(pt => pt.Tag)
                         .ToList();

            ViewBag.Tags = tags;
            return View(product);
        }

        // /Product/ByTag?tags=1,3,5
        public ActionResult ByTag(string tags)
        {
            if (string.IsNullOrEmpty(tags)) return RedirectToAction("Index");

            var tagIds = tags.Split(',').Select(int.Parse).ToList();

            var products = db.ProductTags
                .Where(pt => tagIds.Contains(pt.TagId))
                .Select(pt => pt.Product)
                .Distinct()
                .ToList();

            ViewBag.FilterTags = db.Tags.Where(t => tagIds.Contains(t.TagId)).ToList();

            return View("ProductList", products);
        }

        // Partial View — Card template
        public PartialViewResult ProductCard(Product model)
        {
            return PartialView("_ProductCard", model);
        }
    }
}