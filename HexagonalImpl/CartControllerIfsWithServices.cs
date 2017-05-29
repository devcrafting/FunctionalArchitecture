using System;
using System.Net;
using System.Net.Http;
using HexagonalImpl.Domain;

namespace HexagonalImpl
{
    public class CartControllerIfsWithServices
    {
        private readonly ProductStockService _productStockService;
        private readonly CartService _cartService;

        public CartControllerIfsWithServices(ProductStockService productStockService, CartService cartService)
        {
            _productStockService = productStockService;
            _cartService = cartService;
        }

        public HttpResponseMessage AddProduct(AddProduct addProduct)
        {
            var temporaryReservation = _productStockService.MakeATemporaryReservation(addProduct);
            if (temporaryReservation != null)
            {
                if(!_cartService.Add(addProduct, temporaryReservation))
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
