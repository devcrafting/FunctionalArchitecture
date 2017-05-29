using System;

namespace HexagonalImpl.Domain
{
    public interface ICarts
    {
        Cart Get(Guid cartId);
        void Save(Cart cart);
    }
}