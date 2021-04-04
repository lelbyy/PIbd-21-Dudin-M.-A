using System.Collections.Generic;

namespace RepairShopListImplement.Models
{
    public class Repair
    {
        public int Id { get; set; }
        public string RepairName { get; set; }
        public decimal Price { get; set; }
        public Dictionary<int, int> RepairMaterials { get; set; }
    }
}
