namespace IdentityUser.src.Domain.Common
{
    /// <summary>
    /// Represents the base entity with common properties for all entities.
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the entity was created.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// Gets or sets the date and time when the entity was last updated.
        /// </summary>
        public DateTimeOffset? UpdatedAt { get; set; } = null;

        /// <summary>
        /// Gets or sets the date and time when the entity was deleted.
        /// </summary>
        public DateTimeOffset? DeletedAt { get; set; } = null;

        /// <summary>
        /// Gets a value indicating whether the entity is deleted.
        /// </summary>
        public bool IsDeleted { get; internal set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntity"/> class.
        /// </summary>
        public BaseEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}
