namespace PaladinsApiPCL.Core.Domain.Models
{
    public class Item : PaladinsResponse
    {
        public string Description { get; set; }
        public string DeviceName { get; set; }
        public int IconId { get; set; }
        public int ItemId { get; set; }
        public int Price { get; set; }
        public string ShortDesc { get; set; }
    }
}
