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

        public List<string> PricesWithShipping(string rawHtml)
        {
            var priceRunnerPrices = new Regex(@"(<div class=""price-inc-ship (firstprice)?"">(\s+?)(<p>)?(\s+?)?<strong class=""validated-shipping"">(?<prices>.+?)</strong>)|<strong class=""unvalidated-shipping"">(?<prices>.+?)</strong>", RegexOptions.Singleline);
            return priceRunnerPrices.Matches(rawHtml)
                .OfType<Match>()
                .Select(m => m.Groups["prices"].Value)
                .ToList();
        }
        /*
        public List<string> PricesWithoutShipping(string rawHtml)
        {
            var priceRunnerPrices = new Regex(@"((<strong class=""validated-shipping"">(?<prices>.+?)<)|(<strong class=""unvalidated-shipping"">(?<prices>.+?)<))");
            return priceRunnerPrices.Matches(rawHtml)
                .OfType<Match>()
                .Select(m => m.Groups["prices"].Value)
                .ToList();
        }
        */
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


//http://www.pricerunner.dk/pl/37-3254116/Grafikkort/MSI-GeForce-GTX-1060-GAMING-X-6G-Sammenlign-Priser
//Med fragt:                <strong class="validated-shipping">kr 2.480</strong>
//Fragt ukendt kunde:       <strong class="unvalidated-shipping">Fragt ukendt</strong></a>
//Fragt ukendt ikke kunde:  <p><strong class="unvalidated-shipping">Fragt ukendt</strong>
//Uden fragt:               <div class="price-no-ship">
//<strong>kr 2.645</strong>

//http://www.pricerunner.dk/pl/37-3254116/Grafikkort/MSI-GeForce-GTX-1060-GAMING-X-6G-Sammenlign-Priser?offer_sort=name
//Med fragt:                <strong class="validated-shipping">kr 3.084</strong>
//Fragt ukendt kunde:       <strong class="unvalidated-shipping">Fragt ukendt</strong>
//Fragt ukendt ikke kunde:  <strong class="unvalidated-shipping">Fragt ukendt</strong>
//Uden fragt:               <div  class="price-no-ship firstprice">
//<a rel = "nofollow" title="" target="_blank" class="google-analytic-retailer-data" retailer-data="Jumbo Computer(34682)" href="/track/scripts/redir.php?bt=b2ZmZXI%3D&amp;ch=9&amp;oi=3254116034682001&amp;mc=26&amp;dp=3&amp;sb=name&amp;cy=DNK&amp;ca=37&amp;cn=Grafikkort&amp;pi=3254116&amp;mi=34682&amp;su=ODkwMDQ0MV8zNjEzMg%3D%3D&amp;dg=cHJvZHVjdF9wcmljZQ%3D%3D&amp;cl=price&amp;du=aHR0cDovL3d3dy5qdW1ib3Nob3AuZGsvcGFydGRldGFpbC5hc3B4P3E9cDo4OTAwNDQxO2M6MzYxMzI%3D">
//<strong>kr 2.717</strong></a>
//Uden fragt ikke kunde:    <div  class="price-no-ship firstprice">
							//<strong class="validated-shipping">kr 2.738</strong>