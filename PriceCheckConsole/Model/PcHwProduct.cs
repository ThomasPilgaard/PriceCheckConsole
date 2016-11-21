namespace PriceCheckConsole.Model
{
    public class PcHwProduct : Product
    {
        public PcHwProduct(string productUrl) : base(productUrl)
        {
        }
        public ProductType PType { get; set; }

        public enum ProductType
        {
            Case,
            Cpu,
            Mobo,
            Psu,
            Cooling,
            Gpu,
            Ram,
            Ssd,
            Hdd,
            Monitor,
            Misc
        }
    }
}