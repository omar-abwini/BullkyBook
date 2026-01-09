using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "this field can't be empty")]
        public string Name { get; set; }

        [Required(ErrorMessage = "this field can't be empty")]
        [DisplayName("Display Order")]
        [Range(1,100,ErrorMessage ="invalid number")]
        
        public int DisplayOrder { get; set; }
    }
}
