using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using TiemSach.Models.EF;
using TiemSach.Models.ProductModel;
using TiemSach.ViewModel.ProductViewModel;

namespace TiemSach.Controllers
{
    public class ProductsManagerController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IImageRepository imageRepository;
        private readonly AppDbContext context;

        public ProductsManagerController(IProductRepository productRepository, ICategoryRepository categoryRepository,
            IWebHostEnvironment webHostEnvironment, IImageRepository imageRepository,
            AppDbContext context)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.webHostEnvironment = webHostEnvironment;
            this.imageRepository = imageRepository;
            this.context = context;
        }
        public IActionResult Index()
        {

            ViewBag.Products = productRepository.Get().ToList();
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = GetCategories();

            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Name = model.Name,

                    CategoryId = model.CategoryId,

                    Description = model.Description,


                    Price = model.Price,

                    Remain = model.Remain,
                    publisher = model.pushisher,
                    DoP = model.DoP
                };

                var createProduct = productRepository.Create(product);

                if (model.ImageFiles != null)
                {
                    var uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "images\\products");
                    foreach (var imageFile in model.ImageFiles)
                    {
                        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                        var filePath = Path.Combine(uploadFolder, fileName);
                        using var fs = new FileStream(filePath, FileMode.Create);
                        imageFile.CopyTo(fs);

                        imageRepository.Create(new Image
                        {
                            ImageId = Guid.NewGuid().ToString(),
                            ProductId = createProduct.ProductId,
                            ImageName = fileName
                        });
                    }
                }

                return RedirectToAction("index", "ProductsManager", new { id = createProduct.ProductId });
            }

            ViewBag.Categories = categoryRepository.Get();

            return View();
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            ViewBag.Categories = categoryRepository.Get();

            var product = productRepository.Get(id);
            ViewBag.Images = (from e in context.Images
                              where e.ProductId == product.ProductId
                              select e).ToList();
            if (product == null) return View("~/Views/Error/PageNotFound.cshtml");
            var editProduct = new EditProductViewModel
            {
                CategoryId = product.CategoryId,
                Description = product.Description,
                ImagesFileName = product.Images,
                Name = product.Name,
                Price = product.Price,
                ProductId = product.ProductId,

                Remain = product.Remain,
                pushisher = product.publisher,
                DoP = product.DoP
            };
            return View(editProduct);
        }
        [HttpPost]
        public IActionResult Edit(EditProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    ProductId = model.ProductId,
                    Description = model.Description,
                    Name = model.Name,
                    Price = model.Price,
                    publisher = model.pushisher,
                    DoP = model.DoP,
                    Remain = model.Remain,
                    CategoryId = model.CategoryId
                };
                var editProduct = productRepository.Edit(product);
                if (model.ImageFiles != null)
                {
                    var uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "images\\products");
                    foreach (var imageFile in model.ImageFiles)
                    {
                        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                        var filePath = Path.Combine(uploadFolder, fileName);
                        using var fs = new FileStream(filePath, FileMode.Create);
                        imageFile.CopyTo(fs);

                        imageRepository.Create(new Image
                        {
                            ImageId = Guid.NewGuid().ToString(),
                            ProductId = editProduct.ProductId,
                            ImageName = fileName
                        });
                    }
                }

                if (editProduct != null) return RedirectToAction("Index", "ProductsManager");
            }

            ViewBag.Categories = categoryRepository.Get();
       
            return View();
        }
        private List<Category> GetCategories()
        {
            return categoryRepository.Get().ToList();
        }
        public IActionResult Remove(string id)
        {
            if (productRepository.Remove(id))
                return RedirectToAction("Index", "ProductsManager");
            return View();
        }
        public IActionResult RemoveImage(string id, string imgId)
        {
            var fileName = (from e in context.Images
                            where e.ImageId == imgId
                            select e).ToList().FirstOrDefault().ImageName;
            imageRepository.Remove(imgId);
            var delFile = Path.Combine(webHostEnvironment.WebRootPath, "images\\products", fileName);
            System.IO.File.Delete(delFile);
            return RedirectToAction("Edit", new { id });
        }
    }
}
