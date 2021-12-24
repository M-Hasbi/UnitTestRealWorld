using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestRealWorld.Web.Models;

namespace UnitTestRealWorld.Test
{
    public class ProductControllerTest
    {
        protected DbContextOptions<UnitTestDBContext> _contextOptions { get; private set; }

        public void SetContextOptions(DbContextOptions<UnitTestDBContext> contextOptions)
        {
            _contextOptions = contextOptions;
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


                context.Products.Add(new Product { CategoryId = 1, Name = "Book1" });
                context.SaveChanges();
            }
        }
    }
}
