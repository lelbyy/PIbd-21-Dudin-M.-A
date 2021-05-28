using RepairShopBusinessLogic.Attributes;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RepairShopBusinessLogic.ViewModels
{
    public class RepairViewModel
    {
	[Column(title: "Номер", width: 50)]
	[DataMember]
        public int Id { get; set; }
	[Column(title: "Название", gridViewAutoSize: GridViewAutoSize.Fill)]
	[DataMember]
        public string RepairName { get; set; }
	[Column(title: "Цена", width: 100)]
	[DataMember]
        public decimal Price { get; set; }
	[DataMember]
        public Dictionary<int, (string, int)> RepairMaterials { get; set; }
    }
}
