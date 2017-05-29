using System;

namespace HexagonalImpl.Domain
{
    public class AddProduct
    {
        public AddProduct(Guid cartId, Guid productId, int quantity)
        {
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
        }

        public Guid CartId { get; private set; }

        public Guid ProductId { get; private set; }

        public int Quantity { get; private set; }
    }
}