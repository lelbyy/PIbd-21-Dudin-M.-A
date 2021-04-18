using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using RepairShopBusinessLogic.BindingModels;
using RepairShopBusinessLogic.Interfaces;
using RepairShopBusinessLogic.ViewModels;
using RepairShopDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace RepairShopDatabaseImplement.Implements
{
    public class OrderStorage : IOrderStorage
    {
        public List<OrderViewModel> GetFullList()
        {
            using (RepairShopDatabase context = new RepairShopDatabase())
            {
                return context.Orders
                .Include(rec => rec.Repair)
                .Select(rec => new OrderViewModel
                {
                    Id = rec.Id,
                    RepairId = rec.RepairId,
                    RepairName = rec.Repair.RepairName,
                    Count = rec.Count,
                    Sum = rec.Sum,
                    Status = rec.Status,
                    DateCreate = rec.DateCreate,
                    DateImplement = rec.DateImplement,
                })
                .ToList();
            }
        }
        public List<OrderViewModel> GetFilteredList(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (RepairShopDatabase context = new RepairShopDatabase())
            {
                return context.Orders
                .Where(rec => ( rec.DateCreate.Date == model.DateCreate.Date))
                .Include(rec => rec.Repair)
                .Select(rec => new OrderViewModel
                {
                    Id = rec.Id,
                    RepairId = rec.RepairId,
                    RepairName = rec.Repair.RepairName,
                    Count = rec.Count,
                    Sum = rec.Sum,
                    Status = rec.Status,
                    DateCreate = rec.DateCreate,
                    DateImplement = rec.DateImplement,
                })
                .ToList();
            }
        }
        public OrderViewModel GetElement(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (RepairShopDatabase context = new RepairShopDatabase())
            {
                Order order = context.Orders
                .Include(rec => rec.Repair)
                .FirstOrDefault(rec => rec.Id == model.Id);
                return order != null ?
                new OrderViewModel
                {
                    Id = order.Id,
                    RepairId = order.RepairId,
                    RepairName = order.Repair.RepairName,
                    Count = order.Count,
                    Sum = order.Sum,
                    Status = order.Status,
                    DateCreate = order.DateCreate,
                    DateImplement = order.DateImplement,
                } :
                null;
            }
        }
        public void Insert(OrderBindingModel model)
        {
            using (RepairShopDatabase context = new RepairShopDatabase())
            {
                Order order = new Order
                {
                    RepairId = model.RepairId,
                    Count = model.Count,
                    Sum = model.Sum,
                    Status = model.Status,
                    DateCreate = model.DateCreate,
                    DateImplement = model.DateImplement,
                };
                context.Orders.Add(order);
                context.SaveChanges();
                CreateModel(model, order);
                context.SaveChanges();
            }
        }
        public void Update(OrderBindingModel model)
        {
            using (var context = new RepairShopDatabase())
            {
                var element = context.Orders.FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element);
                context.SaveChanges();
            }
        }
        public void Delete(OrderBindingModel model)
        {
            using (RepairShopDatabase context = new RepairShopDatabase())
            {
                Order element = context.Orders.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Orders.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }
        private Order CreateModel(OrderBindingModel model, Order order)
        {
            if (model == null)
            {
                return null;
            }

            using (RepairShopDatabase context = new RepairShopDatabase())
            {
                Repair element = context.Repairs.FirstOrDefault(rec => rec.Id == model.RepairId);
                if (element != null)
                {
                    if (element.Orders == null)
                    {
                        element.Orders = new List<Order>();
                    }
                    element.Orders.Add(order);
                    context.Repairs.Update(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
            return order;
        }
    }
}
