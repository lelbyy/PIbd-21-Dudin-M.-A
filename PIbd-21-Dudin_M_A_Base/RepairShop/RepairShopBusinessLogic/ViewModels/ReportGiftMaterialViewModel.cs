using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace RepairShopBusinessLogic.ViewModels
{
    public class ReportRepairMaterialViewModel
    {
	[DataMember]
        [DisplayName("Имя изделия")]
        public string RepairName { get; set; }
	[DataMember]
        [DisplayName("Итоговое количество")]
        public int TotalCount { get; set; }
	[DataMember]
        public List<Tuple<string, int>> Materials { get; set; }
    }
}

