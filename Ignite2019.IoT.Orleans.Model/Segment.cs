namespace Ignite2019.IoT.Orleans.Model
{
    public class Segment
    {
        public ulong LatestNum { get; set; }

        
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}