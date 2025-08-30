namespace EasyZap.Models
{
    public class WorkDay
    {
        public int Id { get; set; }
        public int MasterId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string? Notes { get; set; }
        public ApplicationUser? Master { get; set; }
    }
}
