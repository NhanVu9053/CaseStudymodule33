using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TiemSach.Models;
using TiemSach.Models.EF;
using TiemSach.Models.OrderModel;
using TiemSach.Models.ProductModel;
using TiemSach.Models.UserModel;
using X.PagedList;

namespace TiemSach.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly AppDbContext context;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IOrderRepository orderRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IOrderDetailRepository orderDetailRepository;

        public HomeController(ILogger<HomeController> logger,
            IProductRepository productRepository,        
            ICategoryRepository categoryRepository,
            AppDbContext context,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository,
            IOrderDetailRepository orderDetailRepository
            )
        {
            _logger = logger;
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.context = context;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.orderRepository = orderRepository;
            this.customerRepository = customerRepository;
            this.orderDetailRepository = orderDetailRepository;
        }

        public IActionResult Index(int? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            ViewBag.Categories = categoryRepository.Get().ToList();
            ViewBag.Products = productRepository.Get().ToList();
            var result = productRepository.Get().ToPagedList(pageNumber, pageSize);
            return View(result);
        }

        public IActionResult ViewProduct(string id)
        {
            var product = productRepository.Get(id);
            if (product == null)
            {
                ViewBag.ErrorMessage = "Không tìm thấy sản phẩm!";
                return View("~/Views/Error/PageNotFound.cshtml");
            }
            ViewBag.Images = (from e in context.Images
                              where e.ProductId == product.ProductId
                              select e).ToList();
            var relatedProducts = (from p in context.Products
                                   where p.CategoryId == product.CategoryId                                
                                  select p).ToList();
            relatedProducts.Remove(context.Products.Find(id));          
            ViewBag.Categories = categoryRepository.Get().ToList();
            ViewBag.RelatedProducts = relatedProducts.Take(6);
            return View(product);
        }


        public IActionResult Category(int id, int page = 1)
        {
            var products = (from p in context.Products where p.CategoryId == id && p.Remain > 0 select p).ToList();
            ViewBag.Categories = (from c in context.Categories select c).ToList();         
            var categories = (from c in context.Categories where c.CategoryId == id select c).ToList();
            if (categories.Count != 0)
                ViewBag.title = categories.FirstOrDefault().Name;
            else
                ViewBag.title = "Không có danh mục!";

            ViewBag.CategoryId = id;
            ViewBag.Count = products.Count;
            ViewBag.Page = page;
            return View(products.Skip(page * 12 - 12).Take(12).ToList());
        }

        public IActionResult Search(string search)
        {
            var data = productRepository.Search(search);
            return View(data);
        }

    }
}
