using System;
using System.Collections.Generic;
using System.Text;
using RepairShopBusinessLogic.BindingModels;
using RepairShopBusinessLogic.Interfaces;
using RepairShopBusinessLogic.ViewModels;
using RepairShopDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RepairShopDatabaseImplement.Implements
{
    public class RepairStorage : IRepairStorage
    {
        public List<RepairViewModel> GetFullList()
        {
            using (var context = new RepairShopDatabase())
            {
                return context.Repairs
                .Include(rec => rec.RepairMaterials)
               .ThenInclude(rec => rec.Material)
               .ToList()
               .Select(rec => new RepairViewModel
               {
                   Id = rec.Id,
                   RepairName = rec.RepairName,
                   Price = rec.Price,
                   RepairMaterials = rec.RepairMaterials
                .ToDictionary(recPC => recPC.MaterialId, recPC =>
               (recPC.Material?.MaterialName, recPC.Count))
               })
               .ToList();
            }
        }
        public List<RepairViewModel> GetFilteredList(RepairBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new RepairShopDatabase())
            {
                return context.Repairs
                .Include(rec => rec.RepairMaterials)
               .ThenInclude(rec => rec.Material)
               .Where(rec => rec.RepairName.Contains(model.RepairName))
               .ToList()
               .Select(rec => new RepairViewModel
               {
                   Id = rec.Id,
                   RepairName = rec.RepairName,
                   Price = rec.Price,
                   RepairMaterials = rec.RepairMaterials
                .ToDictionary(recPC => recPC.MaterialId, recPC =>
               (recPC.Material?.MaterialName, recPC.Count))
               })
               .ToList();
            }
        }
        public RepairViewModel GetElement(RepairBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new RepairShopDatabase())
            {
                var product = context.Repairs
                .Include(rec => rec.RepairMaterials)
               .ThenInclude(rec => rec.Material)
               .FirstOrDefault(rec => rec.RepairName == model.RepairName || rec.Id
               == model.Id);
                return product != null ?
                new RepairViewModel
                {
                    Id = product.Id,
                    RepairName = product.RepairName,
                    Price = product.Price,
                    RepairMaterials = product.RepairMaterials
                .ToDictionary(recPC => recPC.MaterialId, recPC =>
               (recPC.Material?.MaterialName, recPC.Count))
                } :
               null;
            }
        }
        public void Insert(RepairBindingModel model)
        {
            using (var context = new RepairShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new Repair(), context);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public void Update(RepairBindingModel model)
        {
            using (var context = new RepairShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try

                    {
                        var element = context.Repairs.FirstOrDefault(rec => rec.Id ==
                       model.Id);
                        if (element == null)
                        {
                            throw new Exception("Элемент не найден");
                        }
                        CreateModel(model, element, context);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public void Delete(RepairBindingModel model)
        {
            using (var context = new RepairShopDatabase())
            {
                var element = context.Repairs.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element != null)
                {
                    context.Repairs.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }
        private Repair CreateModel(RepairBindingModel model, Repair product,
       RepairShopDatabase context)
        {
            product.RepairName = model.RepairName;
            product.Price = model.Price;
            if (product.Id == 0)
            {
                context.Repairs.Add(product);
                context.SaveChanges();
            }
            if (model.Id.HasValue)
            {
                var productMaterials = context.RepairMaterials.Where(rec =>
               rec.RepairId == model.Id.Value).ToList();

                context.RepairMaterials.RemoveRange(productMaterials.Where(rec =>
               !model.RepairMaterials.ContainsKey(rec.RepairId)).ToList());
                context.SaveChanges();

                foreach (var updateMaterial in productMaterials)
                {
                    updateMaterial.Count =
                   model.RepairMaterials[updateMaterial.MaterialId].Item2;
                    model.RepairMaterials.Remove(updateMaterial.RepairId);
                }
                context.SaveChanges();
            }
            // добавили новые
            foreach (var pc in model.RepairMaterials)
            {
                context.RepairMaterials.Add(new RepairMaterial
                {
                    RepairId = product.Id,
                    MaterialId = pc.Key,
                    Count = pc.Value.Item2
                });
                context.SaveChanges();
            }
            return product;
        }

    }
}
