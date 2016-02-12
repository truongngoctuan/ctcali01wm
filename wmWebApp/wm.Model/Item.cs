using System.ComponentModel.DataAnnotations;

namespace wm.Model
{
    public class Item : AuditableEntity<int>
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
