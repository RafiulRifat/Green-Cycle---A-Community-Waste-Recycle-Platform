using GreenCycle.Models.Entities;

namespace GreenCycle.Services.Interfaces
{
    public interface IRecognitionService
    {
        /// <summary>
        /// Calculates CO2e emissions for a given material and weight.
        /// </summary>
        RecognitionResult CalculateEmissions(MaterialType material, decimal weightKg);
    }
}
