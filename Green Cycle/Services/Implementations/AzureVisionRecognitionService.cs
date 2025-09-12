using Green_Cycle.Services.Interfaces;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Green_Cycle.Services.Implementations
{
    public class AzureVisionRecognitionService : IRecognitionService
    {
        private readonly ComputerVisionClient _client;

        public AzureVisionRecognitionService(string endpoint, string key)
        {
            _client = new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(key))
            {
                Endpoint = endpoint
            };
        }

        public async Task<RecognitionResultDto> AnalyzeAsync(Stream imageStream, string fileName, string contentType)
        {
            var features = new List<VisualFeatureTypes?>
            {
                VisualFeatureTypes.Tags,
                VisualFeatureTypes.Description
            };

            var result = await _client.AnalyzeImageInStreamAsync(imageStream, features);

            var topTag = result.Tags != null && result.Tags.Count > 0
                ? result.Tags[0]
                : null;

            return new RecognitionResultDto
            {
                Label = topTag?.Name ?? result.Description?.Captions?[0]?.Text ?? "Unknown",
                Confidence = topTag?.Confidence ?? result.Description?.Captions?[0]?.Confidence ?? 0,
                Material = MapMaterial(topTag?.Name)
            };
        }

        private static string MapMaterial(string label)
        {
            if (string.IsNullOrEmpty(label)) return "General";
            label = label.ToLower();
            if (label.Contains("plastic")) return "Plastic";
            if (label.Contains("glass")) return "Glass";
            if (label.Contains("can") || label.Contains("aluminum")) return "Metal";
            if (label.Contains("paper") || label.Contains("cardboard")) return "Paper";
            return "General";
        }
    }
}
