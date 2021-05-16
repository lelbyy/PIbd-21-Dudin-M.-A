using RepairShopBusinessLogic.BindingModels;
using RepairShopBusinessLogic.HelperModels;
using RepairShopBusinessLogic.Interfaces;
using RepairShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepairShopBusinessLogic.BusinessLogics
{
    public class ReportLogic
    {
        private readonly IMaterialStorage _materialStorage;
        private readonly IRepairStorage _repairStorage;
        private readonly IOrderStorage _orderStorage;
        public ReportLogic(IRepairStorage repairStorage, IMaterialStorage
      materialStorage, IOrderStorage orderStorage)
        {
            _repairStorage = repairStorage;
            _materialStorage = materialStorage;
            _orderStorage = orderStorage;
        }
        /// <summary>
        /// Получение списка компонент с указанием, в каких изделиях используются
        /// </summary>
        /// <returns></returns>104
        public List<ReportRepairMaterialViewModel> GetRepairMaterial()
        {
            var materials = _materialStorage.GetFullList();
            var repairs = _repairStorage.GetFullList();
            var list = new List<ReportRepairMaterialViewModel>();
            foreach (var repair in repairs)
            {
                var record = new ReportRepairMaterialViewModel
                {
                    RepairName = repair.RepairName,
                    Materials = new List<Tuple<string, int>>(),
                    TotalCount = 0
                };
                foreach (var material in materials)
                {
                    if (repair.RepairMaterials.ContainsKey(material.Id))
                    {
                        record.Materials.Add(new Tuple<string, int>(material.MaterialName, repair.RepairMaterials[material.Id].Item2));
                        record.TotalCount += repair.RepairMaterials[material.Id].Item2;
                    }
                }
                list.Add(record);
            }
            return list;
        }
        /// Получение списка заказов за определенный период
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ReportOrdersViewModel> GetOrders(ReportBindingModel model)
        {
            return _orderStorage.GetFilteredList(new OrderBindingModel
            {
                DateFrom =
           model.DateFrom,
                DateTo = model.DateTo
            })
            .Select(x => new ReportOrdersViewModel
            {
                DateCreate = x.DateCreate,
                RepairName = x.RepairName,
                Count = x.Count,
                Sum = x.Sum,
                Status = x.Status
            })
           .ToList();
        }
        /// <summary>
        /// Сохранение компонент в файл-Word
        /// </summary>
        /// <param name="model"></param>
        public void SaveMaterialsToWordFile(ReportBindingModel model)
        {
            SaveToWord.CreateDoc(new WordInfo
            {
                FileName = model.FileName,
                Title = "Список изделий",
                Repairs = _repairStorage.GetFullList()
            });
        }
        /// <summary>
        /// Сохранение компонент с указаеним продуктов в файл-Excel
        /// </summary>
        /// <param name="model"></param>
        public void SaveRepairsMaterialsToExcelFile(ReportBindingModel model)
        {
            SaveToExcel.CreateDoc(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Список материалов",
                RepairMaterials = GetRepairMaterial()
            });
        }
        /// <summary>
        /// Сохранение заказов в файл-Pdf
        /// </summary>
        /// <param name="model"></param>
        public void SaveOrdersToPdfFile(ReportBindingModel model)
        {
            SaveToPdf.CreateDoc(new PdfInfo
            {
                FileName = model.FileName,
                Title = "Список заказов",
                DateFrom = model.DateFrom.Value,
                DateTo = model.DateTo.Value,
                Orders = GetOrders(model)
            });
        }
    }
}