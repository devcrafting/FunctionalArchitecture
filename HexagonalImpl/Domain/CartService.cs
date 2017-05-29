namespace HexagonalImpl.Domain
{
    public class CartService
    {
        private readonly ICarts _carts;

        public CartService(ICarts carts)
        {
            _carts = carts;
        }

        public bool Add(AddProduct addProduct, TemporaryReservation temporaryReservation)
        {
            var cart = _carts.Get(addProduct.CartId);
            if (cart.Add(addProduct.ProductId, addProduct.Quantity, temporaryReservation))
            {
                _carts.Save(cart);
                return true;
            }
            return false;
        }
    }
}