using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiemSach.Models.EF;

namespace TiemSach.Models.ProductModel
{
    public class ImageRepository : IImageRepository
    {
        private readonly AppDbContext context;

        public ImageRepository(AppDbContext context)
        {
            this.context = context;
        }
        public Image Create(Image image)
        {
            context.Images.Add(image);
            context.SaveChanges();
            return image;
        }

        public Image Get(string id)
        {
            var image = (from e in context.Images
                         where e.ImageId == id
                         select e).FirstOrDefault();
            return image;
        }

        public bool Remove(string id)
        {
            var imageToRemove = context.Images.Find(id);
            if (imageToRemove != null)
            {
                context.Images.Remove(imageToRemove);
                return context.SaveChanges() > 0;
            }

            return false;
        }
    }
}
