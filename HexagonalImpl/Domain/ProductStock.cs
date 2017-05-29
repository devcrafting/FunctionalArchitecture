namespace HexagonalImpl.Domain
{
    public class ProductStock
    {
        private readonly int _currentStock;

        public ProductStock(int currentStock)
        {
            _currentStock = currentStock;
        }

        public TemporaryReservation MakeATemporaryReservation(int quantity)
        {
            throw new System.NotImplementedException();
        }

        public Either<TemporaryReservation, Error> MakeATemporaryReservation_(int quantity)
        {
            if (quantity > _currentStock)
            {
                return Either<TemporaryReservation, Error>.Right(new Error("Not enough stock, sorry"));
            }
            return Either<TemporaryReservation, Error>.Left(new TemporaryReservation());
        }
    }
}