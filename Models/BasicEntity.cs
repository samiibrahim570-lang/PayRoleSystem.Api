using System.ComponentModel.DataAnnotations;
using PayRoleSystem.Models.Interfaces;

namespace PayRoleSystem.Models
{
    public abstract class BasicEntity : Entity<int>, IBasicEntity
    {
        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public bool IsDeleted { get; set; } = false;

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Required]
        public long CreatedBy { get; set; } 

        public DateTime? ModifiedOn { get; set; }

        public long? ModifiedBy { get; set; }
    }
}
