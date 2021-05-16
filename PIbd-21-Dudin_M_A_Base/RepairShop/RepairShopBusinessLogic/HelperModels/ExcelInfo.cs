using RepairShopBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace RepairShopBusinessLogic.HelperModels
{
    class ExcelInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<ReportRepairMaterialViewModel> RepairMaterials { get; set; }
    }
}