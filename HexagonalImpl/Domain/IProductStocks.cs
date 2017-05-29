using System;

namespace HexagonalImpl.Domain
{
    public interface IProductStocks
    {
        ProductStock Get(Guid productId);

        void Save(ProductStock productStock);
    }
}