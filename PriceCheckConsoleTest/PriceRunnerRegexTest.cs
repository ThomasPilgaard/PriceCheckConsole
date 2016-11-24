using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PriceCheckConsole.Utility;

namespace PriceCheckConsoleTest
{
    [TestClass]
    public class PriceRunnerRegexTest
    {
        static readonly List<string> LinkList = new List<string>();
        static readonly List<string> LinkListWithSubProd = new List<string>();
        static readonly PriceRunnerScraper PriceRunnerDebug = new PriceRunnerScraper();
        private static readonly WebClient W = new WebClient { Encoding = Encoding.UTF8 };

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            string html = W.DownloadString("http://www.pricerunner.dk/pl/37-3254116/Grafikkort/MSI-GeForce-GTX-1060-GAMING-X-6G-Sammenlign-Priser");
            string html2 = W.DownloadString("http://www.pricerunner.dk/pl/40-3098022/CPU/Intel-Core-i7-6700K-4GHz-Box-Sammenlign-Priser");
            string html3 = W.DownloadString("http://www.pricerunner.dk/pl/184-3508132/Computer-koeling/Fractal-Design-Kelvin-S36-Sammenlign-Priser");
            string html4 = W.DownloadString("http://www.pricerunner.dk/pl/186-3064010/Kabinetter/Fractal-Design-Define-S-Sammenlign-Priser");
            string html5 = W.DownloadString("http://www.pricerunner.dk/pl/37-3254116/Grafikkort/MSI-GeForce-GTX-1060-GAMING-X-6G-Sammenlign-Priser?offer_sort=strategicallyinstock");
            string html6 = W.DownloadString("http://www.pricerunner.dk/pl/37-3254116/Grafikkort/MSI-GeForce-GTX-1060-GAMING-X-6G-Sammenlign-Priser?offer_sort=pricewithship");
            string html7 = W.DownloadString("http://www.pricerunner.dk/pl/37-3254116/Grafikkort/MSI-GeForce-GTX-1060-GAMING-X-6G-Sammenlign-Priser?offer_sort=price");
            string html8 = W.DownloadString("http://www.pricerunner.dk/pl/37-3254116/Grafikkort/MSI-GeForce-GTX-1060-GAMING-X-6G-Sammenlign-Priser?offer_sort=name");
            string html9 = W.DownloadString("http://www.pricerunner.dk/pl/37-3254116/Grafikkort/MSI-GeForce-GTX-1060-GAMING-X-6G-Sammenlign-Priser?offer_sort=rate");
            string html10 = W.DownloadString("http://www.pricerunner.dk/pl/37-3254116/Grafikkort/MSI-GeForce-GTX-1060-GAMING-X-6G-Sammenlign-Priser?offer_sort=instock");
            string html11 = W.DownloadString("http://www.pricerunner.dk/pl/37-3254116/Grafikkort/MSI-GeForce-GTX-1060-GAMING-X-6G-Sammenlign-Priser?offer_sort=deliverytime");
            string html12 = W.DownloadString("http://www.pricerunner.dk/pl/11-3081951/MP3-afspillere/Apple-iPod-Nano-16GB-%288th-Generation%29-Sammenlign-Priser");
            string html13 = W.DownloadString("http://www.pricerunner.dk/pl/1-3310055/Mobiltelefoner/Apple-iPhone-7-32GB-Sammenlign-Priser");

