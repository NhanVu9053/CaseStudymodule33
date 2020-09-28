using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiemSach.Models.EF;
using TiemSach.ViewModel;

namespace TiemSach.Models.ProductModel
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext context;
        private readonly IImageRepository imageRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductRepository(AppDbContext context,
                                IImageRepository imageRepository,
                                 IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.imageRepository = imageRepository;
            this.webHostEnvironment = webHostEnvironment;
        }
        public Product Create(Product product)
        {
            product.IsDeleted = false;
            product.CreatedTime = DateTime.Now;
            product.ProductId = Guid.NewGuid().ToString();
            context.Products.Add(product);
            context.SaveChanges();
            return product;
        }

        public Product Edit(Product product)
        {
            var editProduct = context.Products.Attach(product);
            editProduct.State = EntityState.Modified;
            context.SaveChanges();
            return product;
        }

        public IEnumerable<Product> Get()
        {
            return (from p in context.Products where p.IsDeleted == false select p).OrderByDescending(
                p => p.CreatedTime);
        }

        public ProductDetailViewModel Get(string id)
        {
            var data = (from e in context.Products
                        where e.IsDeleted == false
                        join d in context.Categories on e.CategoryId equals d.CategoryId

                        where e.ProductId == id
                        select new ProductDetailViewModel
                        {
                            ProductId = e.ProductId,
                            CategoryId = e.CategoryId,
                            CategoryName = d.Name,
                            Description = e.Description,
                            Name = e.Name,
                            Price = e.Price,
                            Remain = e.Remain,
                            publisher = e.publisher,
                            DoP = e.DoP

                        }).FirstOrDefault();
            return data;
        }

        public bool Remove(string id)
        {
            var productToRemove = context.Products.Find(id);
            if (productToRemove != null)
            {
                /*var images = (from e in context.Images
                              where e.ProductId == productToRemove.ProductId
                              select e).ToList();
                foreach (var image in images)
                {
                    imageRepository.Remove(image.ImageId);

                    string delFile = Path.Combine(webHostEnvironment.WebRootPath, "images\\products", image.ImageName);
                    File.Delete(delFile);
                }*/
                productToRemove.IsDeleted = true;
                return context.SaveChanges() > 0;
            }

            return false;
        }
    }
}
