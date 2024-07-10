using SimplyLoyalty.Domain.Shared;

namespace UserService.Domain
{
    public class User : BaseEntity<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Earning> Earnings { get; set; }
    }
}
