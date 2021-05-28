using RepairShopBusinessLogic.BindingModels;
using RepairShopBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace RepairShopBusinessLogic.Interfaces
{
    public interface IMessageInfoStorage
    {
        List<MessageInfoViewModel> GetFullList();
        List<MessageInfoViewModel> GetFilteredList(MessageInfoBindingModel model);
        void Insert(MessageInfoBindingModel model);
    }
}
