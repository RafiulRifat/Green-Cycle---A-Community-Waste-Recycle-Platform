using GreenCycle.Models.Entities;
using GreenCycle.Services.Interfaces;

namespace GreenCycle.Services.Implementations
{
    public class RecognitionService : IRecognitionService
    {
        private const decimal PlasticMin = 1.7m;
        private const decimal PlasticMax = 3.5m;

        private const decimal GlassMin = 0.5m;
        private const decimal GlassMax = 3.08m;

        private const decimal PaperMin = 0.9m;
        private const decimal PaperMax = 1.2m;

        public RecognitionResult CalculateEmissions(MaterialType material, decimal weightKg)
        {
            decimal minFactor;
            decimal maxFactor;
            string notes;

            switch (material)
            {
                case MaterialType.Plastic:
                    minFactor = PlasticMin;
                    maxFactor = PlasticMax;
                    notes = "Fossil-based plastic. Range depends on polymer and process energy.";
                    break;
                case MaterialType.Glass:
                    minFactor = GlassMin;
                    maxFactor = GlassMax;
                    notes = "Glass emissions vary with cullet %, furnace efficiency, and fuel mix.";
                    break;
                case MaterialType.Paper:
                default:
                    minFactor = PaperMin;
                    maxFactor = PaperMax;
                    notes = "Virgin pulp paper; pulping energy dominates emissions.";
                    break;
            }

            var min = weightKg * minFactor;
            var max = weightKg * maxFactor;
            var avg = (min + max) / 2m;

            return new RecognitionResult
            {
                Material = material,
                WeightKg = weightKg,
                FactorMinKgPerKg = minFactor,
                FactorMaxKgPerKg = maxFactor,
                EmissionsMinKg = Round3(min),
                EmissionsMaxKg = Round3(max),
                EmissionsAvgKg = Round3(avg),
                Notes = notes
            };
        }

        private static decimal Round3(decimal v) =>
            decimal.Round(v, 3, System.MidpointRounding.AwayFromZero);
    }
}
