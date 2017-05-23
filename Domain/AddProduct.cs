using System;

namespace HexagonalImpl.Domain
{
    public class AddProduct
    {
        public Guid CartId { get; private set; }

        public Guid ProductId { get; private set; }

        public int Quantity { get; private set; }
    }
}