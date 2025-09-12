namespace Green_Cycle.Models.Entities.Hybrid
{
    public class Machine
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }

        public int ZoneId { get; set; }
        public virtual ServiceZone Zone { get; set; }

        public string Status { get; set; }     // Available | NearFull | Full | Cancelled
        public int FillPercent { get; set; }   // 0-100

        // NEW: map coordinates
        public double? Lat { get; set; }
        public double? Lng { get; set; }
    }
}
