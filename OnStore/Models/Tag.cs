using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnStore.Models
{
    [Table("Tag")]
    public class Tag
    {
        [Key]
        public int TagId { get; set; }

        [Required, StringLength(50)]
        public string TagName { get; set; }
        public virtual ICollection<ProductTag> ProductTags { get; set; }
    }
}