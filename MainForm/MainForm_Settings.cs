using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainForm
{
    public partial class MainForm_Settings : Form
    {
        public MainForm_Settings()
        {
            InitializeComponent();
            InfoInit();

            // Подписываемся на изменения только после инициализации загрузки
            settings_textBoxNickname_Input.TextChanged += settings_textBoxNickname_Input_TextChanged;
            settings_textBoxSourceData_Output.TextChanged += settings_textBoxSourceData_Output_TextChanged;
        }

        private string nickName = ConfigManager.GetValue("nickName");
        private string folderPath = ConfigManager.GetValue("dataPath");

        private bool _isNickNameChanged = false;
        private bool _isFolderPathChanged = false;

        // Флаг для готовности к работе после настройки
        public bool _isConfigurated = true;

        private void InfoInit()
        {
            // Загрузка ника
            if (!string.IsNullOrEmpty(nickName))
                settings_textBoxNickname_Input.Text = nickName;

            // Загрузка пути к файлам
            if (!string.IsNullOrEmpty(folderPath))
                settings_textBoxSourceData_Output.Text = folderPath;
            else
                settings_textBoxSourceData_Output.Text = "Путь не установлен";
        }

        private void settings_textBoxNickname_Input_TextChanged(object sender, EventArgs e)
        {
            _isNickNameChanged = true;
        }

        private void settings_textBoxSourceData_Output_TextChanged(object sender, EventArgs e)
        {
            _isFolderPathChanged = true;
        }

        #region BUTTONS

        private void settings_buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Сохраняем ник если он был изменен
                if (_isNickNameChanged)
                {
                    nickName = settings_textBoxNickname_Input.Text;

                    ConfigManager.SetValue("nickName", nickName);
                }

                if (_isFolderPathChanged)
                {
                    folderPath = settings_textBoxSourceData_Output.Text;

                    ConfigManager.SetValue("dataPath", folderPath);
                }
            }
            catch
            {

            }
        }

        private void settings_buttonSourceData_ChangeDirectory_Click(object sender, EventArgs e)
        {
            string selectedPath;

            settings_folderBrowserDialog.ShowDialog();
            selectedPath = settings_folderBrowserDialog.SelectedPath;

            settings_textBoxSourceData_Output.Text = selectedPath;
        }

        private void settings_buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

    }
}
