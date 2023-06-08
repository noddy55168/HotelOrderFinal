namespace HotelOrderFinal.Models
{
    public class CDiscountWrap
    {
        private Discount _discount;

        public Discount discount
        {
            get { return _discount; }
            set { _discount = value; }
        }

        public CDiscountWrap()
        {
            discount = new Discount();
        }

        public int DiscountId
        {
            get { return _discount.DiscountId; }
            set { _discount.DiscountId = value; }
        }
        public string DiscountName
        {
            get { return _discount.DiscountName; }
            set { _discount.DiscountName = value; }
        }
        public string? DiscountImage
        {
            get { return _discount.DiscountImage; }
            set { _discount.DiscountImage = value; }
        }
        public string? DiscountDirections
        {
            get { return _discount.DiscountDirections; }
            set { _discount.DiscountDirections = value; }
        }
        public decimal? DiscountDiscount
        {
            get { return _discount.DiscountDiscount; }
            set { _discount.DiscountDiscount = value; }
        }
        public bool? DiscountExist
        {
            get { return _discount.DiscountExist; }
            set { _discount.DiscountExist = value; }
        }       
        public IFormFile photo { get; set; }

    }
}

