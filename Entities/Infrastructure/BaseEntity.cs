using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Entities.Users;

namespace Entities.Infrastructure
{
    public interface IEntity
    {
        bool IsActive { get; set; }
        DateTime? DeleteAt { get; set; }
    }

    public interface IJsonProperty<T>
        where T : class
    {
    }

    public abstract class BaseEntity<TKey> : IEntity
    {
     
        public virtual TKey Id { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime? DeleteAt { get; set; }

    }

    public abstract class BaseEntity : BaseEntity<long>
    {
    }

    public abstract class BaseOptionEntity : BaseEntity
    {
        /// <summary>
        /// نام فیلد مورد نظر در تنظیمات
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Key { get; set; }

        /// <summary>
        /// مقدار فیلد
        /// </summary>
        [Required]
        public string Value { get; set; }
    }


    public abstract class BaseAuditableEntity<TKey, TUser, TUserKey> : BaseEntity<TKey>
    {

        public virtual DateTime? CreatedDateTime { get; set; } 
        public virtual TUser CreatedUser { get; set; }
        public virtual TUserKey CreatedUserId { get; set; }
        public virtual DateTime? ModifiedDateTime { get; set; } 
        public virtual TUser ModifiedUser { get; set; }
        public virtual TUserKey ModifiedUserId { get; set; }

        [JsonIgnore]
        public virtual string Audit { get; set; }
    }

    public abstract class BaseAuditableEntity : BaseAuditableEntity<long, User, int>
    {

    }
    public abstract class BaseAuditableEntity<TKey> : BaseAuditableEntity<TKey, User, int>
    {

    }
}
