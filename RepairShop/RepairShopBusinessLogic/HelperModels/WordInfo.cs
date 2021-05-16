using RepairShopBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace RepairShopBusinessLogic.HelperModels
{
    class WordInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<RepairViewModel> Repairs { get; set; }
    }
}
