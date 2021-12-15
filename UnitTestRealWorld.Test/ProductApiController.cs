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
    public class ProductApiController
    {
        private readonly Mock<IRepository<Product>> _mockRepo;
        private readonly ProductsApiController _controller;

        private Product GetFirstProduct(int productId)
        {
            Product product = products.First(x => x.Id == productId);
            return product;
        }

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
        [Theory]
        [InlineData(0)]
        public async void GetProduct_InValidId_ReturnNotFound(int productId)
        {
            Product product = null;

            _mockRepo.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);

            var result = await _controller.GetProduct(productId);

            Assert.IsType<NotFoundResult>(result);

        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetProduct_ValidId_ReturnOkWithProduct(int productId)
        {
            var product = GetFirstProduct(productId);

            _mockRepo.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);

            var result = await _controller.GetProduct(productId);

            var returnOk = Assert.IsType<OkObjectResult>(result);

            var returnProduct = Assert.IsType<Product>(returnOk.Value);

            Assert.Equal(productId, returnProduct.Id);
            Assert.Equal(product.Name, returnProduct.Name);
        }
        [Theory]
        [InlineData(1)]
        public void PutProduct_IdIsNotEqualToProduct_ReturnBadRequest(int productId)
        {
            var product = GetFirstProduct(productId);

            var result = _controller.PutProduct(2,product);

            Assert.IsType<BadRequestResult>(result);
        }
        [Theory]
        [InlineData(1)]
        public void PutProduct_ActionExecutes_ReturnNoContent(int productId)
        {
            var product = GetFirstProduct(productId);

            _mockRepo.Setup(x => x.Update(product));

            var result = _controller.PutProduct(productId, product);

            _mockRepo.Verify(x => x.Update(product), Times.Once);

            Assert.IsType<NoContentResult>(result);
        }

    }
}
