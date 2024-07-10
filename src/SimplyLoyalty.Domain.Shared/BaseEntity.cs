namespace SimplyLoyalty.Domain.Shared
{
    public class BaseEntity<TKey>
    {
        public BaseEntity()
        {
        }

        public BaseEntity(TKey id)
        {
            Id = id;
        }

        public virtual TKey Id { get; protected set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
    }

    public class BaseEntity : BaseEntity<Guid>
    {
        public BaseEntity()
        {
        }

        public BaseEntity(Guid id) : base(id)
        {
        }
    }
}
