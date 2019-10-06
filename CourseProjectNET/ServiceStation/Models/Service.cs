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
        [Display(Name = "������")]
        public string ServiceName { get; set; }

        [Display(Name = "����������� ����� (�����)")]
        [Range(0, 50000, ErrorMessage = "����� ����� ���� �� 0 �� 50000 �����")]
        public int NecessaryTime { get; set; }

        [Display(Name = "��������� ������")]
        [Range(0, 99999999, ErrorMessage = "���� �� ����� ���� ������ ����")]
        public decimal Price { get; set; }

        [Display(Name = "���������")]
        public int CategoryId { get; set; }

        [Display(Name = "���������")]
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
