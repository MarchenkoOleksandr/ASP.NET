namespace ServiceStation.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        
        public int ScheduleId { get; set; }

        public int ServiceId { get; set; }

        [Display(Name = "Цена")]
        [Range(0, 99999999, ErrorMessage = "Цена не может быть меньше нуля")]
        public decimal Price { get; set; }

        public virtual Schedule Schedule { get; set; }

        public virtual Service Service { get; set; }
    }
}
