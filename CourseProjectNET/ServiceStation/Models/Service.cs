namespace ServiceStation.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class Service
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Услуга")]
        public string ServiceName { get; set; }

        [Display(Name = "Необходимое время (минут)")]
        [Range(0, 50000, ErrorMessage = "Время может быть от 0 до 50000 минут")]
        public int NecessaryTime { get; set; }

        [Display(Name = "Стоимость услуги")]
        [Range(0, 99999999, ErrorMessage = "Цена не может быть меньше нуля")]
        public decimal Price { get; set; }

        [Display(Name = "Категория")]
        public int CategoryId { get; set; }

        [Display(Name = "Категория")]
        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Service()
        {
            Orders = new HashSet<Order>();
        }
    }
}
