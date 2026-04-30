using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PayRoleSystem.Models.Interfaces;

namespace PayRoleSystem.Models
{
    public abstract class Entity<TKey> : IBase<TKey>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKey Id { get; set; }

        [Required]
        public Guid GlobalId { get; set; } 
    }
}
