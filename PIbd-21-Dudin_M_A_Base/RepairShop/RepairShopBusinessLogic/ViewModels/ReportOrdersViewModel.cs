using RepairShopBusinessLogic.Enums;
using System;
using System.ComponentModel;

namespace RepairShopBusinessLogic.ViewModels
{
    public class ReportOrdersViewModel
    {
        [DisplayName("Дата создания")]
        public DateTime DateCreate { get; set; }
        [DisplayName("Имя изделия")]
        public string RepairName { get; set; }
        [DisplayName("Количество")]
        public int Count { get; set; }
        [DisplayName("Сумма")]
        public decimal Sum { get; set; }
        [DisplayName("Статус")]
        public OrderStatus Status { get; set; }
    }
}
