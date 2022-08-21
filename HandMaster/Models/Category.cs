using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HandMaster.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [DisplayName("Название категории")]
        [Required]
        public string Name { get; set; }     
        [DisplayName("Заказ")]
        [Required]
        [Range(1,int.MaxValue, ErrorMessage = "Недопустимое значение")]
        public int DisplayOrder { get; set; }
    }
}
