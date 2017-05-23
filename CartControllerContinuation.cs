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
            HttpResponseMessage response = null;
            var productStock = _productStocks.Get(addProduct.ProductId);
            var eitherTemporaryReservationErrors = productStock.MakeATemporaryReservation_(addProduct.Quantity);
            eitherTemporaryReservationErrors.ContinueWith(temporaryReservation =>
            {
                _productStocks.Save(productStock);
                var cart = _carts.Get(addProduct.CartId);
                var eitherVoidErrors = cart.Add_(addProduct.ProductId, addProduct.Quantity, temporaryReservation);
                eitherVoidErrors.ContinueWith(() =>
                {
                    _carts.Save(cart);
                    response = new HttpResponseMessage(HttpStatusCode.Created);
                }, x => response = new HttpResponseMessage(HttpStatusCode.Forbidden));
            }, x => response = new HttpResponseMessage(HttpStatusCode.Forbidden));
            return response;
        }
    }
}
