using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HandMaster_Models
{
    public class ApplicationType
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Название апп тайп")]
        [Required]
        public string Name { get; set; }
    }
}
