using GreenCycle.Models.Entities;

namespace GreenCycle.Models.ViewModels
{
    public class RecognitionResultViewModel
    {
        public MaterialType Material { get; set; }
        public decimal WeightKg { get; set; }

        public decimal FactorMinKgPerKg { get; set; }
        public decimal FactorMaxKgPerKg { get; set; }

        public decimal EmissionsMinKg { get; set; }
        public decimal EmissionsMaxKg { get; set; }
        public decimal EmissionsAvgKg { get; set; }

        public string Notes { get; set; }

        // For previewing the uploaded photo on the result page (data URL)
        public string PhotoDataUrl { get; set; }
    }
}
