using System;
using Microsoft.EntityFrameworkCore;
using RepairShopDatabaseImplement.Models;

namespace RepairShopDatabaseImplement
{
    public class RepairShopDatabase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=RepairShopDatabase2;Integrated Security=True;MultipleActiveResultSets=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<Material> Materials { set; get; }
        public virtual DbSet<Repair> Repairs { set; get; }
        public virtual DbSet<RepairMaterial> RepairMaterials { set; get; }
        public virtual DbSet<Order> Orders { set; get; }
    }
}
