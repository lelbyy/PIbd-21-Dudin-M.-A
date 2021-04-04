using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using RepairShopBusinessLogic.BindingModels;
using RepairShopBusinessLogic.BusinessLogics;
using RepairShopBusinessLogic.ViewModels;
using System.Windows.Forms;
using Unity;

namespace RepairShopView
{
    public partial class FormRepair : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private readonly RepairLogic logic;
        private int? id;
        private Dictionary<int, (string, int)> repairMaterials;
        public FormRepair(RepairLogic service)
        {
            InitializeComponent();
            this.logic = service;
        }
              
        private void LoadData()
        {
            try
            {
                if (repairMaterials != null)
                {
                    dataGridView.Rows.Clear();
                    foreach (var pc in repairMaterials)
                    {
                        dataGridView.Rows.Add(new object[] { pc.Key, pc.Value.Item1,
                            pc.Value.Item2 });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormRepairMaterial>();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (repairMaterials.ContainsKey(form.Id))
                {
                    repairMaterials[form.Id] = (form.MaterialName, form.Count);
                }
                else
                {
                    repairMaterials.Add(form.Id, (form.MaterialName, form.Count));
                }
                LoadData();
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<FormRepairMaterial>();
                int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                form.Id = id;
                form.Count = repairMaterials[id].Item2;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    repairMaterials[form.Id] = (form.MaterialName, form.Count);
                    LoadData();
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {

                        repairMaterials.Remove(Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value));
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            if (repairMaterials == null || repairMaterials.Count == 0)
            {
                MessageBox.Show("Заполните материалы", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            try
            {
                logic.CreateOrUpdate(new RepairBindingModel
                {
                    Id = id,

                    RepairName = textBoxName.Text,
                    Price = Convert.ToDecimal(textBoxPrice.Text),
                    RepairMaterials = repairMaterials
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void FormRepair_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    RepairViewModel view = logic.Read(new RepairBindingModel
                    {
                        Id = id.Value
                    })?[0];
                    if (view != null)
                    {
                        textBoxName.Text = view.RepairName;
                        textBoxPrice.Text = view.Price.ToString();
                        repairMaterials = view.RepairMaterials;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                }
            }
            else
            {
                repairMaterials = new Dictionary<int, (string, int)>();
            }
        }
    }
}

