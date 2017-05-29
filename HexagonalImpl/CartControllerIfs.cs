using System;
using System.Net;
using System.Net.Http;
using HexagonalImpl.Domain;

namespace HexagonalImpl
{
    public class CartControllerIfs
    {
        private readonly IProductStocks _productStocks;
        private readonly ICarts _carts;

        public CartControllerIfs(IProductStocks productStocks, ICarts carts)
        {
            _productStocks = productStocks;
            _carts = carts;
        }

        public HttpResponseMessage AddProduct(AddProduct addProduct)
        {
            var productStock = _productStocks.Get(addProduct.ProductId);
            var temporaryReservation = productStock.MakeATemporaryReservation(addProduct.Quantity);
            if (temporaryReservation != null)
            {
                _productStocks.Save(productStock);
                var cart = _carts.Get(addProduct.CartId);
                if (cart.Add(addProduct.ProductId, addProduct.Quantity, temporaryReservation))
                {
                    _carts.Save(cart);
                }
                else
                {
                    return Error("Sorry, cannot add to cart");
                }
            }
            else
            {
                return Error("Sorry, not enough stock");
            }
            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        private static HttpResponseMessage Error(string message)
        {
            return new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                Content = new StringContent(message)
            };
        }
    }
}
