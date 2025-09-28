
namespace parla_metro_tickets_api.src.Helper
{
    public class QueryObject
    {
        public string? textFilter { get; set; } = string.Empty;
        public string? type { get; set; } = string.Empty;
        public string? status { get; set; } = string.Empty;
        public string? sortByAmountPaid { get; set; } = string.Empty;
        public string? sortByDate { get; set; } = string.Empty;
        public bool isDescendingAmountPaid { get; set; } = false;
        public bool isDescendingDate { get; set; } = false;
    }
}