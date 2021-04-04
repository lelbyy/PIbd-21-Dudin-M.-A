using System.ComponentModel;

namespace RepairShopBusinessLogic.ViewModels
{
    public class MaterialViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название материала")]
        public string MaterialName { get; set; }
    }
}
