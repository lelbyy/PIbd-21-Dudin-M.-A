using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RepairShopBusinessLogic.ViewModels
{
    public class ReportRepairMaterialViewModel
    {
        [DisplayName("Имя изделия")]
        public string RepairName { get; set; }
        [DisplayName("Итоговое количество")]
        public int TotalCount { get; set; }
        public List<Tuple<string, int>> Materials { get; set; }
    }
}

