using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepairShopDatabaseImplement.Models
{
    public class Repair
    {
        public int Id { get; set; }
        [Required]
        public string RepairName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [ForeignKey("RepairId")]
        public virtual List<RepairMaterial> RepairMaterials { get; set; }
        [ForeignKey("RepairId")]
        public virtual List<Order> Orders { get; set; }
    }
}
