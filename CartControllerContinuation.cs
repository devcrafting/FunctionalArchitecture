using System;
using System.Net;
using System.Net.Http;
using HexagonalImpl.Domain;

namespace HexagonalImpl
{
    public class CartControllerContinuation
    {
        private readonly IProductStocks _productStocks;
        private readonly ICarts _carts;

        public CartControllerContinuation(IProductStocks productStocks, ICarts carts)
        {
            _productStocks = productStocks;
            _carts = carts;
        }

        public HttpResponseMessage AddProduct(AddProduct addProduct)
        {
            var productStock = _productStocks.Get(addProduct.ProductId);
            return productStock
                .MakeATemporaryReservation_(addProduct.Quantity)
                .ContinueWith(temporaryReservation =>
                    {
                        _productStocks.Save(productStock);
                        var cart = _carts.Get(addProduct.CartId);
                        return cart.Add_(addProduct.ProductId, addProduct.Quantity, temporaryReservation);
                    })
                .ContinueWith(cart =>
                    {
                        _carts.Save(cart);
                        return Either<HttpResponseMessage, Error>.Left(new HttpResponseMessage(HttpStatusCode.OK));
                    })
                .OnError(error => new HttpResponseMessage(HttpStatusCode.BadRequest))
                .Result();
        }
    }
}
