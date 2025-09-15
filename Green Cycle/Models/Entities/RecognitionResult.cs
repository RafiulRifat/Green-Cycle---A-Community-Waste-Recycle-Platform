namespace GreenCycle.Models.Entities
{
    public enum MaterialType
    {
        Plastic = 0,
        Glass = 1,
        Paper = 2
    }

    public class RecognitionResult
    {
        public MaterialType Material { get; set; }
        public decimal WeightKg { get; set; }
        public decimal FactorMinKgPerKg { get; set; }
        public decimal FactorMaxKgPerKg { get; set; }
        public decimal EmissionsMinKg { get; set; }
        public decimal EmissionsMaxKg { get; set; }
        public decimal EmissionsAvgKg { get; set; }
        public string Notes { get; set; }
    }
}
