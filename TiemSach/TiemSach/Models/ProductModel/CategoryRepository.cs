using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiemSach.Models.EF;

namespace TiemSach.Models.ProductModel
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext context;

        public CategoryRepository(AppDbContext context)
        {
            this.context = context;
        }
        public Category Create(Category category)
        {
            category.IsDeleted = false;
            context.Categories.Add(category);
            context.SaveChanges();
            return category;
        }

        public Category Edit(Category category)
        {
            var editCategory = context.Categories.Attach(category);
            editCategory.State = EntityState.Modified;
            context.SaveChanges();
            return category;
        }

        public IEnumerable<Category> Get()
        {
            return from c in context.Categories where c.IsDeleted == false select c;
        }

        public Category Get(int id)
        {
            var category = (from e in context.Categories
                            where e.IsDeleted == false
                                  && e.CategoryId == id
                            select e).FirstOrDefault();
            return category;
        }

        public bool Remove(int id)
        {
            var categoryToRemove = context.Categories.Find(id);
            if (categoryToRemove != null)
            {
                var productsInCategory = from p in context.Products where p.CategoryId == id select p;
                if (productsInCategory.ToList().Count > 0)
                    return false;
                context.Categories.Remove(categoryToRemove);
                return context.SaveChanges() > 0;
            }

            return false;
        }
    }
}
