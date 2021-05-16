using RepairShopBusinessLogic.BindingModels;
using RepairShopBusinessLogic.Interfaces;
using RepairShopBusinessLogic.ViewModels;
using RepairShopListImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepairShopListImplement.Implements
{
    public class RepairStorage : IRepairStorage
    {
        private readonly DataListSingleton source;
        public RepairStorage()
        {
            source = DataListSingleton.GetInstance();
        }
        public List<RepairViewModel> GetFullList()
        {
            List<RepairViewModel> result = new List<RepairViewModel>();
            foreach (var material in source.Repairs)
            {
                result.Add(CreateModel(material));
            }
            return result;
        }
        public List<RepairViewModel> GetFilteredList(RepairBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            List<RepairViewModel> result = new List<RepairViewModel>();
            foreach (var repair in source.Repairs)
            {
                if (repair.RepairName.Contains(model.RepairName))
                {
                    result.Add(CreateModel(repair));
                }
            }
            return result;
        }
        public RepairViewModel GetElement(RepairBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            foreach (var repair in source.Repairs)
            {
                if (repair.Id == model.Id || repair.RepairName ==
                model.RepairName)
                {
                    return CreateModel(repair);
                }
            }
            return null;
        }
        public void Insert(RepairBindingModel model)
        {
            Repair tempRepair = new Repair
            {
                Id = 1,
                RepairMaterials = new
            Dictionary<int, int>()
            };
            foreach (var repair in source.Repairs)
            {
                if (repair.Id >= tempRepair.Id)
                {
                    tempRepair.Id = repair.Id + 1;
                }
            }
            source.Repairs.Add(CreateModel(model, tempRepair));
        }
        public void Update(RepairBindingModel model)
        {
            Repair tempRepair = null;
            foreach (var repair in source.Repairs)
            {
                if (repair.Id == model.Id)
                {
                    tempRepair = repair;
                }
            }
            if (tempRepair == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, tempRepair);
        }
        public void Delete(RepairBindingModel model)
        {
            for (int i = 0; i < source.Repairs.Count; ++i)
            {
                if (source.Repairs[i].Id == model.Id)
                {
                    source.Repairs.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
        private Repair CreateModel(RepairBindingModel model, Repair repair)
        {
            repair.RepairName = model.RepairName;
            repair.Price = model.Price;
            // удаляем убранные
            foreach (var key in repair.RepairMaterials.Keys.ToList())
            {
                if (!model.RepairMaterials.ContainsKey(key))
                {
                    repair.RepairMaterials.Remove(key);
                }
            }
            // обновляем существуюущие и добавляем новые
            foreach (var material in model.RepairMaterials)
            {
                if (repair.RepairMaterials.ContainsKey(material.Key))
                {
                    repair.RepairMaterials[material.Key] =
                    model.RepairMaterials[material.Key].Item2;

                }
                else
                {
                    repair.RepairMaterials.Add(material.Key,
                    model.RepairMaterials[material.Key].Item2);
                }
            }
            return repair;
        }
        private RepairViewModel CreateModel(Repair repair)
        {
            Dictionary<int, (string, int)> repairCMaterials = new
            Dictionary<int, (string, int)>();
            foreach (var pc in repair.RepairMaterials)
            {
                string materialName = string.Empty;
                foreach (var material in source.Materials)
                {
                    if (pc.Key == material.Id)
                    {
                        materialName = material.MaterialName;
                        break;
                    }
                }
                repairCMaterials.Add(pc.Key, (materialName, pc.Value));
            }
            return new RepairViewModel
            {
                Id = repair.Id,
                RepairName = repair.RepairName,
                Price = repair.Price,
                RepairMaterials = repairCMaterials
            };
        }
    }
}