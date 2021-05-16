using System.Collections.Generic;
using System.ComponentModel;

namespace RepairShopBusinessLogic.ViewModels
{
    public class RepairViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название изделия")]
        public string RepairName { get; set; }
        [DisplayName("Цена")]
        public decimal Price { get; set; }
        public Dictionary<int, (string, int)> RepairMaterials { get; set; }
    }
}
