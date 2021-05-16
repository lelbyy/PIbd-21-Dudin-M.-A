using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace RepairShopBusinessLogic.ViewModels
{
    public class RepairViewModel
    {
	[DataMember]
        public int Id { get; set; }
	[DataMember]
        [DisplayName("Название изделия")]
        public string RepairName { get; set; }
	[DataMember]
        [DisplayName("Цена")]
        public decimal Price { get; set; }
	[DataMember]
        public Dictionary<int, (string, int)> RepairMaterials { get; set; }
    }
}
