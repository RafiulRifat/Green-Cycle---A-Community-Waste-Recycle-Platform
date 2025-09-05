using System.Collections.Generic;
using Green_Cycle.Models.Entities;

namespace Green_Cycle.Models.ViewModels.DropOffPoints
{
    public class DropOffPointsIndexViewModel
    {
        public IEnumerable<DropOffPoint> Items { get; set; }

        // filters
        public string Search { get; set; }
        public int? Distance { get; set; }     // km

        // paging
        public int Page { get; set; }          // 1-based
        public int PageSize { get; set; }      // 10/25/50
        public int TotalCount { get; set; }
        public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
    }
}
