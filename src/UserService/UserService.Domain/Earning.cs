using SimplyLoyalty.Domain.Shared;

namespace UserService.Domain
{
    public class Earning : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        /// <summary>
        /// points earned for a specific engagement
        /// </summary>
        public int EarnedPoints { get; set; }
    }
}

