using System;
using System.Net;
using HexagonalImpl.Domain;
using Xunit;

namespace HexagonalImpl.Tests
{
    public class CartControllerContinuationShould
    {
        [Fact]
        public void Return_error_when_adding_product_without_enough_stock()
        {
            var cartController = new CartControllerContinuation(new FakeProductStocks(0), new FakeCarts());
            var responseMessage = cartController.AddProduct(new AddProduct(Guid.NewGuid(), Guid.NewGuid(), 10));
            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
        }

        [Fact]
        public void Return_error_when_adding_product_with_quantity_more_than_99()
        {
            var cartController = new CartControllerContinuation(new FakeProductStocks(1000), new FakeCarts());
            var responseMessage = cartController.AddProduct(new AddProduct(Guid.NewGuid(), Guid.NewGuid(), 100));
            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
        }

        [Fact]
        public void Return_ok_when_adding_product_without_errors()
        {
            var cartController = new CartControllerContinuation(new FakeProductStocks(15), new FakeCarts());
            var responseMessage = cartController.AddProduct(new AddProduct(Guid.NewGuid(), Guid.NewGuid(), 10));
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
        }
    }

    public class FakeCarts : ICarts
    {
        public Cart Get(Guid cartId)
        {
            return new Cart();
        }

        public void Save(Cart cart)
        {
        }
    }

    public class FakeProductStocks : IProductStocks
    {
        private readonly int _currentStock;

        public FakeProductStocks(int currentStock)
        {
            _currentStock = currentStock;
        }

        public ProductStock Get(Guid productId)
        {
            return new ProductStock(_currentStock);
        }

        public void Save(ProductStock productStock)
        {
        }
    }
}
