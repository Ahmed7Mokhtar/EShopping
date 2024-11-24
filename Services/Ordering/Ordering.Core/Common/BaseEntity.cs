namespace Ordering.Core.Common
{
    public abstract class BaseEntity
    {
        public string Id { get; protected set; } = null!;
        public string? CreatedBy { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTimeOffset? LastModifiedAt { get; set; }
    }
}