            LinkList.Add(PriceRunnerDebug.RemoveSubProducts(html));
            LinkList.Add(PriceRunnerDebug.RemoveSubProducts(html2));
            LinkList.Add(PriceRunnerDebug.RemoveSubProducts(html3));
            LinkList.Add(PriceRunnerDebug.RemoveSubProducts(html4));
            LinkList.Add(PriceRunnerDebug.RemoveSubProducts(html5));
            LinkList.Add(PriceRunnerDebug.RemoveSubProducts(html6));
            LinkList.Add(PriceRunnerDebug.RemoveSubProducts(html7));
            LinkList.Add(PriceRunnerDebug.RemoveSubProducts(html8));
            LinkList.Add(PriceRunnerDebug.RemoveSubProducts(html9));
            LinkList.Add(PriceRunnerDebug.RemoveSubProducts(html10));
            LinkList.Add(PriceRunnerDebug.RemoveSubProducts(html11));
            LinkList.Add(PriceRunnerDebug.RemoveSubProducts(html12));
            LinkList.Add(PriceRunnerDebug.RemoveSubProducts(html13));
            LinkListWithSubProd.Add(html);
            LinkListWithSubProd.Add(html4);
            LinkListWithSubProd.Add(html8);
            LinkListWithSubProd.Add(html13);
        }

        [TestMethod]
        public void TestNumbOfVendors()
        {
            foreach (var e in LinkList)
            {
                var numbOfVendors = PriceRunnerDebug.NumberOfVendors(e);
                Assert.IsNotNull(numbOfVendors);
                Assert.IsTrue(numbOfVendors > 0, "numbOfVendors: " + numbOfVendors);
            }
        }

        [TestMethod]
        public void TestProductNameFound()
        {
            foreach (var e in LinkList)
            {
                var productName = PriceRunnerDebug.ProductName(e);
                Assert.IsNotNull(productName);
                Assert.IsTrue(productName.Length > 0);
            }
        }
        
        [TestMethod]
        public void TestCorrectNumbOfVendors()
        {
            foreach (var e in LinkList)
            {
                var numbOfVendors = PriceRunnerDebug.NumberOfVendors(e);
                var vendors = PriceRunnerDebug.Vendors(e);
                Assert.AreEqual(numbOfVendors, vendors.Count);
            }
        }
        
        [TestMethod]
        public void TestCorrectNumbOfPricesWs()
        {
            foreach (var e in LinkList)
            {
                var numbOfVendors = PriceRunnerDebug.NumberOfVendors(e);
                var pricesWithShipping = PriceRunnerDebug.PricesWithShipping(e);
                Assert.AreEqual(numbOfVendors, pricesWithShipping.Count);
            }
        }
        
        [TestMethod]
        public void TestCorrectNumbOfPricesNoS()
        {
            foreach (var e in LinkList)
            {
                var numbOfVendors = PriceRunnerDebug.NumberOfVendors(e);
                var pricesWithoutShipping = PriceRunnerDebug.PricesWithoutShipping(e);
                Assert.AreEqual(numbOfVendors, pricesWithoutShipping.Count);
            }
        }

        [TestMethod]
        public void TestCorrectShippingTimes()
        {
            foreach (var e in LinkList)
            {
                var numbOfVendors = PriceRunnerDebug.NumberOfVendors(e);
                var shippingTimes = PriceRunnerDebug.ShippingTime(e);
                Assert.AreEqual(numbOfVendors, shippingTimes.Count);

            }
        }

        [TestMethod]
        public void TestCorrectStockStatus()
        {
            foreach (var e in LinkList)
            {
                var numbOfVendors = PriceRunnerDebug.NumberOfVendors(e);
                var stockStatus = PriceRunnerDebug.StockStatus(e);
                Assert.AreEqual(numbOfVendors, stockStatus.Count);
            }
        }

        [TestMethod]
        public void TestProductCategory()
        {
            foreach (var e in LinkList)
            {
                var category = PriceRunnerDebug.ProductCategory(e);
                Assert.IsTrue(category.Count > 0);
            }
        }

        [TestMethod]
        public void TestSubProductRemoval()
        {
            //Assert.IsTrue(e) can fail for a product, without vendors having sub products.
            var subProdTest = new Regex(@"more-retailer=""1""", RegexOptions.Compiled);
            foreach (var e in LinkListWithSubProd)
            {
                Assert.IsTrue(subProdTest.IsMatch(e));
                Assert.IsFalse(subProdTest.IsMatch(PriceRunnerDebug.RemoveSubProducts(e)));
            }
        }
    }
}
