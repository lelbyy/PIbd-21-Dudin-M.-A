﻿using RepairShopBusinessLogic.BindingModels;
using RepairShopBusinessLogic.BusinessLogics;
using System;
using System.Windows.Forms;
using Unity;


namespace RepairShopView
{
    public partial class FormClients : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        private readonly ClientLogic logic;
        public FormClients(ClientLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
               Program.ConfigGrid(logic.Read(null), dataGridView);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id =
                   Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                    try
                    {
                        logic.Delete(new ClientBindingModel { Id = id });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
