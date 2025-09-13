namespace EasyZap.Models
{
    public class MasterLink
    {
        public int Id { get; set; }
        public string Token { get; set; } = null;
        public int MasterId { get; set; }
        public ApplicationUser Master { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiresAt { get; set; }
        public bool IsActive { get; set; } = true;
        public int Clicks { get; set; } = 0;
    }
}
