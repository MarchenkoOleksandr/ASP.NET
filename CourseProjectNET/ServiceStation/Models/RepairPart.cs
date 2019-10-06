namespace ServiceStation.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class RepairPart
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "��������")]
        public string PartName { get; set; }

        [Display(Name = "����������")]
        [Range(1, 1000, ErrorMessage = "���������� ����� ���� �� 1 �� 1000 ����")]
        public int Amount { get; set; }

        [Display(Name = "����� ���������")]
        [Range(0, 99999999, ErrorMessage = "���� �� ����� ���� ������ ����")]
        public decimal Price { get; set; }

        public int ScheduleId { get; set; }

        public virtual Schedule Schedule { get; set; }
    }
}
