using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using UnitTestRealWorld.Web.Controllers;
using UnitTestRealWorld.Web.Models;
using UnitTestRealWorld.Web.Repository;
using Xunit;

namespace UnitTestRealWorld.Test
{
    public class ProductControllerTest
    {
        private readonly Mock<IRepository<Product>> _mockRepo;

        private readonly ProductsController _controller;

        private List<Product> products;


        public ProductControllerTest()
        {
            _mockRepo = new Mock<IRepository<Product>>();

            _controller = new ProductsController(_mockRepo.Object);

            products = new List<Product>()
            {
              new Product() { Id = 1, Name = "Pencil", Price = 12, Color = "Red", Stock=100 },
              new Product() { Id = 1, Name = "Book", Price = 15, Color = "Blue", Stock=100 }
            };
        }
        [Fact]
        public async void Index_ActionExecutes_ReturnView()
        {
            var result = await _controller.Index();

            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public async void Index_ActionExecutes_ReturnProductList()
        {
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            var result = await _controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            var productList = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model);

            Assert.Equal(2, productList.Count());
        }
    }
}
