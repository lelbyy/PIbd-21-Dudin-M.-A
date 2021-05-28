﻿using RepairShopBusinessLogic.BusinessLogics;
using RepairShopBusinessLogic.Interfaces;
using RepairShopBusinessLogic.HelperModels;
using RepairShopDatabaseImplement.Implements;
using System;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;
using System.Configuration;
using System.Threading;

namespace RepairShopView
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();
	    MailLogic.MailConfig(new MailConfig
            {
                SmtpClientHost = ConfigurationManager.AppSettings["SmtpClientHost"],
                SmtpClientPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpClientPort"]),
                MailLogin = ConfigurationManager.AppSettings["MailLogin"],
                MailPassword = ConfigurationManager.AppSettings["MailPassword"],
            });
            var timer = new System.Threading.Timer(new TimerCallback(MailCheck), new MailCheckInfo
            {
                PopHost = ConfigurationManager.AppSettings["PopHost"],
                PopPort = Convert.ToInt32(ConfigurationManager.AppSettings["PopPort"]),
                Storage = container.Resolve<IMessageInfoStorage>(),
                ClientStorage = container.Resolve<IClientStorage>()
            }, 0, 10000);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<FormMain>());
        }
        private static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<IMaterialStorage, MaterialStorage>(new
           HierarchicalLifetimeManager());
             currentContainer.RegisterType<MaterialLogic>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IOrderStorage, OrderStorage>(new
 	   HierarchicalLifetimeManager());
            currentContainer.RegisterType<OrderLogic>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IRepairStorage, RepairStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<RepairLogic>(new
	   HierarchicalLifetimeManager());
            currentContainer.RegisterType<IClientStorage, ClientStorage>(new
 	   HierarchicalLifetimeManager());
            currentContainer.RegisterType<ClientLogic>(new
	   HierarchicalLifetimeManager());
            currentContainer.RegisterType<IImplementerStorage, ImplementerStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<ImplementerStorage>(new
	   HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMessageInfoStorage, MessageInfoStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<MailLogic>(new
           HierarchicalLifetimeManager());
	    currentContainer.RegisterType<ReportLogic>(new
           HierarchicalLifetimeManager());
            return currentContainer;
        }
	 private static void MailCheck(object obj)
        {
            MailLogic.MailCheck((MailCheckInfo)obj);
        }      
    }
}
