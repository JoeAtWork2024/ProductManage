using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductManage.Models;
using System.Diagnostics;
using System.Text.Json;
using NLog;
using NLog.Web;

namespace ProductManage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AdventureWorksLT2022Context _context;

        public HomeController(ILogger<HomeController> logger, AdventureWorksLT2022Context context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
           return View();
        }

        public IActionResult ManageProduct()
        {
            //進畫面先全部查
            _logger.LogInformation("ManageProduct-Start");
            List<Product> products = new List<Product>();
            try
            {
                products = _context.Products.ToList();
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
            }
            _logger.LogInformation("ManageProduct-End");
            return View(products);
        }

        [HttpGet]
        public IActionResult ManageProduct(string? productName, int? productId)
        {
            //有條件的查詢
            _logger.LogInformation("ManageProduct-Start");
            List<Product> products = new List<Product>();
            try
            {
                products = _context.Products.Where(x=>x.ProductId==productId || productId==null).Where(x=>x.Name.Contains(productName) || productName==null).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            _logger.LogInformation("ManageProduct-End");
            return View(products);
        }

        public IActionResult EditProduct(int id)
        {
            var product = _context.Products.Where(x=>x.ProductId == id).FirstOrDefault();
            return View(product);
        }

        [HttpPost]
        public IActionResult EditProductSave([FromBody] QueryRequest? viewProduct)
        {
            _logger.LogInformation("EditProductSave-Start");
            try
            {
                if (ModelState.IsValid)
                {
                    var product = _context.Products.Where(x=>x.ProductId == viewProduct.ProductId).FirstOrDefault();
                    product.ModifiedDate = DateTime.Now;
                    product.Size = viewProduct.Size;
                    product.Name = viewProduct.Name;
                    product.ListPrice = decimal.Parse(viewProduct.ListPrice.ToString());
                    product.StandardCost = decimal.Parse(viewProduct.StandardCost.ToString());
                    product.Weight = viewProduct.Weight;
                    product.ProductNumber = viewProduct.ProductNumber;
                    product.DiscontinuedDate = viewProduct.DiscontinuedDate;
                    product.SellEndDate = viewProduct.SellEndDate;
                    product.SellStartDate = viewProduct.SellStartDate;
                    _context.Products.Update(product);
                    _context.SaveChanges();
                }
                else
                {
                    //輸入的值有問題
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                                      .Select(e => e.ErrorMessage)
                                      .ToList();
                    return Json(new { success = false, message = "儲存失敗-" + errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new { success = false, message = ex.Message });
            }

            return Json(new { success = true, message = "" });
        }

        public IActionResult SaveSuccess()
        {
            return View();
        }


        [HttpPost]
        public string ApiTest([FromBody] Product? request)
        {
            return JsonSerializer.Serialize("!!"); 
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
