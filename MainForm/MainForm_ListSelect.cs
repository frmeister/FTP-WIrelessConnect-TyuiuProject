using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainForm
{
    public partial class MainForm_ListSelect : Form
    {
        public List<string> SelectedFiles { get; private set; } = new List<string>();

        public MainForm_ListSelect(List<string> availableFiles)
        {
            InitializeComponent();

            // Убедимся, что контрол существует и видим
            if (selector_checkedListBox == null)
            {
                MessageBox.Show("Ошибка: selector_checkedListBox не инициализирован!");
                return;
            }

            selector_checkedListBox.Items.Clear();
            if (availableFiles != null && availableFiles.Count > 0)
            {
                foreach (var file in availableFiles)
                    selector_checkedListBox.Items.Add(file);
                Debug.WriteLine($"Добавлено файлов в список: {selector_checkedListBox.Items.Count}");
            }
            else
            {
                Debug.WriteLine("Список файлов пуст");
                selector_checkedListBox.Items.Add("Нет файлов для отображения");
            }
        }

        private void selector_buttonOK_Click(object sender, EventArgs e)
        {
            SelectedFiles.Clear();
            foreach (var item in selector_checkedListBox.CheckedItems)
            {
                SelectedFiles.Add(item.ToString());
            }
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void selector_buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}