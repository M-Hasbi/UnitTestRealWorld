using Microsoft.EntityFrameworkCore;
using UnitTestRealWorld.Web.Models;

namespace UnitTestRealWorld.Test
{
    public class ProductControllerTest
    {
        protected DbContextOptions<UnitTestDBContext> _contextOptions { get; private set; }

        public void SetContextOptions(DbContextOptions<UnitTestDBContext> contextOptions)
        {
            _contextOptions = contextOptions;
            Seed();
        }

        public void Seed()
        {
            using (UnitTestDBContext context = new UnitTestDBContext(_contextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Categories.Add(new Category { Id = 1, Name = "Books" });
                context.Categories.Add(new Category { Id = 2, Name = "Notebooks" });
                context.SaveChanges();


               // context.Products.Add(new Product { CategoryId = 2, Name = "Book10", Color = "Red", Price = 200, Stock = 2000 });
               // context.Products.Add(new Product { CategoryId = 2, Name = "Book20", Color = "Blue", Price = 200, Stock = 2000 });
                context.SaveChanges();
            }
        }
    }
}
