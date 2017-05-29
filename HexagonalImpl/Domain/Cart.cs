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

        public Either<Cart, Error> Add_(Guid productId, int quantity, TemporaryReservation temporaryReservation)
        {
            // NB: for sure we could check that before, but in real, we should test current cart nb + quantity added here...
            if (quantity > 99)
            {
                return Either<Cart, Error>.Right(new Error("Too many of the same product in cart"));
            }
            // modify this
            // OR probably a new cart instance instead of modifying this...
            return Either<Cart, Error>.Left(this);
        }
    }
}