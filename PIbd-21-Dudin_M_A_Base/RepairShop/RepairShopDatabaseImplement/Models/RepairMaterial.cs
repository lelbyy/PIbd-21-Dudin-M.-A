using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace RepairShopDatabaseImplement.Models
{
    public class RepairMaterial
    {
        public int Id { get; set; }
        public int RepairId { get; set; }
        public int MaterialId { get; set; }
        [Required]
        public int Count { get; set; }
        public virtual Material Material { get; set; }
        public virtual Repair Repair { get; set; }
    }
}
