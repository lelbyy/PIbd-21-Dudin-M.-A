using System.Runtime.Serialization;
namespace RepairShopBusinessLogic.BindingModels
{
    [DataContract]
    public class CreateOrderBindingModel
    {
	[DataMember]
        public int ClientId { get; set; }
	[DataMember]
        public int RepairId { get; set; }
	[DataMember]
        public string RepairName { get; set; }
	[DataMember]
        public int Count { get; set; }
	[DataMember]
        public decimal Sum { get; set; }
    }
}