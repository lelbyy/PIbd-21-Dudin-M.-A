using RepairShopBusinessLogic.BindingModels;
using RepairShopBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace RepairShopBusinessLogic.Interfaces
{
    public interface IMaterialStorage
    {
        List<MaterialViewModel> GetFullList();
        List<MaterialViewModel> GetFilteredList(MaterialBindingModel model);
        MaterialViewModel GetElement(MaterialBindingModel model);
        void Insert(MaterialBindingModel model);
        void Update(MaterialBindingModel model);
        void Delete(MaterialBindingModel model);
    }
}