namespace RepairShopBusinessLogic.BindingModels
{
    public class CreateOrderBindingModel
    {
        public int RepairId { get; set; }
        public string RepairName { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
    }
}