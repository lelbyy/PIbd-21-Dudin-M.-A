using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using RepairShopBusinessLogic.BindingModels;
using RepairShopBusinessLogic.Enums;
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
                    .Include(rec => rec.Client)
		    .Include(rec => rec.Implementer)
                    .Select(CreateModel).ToList();
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
                 .Include(rec => rec.Repair)
                 .Include(rec => rec.Client)
                 .Include(rec => rec.Implementer)
                    .Where(rec => (!model.DateFrom.HasValue && !model.DateTo.HasValue &&
                    rec.DateCreate.Date == model.DateCreate.Date) ||
                     (model.DateFrom.HasValue && model.DateTo.HasValue &&
                    rec.DateCreate.Date >= model.DateFrom.Value.Date && rec.DateCreate.Date <=
                    model.DateTo.Value.Date) ||
                     (model.ClientId.HasValue && rec.ClientId == model.ClientId) ||
                    (model.FreeOrders.HasValue && model.FreeOrders.Value && rec.Status ==
                    OrderStatus.Принят) ||
                     (model.ImplementerId.HasValue && rec.ImplementerId ==
                    model.ImplementerId && rec.Status == OrderStatus.Выполняется))
                 .Select(CreateModel).ToList();
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
		.Include(rec => rec.Client)
                .Include(rec => rec.Implementer)
                .FirstOrDefault(rec => rec.Id == model.Id);
                return order != null ?
                CreateModel(order) :
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

	private OrderViewModel CreateModel(Order order)
        {
            return new OrderViewModel
            {
                Id = order.Id,
                RepairId = order.RepairId,
                ClientId = order.ClientId.Value,
                ImplementerId = order.ImplementerId,
                ClientFIO = order.Client.ClientFIO,
                RepairName = order.Repair.RepairName,
                Count = order.Count,
                Sum = order.Sum,
                Status = order.Status,
                DateCreate = order.DateCreate,
                DateImplement = order?.DateImplement,
                ImplementerName = order.ImplementerId.HasValue ?
                    order.Implementer.Name : string.Empty

            };
        } 

        private Order CreateModel(OrderBindingModel model, Order order)
        {
            order.RepairId = model.RepairId;
            order.Sum = model.Sum;
            order.ClientId = model.ClientId.Value;
	    order.ImplementerId = model.ImplementerId;
            order.Count = model.Count;
            order.Status = model.Status;
            order.Sum = model.Sum;
            order.DateCreate = model.DateCreate;
            order.DateImplement = model.DateImplement;

            return order;
        }
    }
}
