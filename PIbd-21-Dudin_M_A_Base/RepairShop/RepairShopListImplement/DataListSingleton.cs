using RepairShopListImplement.Models;
using System.Collections.Generic;

namespace RepairShopListImplement
{
    public class DataListSingleton
    {
        private static DataListSingleton instance;
        public List<Material> Materials { get; set; }
        public List<Order> Orders { get; set; }
        public List<Repair> Repairs { get; set; }
	public List<Client> Clients { get; set; }
        private DataListSingleton()
        {
            Materials = new List<Material>();
            Orders = new List<Order>();
            Repairs = new List<Repair>();
	    Clients = new List<Client>();
        }
        public static DataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new DataListSingleton();
            }
            return instance;
        }
    }
}