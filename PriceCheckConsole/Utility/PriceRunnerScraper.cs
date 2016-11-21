using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PriceCheckConsole.Utility
{
    public class PriceRunnerScraper
    {
        public string ProductName(string rawHtml)
        {
            var priceRunnerProductName = new Regex(@"itemprop=""name"".+?>(?<name>.+?)<");
            return priceRunnerProductName.Match(rawHtml).Groups["name"].ToString().TrimEnd().Replace("sammenlign priser", ""); 
        }

        //TODO: Add regex method to get product category

        public List<string> PricesWithShipping(string rawHtml)
        {
            var priceRunnerPrices = new Regex(@"(<div class=""price-inc-ship (firstprice)?"">(\s+?)(<p>)?(\s+?)?<strong class=""validated-shipping"">(?<prices>.+?)</strong>)|<strong class=""unvalidated-shipping"">(?<prices>.+?)</strong>", RegexOptions.Singleline);
            return priceRunnerPrices.Matches(rawHtml)
                .OfType<Match>()
                .Select(m => m.Groups["prices"].Value)
                .ToList();
        }
        
        public List<string> PricesWithoutShipping(string rawHtml)
        {
            //First part before "|" gets the prices without shipping for PriceRunner customers, and non-customers, after | gets a weird edge case
            var priceRunnerPrices = new Regex(@"(<div(\s+?)? class=""price-no-ship(\s*?firstprice)?"">(\s+?)<strong(\s*?class=""validated-shipping"")?>(?<prices>.+?)</strong>)|(<div(\s+?)class=""price-no-ship firstprice"">(\s*?)<a.*?"">(\s*?)<strong>(?<prices>.+?)</strong>)", RegexOptions.Singleline);
            return priceRunnerPrices.Matches(rawHtml)
                .OfType<Match>()
                .Select(m => m.Groups["prices"].Value)
                .ToList();
        }
        
        public List<string> Vendors(string rawHtml)
        {
            var priceRunnerVendors = new Regex(@"(<a rel=""nofollow"" target=""_blank"" class=""google-analytic-retailer-data"" retailer-data=""(?<vendor>.+?)\(\d|<p class=""non-partner"">(?<vendor>.+?)<)");
            return priceRunnerVendors.Matches(rawHtml)
                .OfType<Match>()
                .Select(m => m.Groups["vendor"].Value)
                .ToList();
        }

        public List<string> ShippingTime(string rawHtml)
        {
            var priceRunnerShippingTime = new Regex(@"<div class=""show-message-delivery"">(?<shippingTime>(.*?))<");//Ændre til * i stedet for +
            return priceRunnerShippingTime.Matches(rawHtml)
                .OfType<Match>()
                .Select(m => m.Groups["shippingTime"].Value)
                .ToList();
        }

        public List<string> StockStatus(string rawHtml)
        {
            var priceRunnerStockStatus = new Regex(@"(<p class=""stock-info in-stock"">(?<stockStatus>.+?)<)|(<p class=""stock-info out-of-stock"">(?<stockStatus>.+?)<)|(<p class=""stock-info unknown-stock"">(?<stockStatus>.+?)<)");
            return priceRunnerStockStatus.Matches(rawHtml)
                .OfType<Match>()
                .Select(m => m.Groups["stockStatus"].Value)
                .ToList();
        }
    }
}
