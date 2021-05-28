﻿using RepairShopBusinessLogic.Enums;
using RepairShopFileImplement.Models;
using RepairShopListImplement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RepairShopFileImplement.Models
{
	public class FileDataListSingleton
	{
		private static FileDataListSingleton instance;

		private readonly string MaterialFileName = "Material.xml";
		private readonly string OrderFileName = "Order.xml";
		private readonly string RepairFileName = "Repair.xml";
		private readonly string ClientFileName = "Client.xml";
		private readonly string ImplementerFileName = "Implementer.xml";
		private readonly string MessageFileName = "Message.xml";
		public List<Material> Materials { get; set; }
		public List<Order> Orders { get; set; }
		public List<Repair> Repairs { get; set; }
		public List<Client> Clients { get; set; }
		public List<Implementer> Implementers { get; set; }
		public List<MessageInfo> Messages { get; set; }

		private FileDataListSingleton()
		{
			Materials = LoadMaterials();
			Orders = LoadOrders();
			Repairs = LoadRepairs();
			Clients = LoadClients();
			Implementers = LoadImplementers();
			Messages = LoadMessages();
		}

		public static FileDataListSingleton GetInstance()
		{
			if (instance == null)
			{
				instance = new FileDataListSingleton();
			}
			return instance;
		}

		~FileDataListSingleton()
		{
			SaveMaterials();
			SaveOrders();
			SaveRepairs();
			SaveClients();
			SaveImplementers();
			SaveMessages();
		}

		private List<Material> LoadMaterials()
		{
			var list = new List<Material>();
			if (File.Exists(MaterialFileName))
			{
				XDocument xDocument = XDocument.Load(MaterialFileName);
				var xElements = xDocument.Root.Elements("Material").ToList();
				foreach (var elem in xElements)
				{
					list.Add(new Material
					{
						Id = Convert.ToInt32(elem.Attribute("Id").Value),
						MaterialName = elem.Element("MaterialName").Value
					});
				}
			}
			return list;
		}

		private List<Order> LoadOrders()
		{
			var list = new List<Order>();
			if (File.Exists(OrderFileName))
			{
				XDocument xDocument = XDocument.Load(OrderFileName);
				var xElements = xDocument.Root.Elements("Order").ToList();
				foreach (var elem in xElements)
				{
					OrderStatus status = (OrderStatus)0; 
					switch ((elem.Element("Status").Value))
					{
						case "Принят":
							status = (OrderStatus)0;
							break;
						case "Выполняется":
							status = (OrderStatus)1;
							break;
						case "Готов":
							status = (OrderStatus)2;
							break;
						case "Оплачен":
							status = (OrderStatus)3;
							break;
							
					}
					if (string.IsNullOrEmpty(elem.Element("DateImplement").Value))
					{
						list.Add(new Order
						{
							Id = Convert.ToInt32(elem.Attribute("Id").Value),
							RepairId = Convert.ToInt32(elem.Element("RepairId").Value),
							RepairName = elem.Element("RepairName").Value,
							Count = Convert.ToInt32(elem.Element("Count").Value),
							Sum = Convert.ToDecimal(elem.Element("Sum").Value),
							Status = status,
							DateCreate = Convert.ToDateTime(elem.Element("DateCreate").Value)
						});
					}
					else
					{
						list.Add(new Order
						{
							Id = Convert.ToInt32(elem.Attribute("Id").Value),
							RepairId = Convert.ToInt32(elem.Element("RepairId").Value),
							RepairName = elem.Element("RepairName").Value,
							Count = Convert.ToInt32(elem.Element("Count").Value),
							Sum = Convert.ToDecimal(elem.Element("Sum").Value),
							Status = status,
							DateCreate = Convert.ToDateTime(elem.Element("DateCreate").Value),
							DateImplement = Convert.ToDateTime(elem.Element("DateImplement").Value)
						});
					}	
				if (!string.IsNullOrEmpty(elem.Element("ImplementerId").Value))
					{
						order.ImplementerId = Convert.ToInt32(elem.Element("ImplementerId").Value);
					}					
				}
			}
			return list;
		}

		private List<Repair> LoadRepairs()
		{
			var list = new List<Repair>();
			if (File.Exists(RepairFileName))
			{
				XDocument xDocument = XDocument.Load(RepairFileName);
				var xElements = xDocument.Root.Elements("Repair").ToList();
				foreach (var elem in xElements)
				{
					var repairMaterials = new Dictionary<int, int>();
					foreach (var materials in
				   elem.Element("RepairMaterials").Elements("RepairMaterials").ToList())
					{
						repairMaterials.Add(Convert.ToInt32(materials.Element("Key").Value),
					   Convert.ToInt32(materials.Element("Value").Value));
					}
					list.Add(new Repair
					{
						Id = Convert.ToInt32(elem.Attribute("Id").Value),
						RepairName = elem.Element("RepairName").Value,
						Price = Convert.ToDecimal(elem.Element("Price").Value),
						RepairMaterials = repairMaterials
					});
				}
			}
			return list;
		}

		private List<Implementer> LoadImplementers()
		{
			var list = new List<Implementer>();
			if (File.Exists(ImplementerFileName))
			{
				XDocument xDocument = XDocument.Load(ImplementerFileName);
				var xElements = xDocument.Root.Elements("Implementers").ToList();
				foreach (var elem in xElements)
				{
					list.Add(new Implementer
					{
						Id = Convert.ToInt32(elem.Attribute("Id").Value),
						Name = elem.Element("Name").Value,
						WorkingTime = Convert.ToInt32(elem.Element("WorkingTime").Value),
						PauseTime = Convert.ToInt32(elem.Element("PauseTime").Value)
					});
				}
			}
			return list;
		}

		private List<Client> LoadClients()
		{
			var list = new List<Client>();
			if (File.Exists(ClientFileName))
			{
				XDocument xDocument = XDocument.Load(ClientFileName);
				var xElements = xDocument.Root.Elements("Clients").ToList();
				foreach (var elem in xElements)
				{
					list.Add(new Client
					{
						Id = Convert.ToInt32(elem.Attribute("Id").Value),
						ClientFIO = elem.Element("ClientFIO").Value,
						Email = elem.Element("Email").Value,
						Password = elem.Element("Password").Value
					});
				}
			}
			return list;
		}

		private List<MessageInfo> LoadMessages()
		{
			var list = new List<MessageInfo>();
			if (File.Exists(MessageFileName))
			{
				XDocument xDocument = XDocument.Load(MessageFileName);
				var xElements = xDocument.Root.Elements("Message").ToList();
				foreach (var elem in xElements)
				{
					list.Add(new MessageInfo
					{
						MessageId = elem.Attribute("MessageId").Value,
						ClientId = Convert.ToInt32(elem.Element("ClientId").Value),
						SenderName = elem.Element("SenderName").Value,
						DateDelivery = Convert.ToDateTime(elem.Element("DateDelivery")?.Value),
						Subject = elem.Element("Subject").Value,
						Body = elem.Element("Body").Value,
					});
				}
			}
			return list;
		}

		private void SaveMaterials()
		{
			if (Materials != null)
			{
				var xElement = new XElement("Materials");
				foreach (var material in Materials)
				{
					xElement.Add(new XElement("Material",
					new XAttribute("Id", material.Id),
					new XElement("MaterialName", material.MaterialName)));
				}
				XDocument xDocument = new XDocument(xElement);
				xDocument.Save(MaterialFileName);
			}
		}

		private void SaveOrders()
		{
			if (Orders != null)
			{
				var xElement = new XElement("Orders");
				foreach (var order in Orders)
				{					
						xElement.Add(new XElement("Order",
					 new XAttribute("Id", order.Id),
					 new XElement("RepairId", order.RepairId),
					 new XElement("RepairName", order.RepairName),
					 new XElement("Count", order.Count),
					 new XElement("Sum", order.Sum),
					 new XElement("Status", order.Status),
					 new XElement("DateCreate", order.DateCreate),
					 new XElement("DateImplement", order.DateImplement)
					 ));
					}
				XDocument xDocument = new XDocument(xElement);
				xDocument.Save(OrderFileName);
			}
		}

		private void SaveRepairs()
		{
			if (Repairs != null)
			{
				var xElement = new XElement("Repair");
				foreach (var repair in Repairs)
				{
					var materialElement = new XElement("RepairMaterials");
					foreach (var material in repair.RepairMaterials)
					{
						materialElement.Add(new XElement("RepairMaterial",
						new XElement("Key", material.Key),
						new XElement("Value", material.Value)));
					}
					xElement.Add(new XElement("Repair",
					 new XAttribute("Id", repair.Id),
					 new XElement("RepairName", repair.RepairName),
					 new XElement("Price", repair.Price),
					 materialElement));
				}
				XDocument xDocument = new XDocument(xElement);
				xDocument.Save(RepairFileName);
			}
		}
		private void SaveClients()
		{
			if (Clients != null)
			{
				var xElement = new XElement("Clients");
				foreach (var client in Clients)
				{
					xElement.Add(new XElement("Client",
					new XAttribute("Id", client.Id),
					new XElement("ClientFIO", client.ClientFIO),
					new XElement("Email", client.Email),
					new XElement("Password", client.Password)));
				}
				XDocument xDocument = new XDocument(xElement);
				xDocument.Save(ClientFileName);
			}
		}
		private void SaveImplementers()
		{
			if (Implementers != null)
			{
				var xElement = new XElement("Implementers");
				foreach (var client in Implementers)
				{
					xElement.Add(new XElement("Implementer",
					new XAttribute("Id", client.Id),
					new XElement("Name", client.Name),
					new XElement("WorkingTime", client.WorkingTime),
					new XElement("PauseTime", client.PauseTime)));
				}
				XDocument xDocument = new XDocument(xElement);
				xDocument.Save(ImplementerFileName);
			}
		}

		private void SaveMessages()
		{
			if (Messages != null)
			{
				var xElement = new XElement("Messages");
				foreach (var message in Messages)
				{
					xElement.Add(new XElement("Message",
					new XAttribute("MessageId", message.MessageId),
					new XElement("ClientId", message.ClientId),
					new XElement("SenderName", message.SenderName),
					new XElement("DateDelivery", message.DateDelivery),
					new XElement("Subject", message.Subject),
					new XElement("Body", message.Body)
					));
				}
				XDocument xDocument = new XDocument(xElement);
				xDocument.Save(MessageFileName);
			}
		}		
	}
}
