using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestRealWorld.Web.Controllers;
using UnitTestRealWorld.Web.Models;
using Xunit;

namespace UnitTestRealWorld.Test
{
    public class ProductControllerWittSQlite : ProductControllerTest
    {
        public ProductControllerWittSQlite()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            SetContextOptions(new DbContextOptionsBuilder<UnitTestDBContext>().UseSqlite(connection).Options);
        }
        [Fact]
        public async Task Create_ValidModelState_ReturnRedirectToAction()
        {
            var newProduct = new Product { Name = "Oliver Twist", Price = 200, Stock = 2000, Color = "Red", };
            using (var context = new UnitTestDBContext(_contextOptions))
            {
                var category = context.Categories.First();

                newProduct.CategoryId = category.Id;

                var controller = new ProductsController(context);

                var result = await controller.Create(newProduct);

                var redirect = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirect.ActionName);
            }

            using (var context = new UnitTestDBContext(_contextOptions))
            {
                var product = context.Products.FirstOrDefault(x => x.Name == newProduct.Name);

                Assert.Equal(newProduct.Name, product.Name);
            }

        }

        [Theory]
        [InlineData(2)]
        public async Task DeleteCategory_ExistCategoryId_DeleteAllProducts(int categoryId)
        {
            using (var context = new UnitTestDBContext(_contextOptions))
            {
                var category = await context.Categories.FindAsync(categoryId);

                context.Categories.Remove(category);

                context.SaveChanges();
            }

            using (var context = new UnitTestDBContext(_contextOptions))
            {
                var products = await context.Products.Where(x => x.CategoryId == categoryId).ToListAsync();
                Assert.Empty(products);
            }
        }

    }
}
