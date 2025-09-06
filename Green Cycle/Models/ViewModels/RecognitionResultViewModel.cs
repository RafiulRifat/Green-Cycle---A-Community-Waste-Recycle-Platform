using System;

namespace GreenCycle.Models.ViewModels
{
    public class RecognitionResultViewModel
    {
        public string Label { get; set; }
        public double Confidence { get; set; }      // 0.92 => 92%
        public string Material { get; set; }
        public double SavedCO2 { get; set; }        // kilograms
        public string[] Instructions { get; set; }
        public DateTime ScannedAt { get; set; }
        public string PreviewImagePath { get; set; } // ~/Content/uploads/...
    }
}
