﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandMaster_Models
{
    public class Product
    {
        public Product()
        {
            TempSqFt = 1;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(1,int.MaxValue, ErrorMessage = "Некорректная цена")]
        public double Price { get; set; }
        public string Image { get; set; }
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [Display(Name = "Апптайп")]
        public int ApplicationTypeId { get; set; }
        [ForeignKey("ApplicationTypeId")]
        public virtual ApplicationType ApplicationType { get; set; }

        [NotMapped]
        [Range(1, 10000, ErrorMessage = "Кол-во товара должно быть больше нуля")]
        public int TempSqFt { get; set; }
    }
}
