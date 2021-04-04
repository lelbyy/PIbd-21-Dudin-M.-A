using RepairShopBusinessLogic.Enums;
using System;

namespace RepairShopFileImplement.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int RepairId { get; set; }
        public string RepairName { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateImplement { get; set; }
    }
}
