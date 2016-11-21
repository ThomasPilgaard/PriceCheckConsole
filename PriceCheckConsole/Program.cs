using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PriceCheckConsole.Model;
using PriceCheckConsole.Utility;

namespace PriceCheckConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new PcHwProduct("asdf");
            test.ProductName = "test";
            test.PType = PcHwProduct.ProductType.Cooling;
            var w = new WebClient { Encoding = Encoding.UTF8 };
            string html = w.DownloadString("http://www.pricerunner.dk/pl/37-3254116/Grafikkort/MSI-GeForce-GTX-1060-GAMING-X-6G-Sammenlign-Priser");
            string html2 = w.DownloadString("http://www.pricerunner.dk/pl/40-3098022/CPU/Intel-Core-i7-6700K-4GHz-Box-Sammenlign-Priser");
            string html3 = w.DownloadString("http://www.pricerunner.dk/pl/184-3508132/Computer-koeling/Fractal-Design-Kelvin-S36-Sammenlign-Priser");
            string html4 = w.DownloadString("http://www.pricerunner.dk/pl/186-3064010/Kabinetter/Fractal-Design-Define-S-Sammenlign-Priser");
            
            string html5 = w.DownloadString("http://www.pricerunner.dk/pl/37-3254116/Grafikkort/MSI-GeForce-GTX-1060-GAMING-X-6G-Sammenlign-Priser?offer_sort=strategicallyinstock");
            string html6 = w.DownloadString("http://www.pricerunner.dk/pl/37-3254116/Grafikkort/MSI-GeForce-GTX-1060-GAMING-X-6G-Sammenlign-Priser?offer_sort=pricewithship");
            string html7 = w.DownloadString("http://www.pricerunner.dk/pl/37-3254116/Grafikkort/MSI-GeForce-GTX-1060-GAMING-X-6G-Sammenlign-Priser?offer_sort=price");
            string html8 = w.DownloadString("http://www.pricerunner.dk/pl/37-3254116/Grafikkort/MSI-GeForce-GTX-1060-GAMING-X-6G-Sammenlign-Priser?offer_sort=name");
            string html9 = w.DownloadString("http://www.pricerunner.dk/pl/37-3254116/Grafikkort/MSI-GeForce-GTX-1060-GAMING-X-6G-Sammenlign-Priser?offer_sort=rate");
            string html10 = w.DownloadString("http://www.pricerunner.dk/pl/37-3254116/Grafikkort/MSI-GeForce-GTX-1060-GAMING-X-6G-Sammenlign-Priser?offer_sort=instock");
            string html11 = w.DownloadString("http://www.pricerunner.dk/pl/37-3254116/Grafikkort/MSI-GeForce-GTX-1060-GAMING-X-6G-Sammenlign-Priser?offer_sort=deliverytime");
            

            //Debugging
            
            var linkList = new List<string>();
            linkList.Add(html);
            linkList.Add(html2);
            linkList.Add(html3);
            linkList.Add(html4);
            linkList.Add(html5);
            linkList.Add(html6);
            linkList.Add(html7);
            linkList.Add(html8);
            linkList.Add(html9);
            linkList.Add(html10);
            linkList.Add(html11);

            int i = 1;
            foreach (var e in linkList)
            {
                var priceRunner = new PriceRunnerScraper();

                string newHtml = RemoveSubProducts(e);

                //var productName = priceRunner.ProductName(newHtml);
                var vendors = priceRunner.Vendors(newHtml);
                var prices = priceRunner.PricesWithShipping(newHtml);
                var shippingTimes = priceRunner.ShippingTime(newHtml);
                var stockStatus = priceRunner.StockStatus(newHtml);

                Console.WriteLine($" html {i++}, Vendors: {vendors.Count}, Prices: {prices.Count}, ShippingTimes {shippingTimes.Count}, StockStatus {stockStatus.Count}");
            }
            

            /*
            var priceRunner = new PriceRunnerScraper();

            string newHtml = RemoveSubProducts(html2);

            var productName = priceRunner.ProductName(newHtml);
            var vendors = priceRunner.Vendors(newHtml);
            var prices = priceRunner.Prices(newHtml);
            var shippingTimes = priceRunner.ShippingTime(newHtml);
            var stockStatus = priceRunner.StockStatus(newHtml);

            Console.WriteLine($"Vendors: {vendors.Count}, Prices: {prices.Count}, ShippingTimes {shippingTimes.Count}, StockStatus {stockStatus.Count}");
            
            foreach (var m in prices)
            {
                Console.WriteLine(m);
            }
            
            foreach (var m in vendors)
            {
                Console.WriteLine(m);
            }

            foreach (var m in shippingTimes)
            {
                if (m == string.Empty)
                {
                    Console.WriteLine("N/A");
                }
                else Console.WriteLine(m);
            }

            foreach (var m in stockStatus)
            {
                Console.WriteLine(m);
            }
            */


            //Console.WriteLine($"{productName}  {vendors[0]}  {prices[0]}  {stockStatus[0]}  {shippingTimes[0]}");

            Console.ReadLine();
            
        }

        static string RemoveSubProducts(string rawHtml)
        {
            var removeProductVersions = new Regex(@"(?<asdf><ul class=""multiplelist"".+?more-retailer=""1"".+?</ul>)", RegexOptions.Singleline);
            return removeProductVersions.Replace(rawHtml, "");
            
        }
    }
}
