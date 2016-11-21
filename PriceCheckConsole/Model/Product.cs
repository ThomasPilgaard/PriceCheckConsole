using System;

namespace PriceCheckConsole.Model
{
    public abstract class Product
    {
        //TODO: Change to lists
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public string ProductUrl { get; set; }
        public string StockStatus { get; set; }
        public string ShippingTime { get; set; }
        protected Product(string productUrl)
        {
            ProductUrl = productUrl;
        }
    }
}