using System;

namespace HexagonalImpl.Domain
{
    public class Cart
    {
        // bool return type is only relevant with CartControllerIfs implementation
        public bool Add(Guid productId, int quantity, TemporaryReservation temporaryReservation)
        {
            throw new NotImplementedException();
        }

        public EitherVoid<Error> Add_(Guid productId, int quantity, TemporaryReservation temporaryReservation)
        {
            throw new NotImplementedException();
        }
    }
}