using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnitTestRealWorld.Web.Controllers;
using UnitTestRealWorld.Web.Models;
using UnitTestRealWorld.Web.Repository;
using Xunit;

namespace UnitTestRealWorld.Test
{
    public class ProductApiController
    {
        private readonly Mock<IRepository<Product>> _mockRepo;
        private readonly ProductsApiController _controller;

        private List<Product> products;
        public ProductApiController()
        {
            _mockRepo = new Mock<IRepository<Product>>();
            _controller = new ProductsApiController(_mockRepo.Object);
            products = new List<Product>()
            {
                new Product { Id = 1, Name = "Pencil", Price = 12, Color = "Red", Stock = 100 },
                new Product { Id = 2, Name = "Book", Price = 15, Color = "Blue", Stock = 100 }
            };


        }
        [Fact]
        public async void GetProduct_ExecutesAction_ReturnsOkWithProduct()
        {
            _mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(products);
            var result = await _controller.GetProducts();
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Equal<int>(2, returnProducts.ToList().Count);
        }


    }
}
