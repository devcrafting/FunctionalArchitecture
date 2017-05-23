using System;
using System.Net;
using System.Net.Http;
using HexagonalImpl.Domain;

namespace HexagonalImpl
{
    public class CartControllerException
    {
        private readonly IProductStocks _productStocks;
        private readonly ICarts _carts;

        public CartControllerException(IProductStocks productStocks, ICarts carts)
        {
            _productStocks = productStocks;
            _carts = carts;
        }

        public HttpResponseMessage AddProduct(AddProduct addProduct)
        {
            var productStock = _productStocks.Get(addProduct.ProductId);
            try
            {
                var temporaryReservation = productStock.MakeATemporaryReservation(addProduct.Quantity);
                _productStocks.Save(productStock);
                var cart = _carts.Get(addProduct.CartId);
                cart.Add(addProduct.ProductId, addProduct.Quantity, temporaryReservation);
                _carts.Save(cart);
            }
            catch (BusinessException e)
            {
                var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.Forbidden)
                {
                    Content = new StringContent(e.Message)
                };
                return httpResponseMessage;
            }
            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}
