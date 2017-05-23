namespace HexagonalImpl.Domain
{
    public class ProductStockService
    {
        private readonly IProductStocks _productStocks;

        public ProductStockService(IProductStocks productStocks)
        {
            _productStocks = productStocks;
        }

        public TemporaryReservation MakeATemporaryReservation(AddProduct addProduct)
        {
            var productStock = _productStocks.Get(addProduct.ProductId);
            var temporaryReservation = productStock.MakeATemporaryReservation(addProduct.Quantity);
            if (temporaryReservation != null)
            {
                _productStocks.Save(productStock);
            }
            return temporaryReservation;
        }
    }
}