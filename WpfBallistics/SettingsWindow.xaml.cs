using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfBallistics
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            this.Title = "Настройки программы";
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            this.Closed += SettingsWindow_Closed;

            CreateGUI();
        }

        private void SettingsWindow_Closed(object sender, EventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            GC.Collect();
        }

        private void CreateGUI()
        {
            TextBlock lHomeDir = new TextBlock()
            {
                Text = "Путь до программы",
                Margin = new Thickness(0, 0, 0, 0)
            };
            Grid.SetRow(lHomeDir, 1); Grid.SetColumn(lHomeDir, 1);
            MainRoot.Children.Add(lHomeDir);

            TextBox tbHomeDir = new TextBox()
            {
                Text = System.IO.Directory.GetCurrentDirectory(),
                Name = "tbHomeDir",
                Margin = new Thickness(0, 0, 0, 0),
                IsEnabled = false
            };
            Grid.SetRow(tbHomeDir, 1); Grid.SetColumn(tbHomeDir, 2);
            MainRoot.Children.Add(tbHomeDir);

            Button btnOpenHomeDir = new Button()
            {
                Content = "Открыть"
            };
            btnOpenHomeDir.Click += BtnOpenHomeDir_Click;
            Grid.SetRow(btnOpenHomeDir, 1); Grid.SetColumn(btnOpenHomeDir, 3);
            MainRoot.Children.Add(btnOpenHomeDir);

            TextBlock lFileInPath = new TextBlock()
            {
                Text = "Путь до файла с шаблонами",
                Margin = new Thickness(0, 0, 0, 0)
            };
            Grid.SetRow(lFileInPath, 2); Grid.SetColumn(lFileInPath, 1);
            MainRoot.Children.Add(lFileInPath);

            TextBox tbFileInPath = new TextBox()
            {
                Text = Global.FileIniPath,
                Name = "tbFileInPath",
                IsEnabled = false,
                Margin = new Thickness(0, 0, 0, 0)
            };
            Grid.SetRow(tbFileInPath, 2); Grid.SetColumn(tbFileInPath, 2);
            MainRoot.Children.Add(tbFileInPath);

            Button btnFileInPath = new Button()
            {
                Content = "Открыть"
            };
            btnFileInPath.Click += BtnFileInPath_Click;
            Grid.SetRow(btnFileInPath, 2); Grid.SetColumn(btnFileInPath, 3);
            MainRoot.Children.Add(btnFileInPath);

            TextBlock lResultDir = new TextBlock()
            {
                Text = "Путь до сохранения результатов",
                Margin = new Thickness(0, 0, 0, 0)
            };
            Grid.SetRow(lResultDir, 3); Grid.SetColumn(lResultDir, 1);
            MainRoot.Children.Add(lResultDir);

            TextBox tbResultDir = new TextBox()
            {
                Text = Global.ResultDir,
                Name = "tbResultDir",
                IsEnabled = false,
                Margin = new Thickness(0, 0, 0, 0)
            };
            Grid.SetRow(tbResultDir, 3); Grid.SetColumn(tbResultDir, 2);
            MainRoot.Children.Add(tbResultDir);

            Button btnResultDir = new Button()
            {
                Content = "Открыть"
            };
            btnResultDir.Click += BtnResultDir_Click;
            Grid.SetRow(btnResultDir, 3); Grid.SetColumn(btnResultDir, 3);
            MainRoot.Children.Add(btnResultDir);

            TextBlock lUTC = new TextBlock()
            {
                Text = "UTC",
                Margin = new Thickness(0, 0, 0, 0)
            };
            Grid.SetRow(lUTC, 4); Grid.SetColumn(lUTC, 1);
            MainRoot.Children.Add(lUTC);

            TextBox tbUTC = new TextBox()
            {
                Text = Global.UTC.ToString(),
                Name = "tbUTC",
                Margin = new Thickness(0, 0, 0, 0)
            };
            Grid.SetRow(tbUTC, 4); Grid.SetColumn(tbUTC, 2);
            MainRoot.Children.Add(tbUTC);

            Button btnSave = new Button()
            {
                Content = "Сохранить"
            };
            btnSave.Click += BtnSave_Click;
            Grid.SetRow(btnSave, 5); Grid.SetColumn(btnSave, 3);
            MainRoot.Children.Add(btnSave);

            Button btnClose = new Button()
            {
                Content = "Закрыть"
            };
            btnClose.Click += BtnClose_Click;
            Grid.SetRow(btnClose, 5); Grid.SetColumn(btnClose, 4);
            MainRoot.Children.Add(btnClose);

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            TextBox tb = FormHelper.GetTextBox(MainRoot, "tbUTC");
            if (tb.Text.Length == 0)
            {
                MessageBox.Show("Введите UTC!");
                return;
            }

            sbyte UTC;

            try
            {
                Convert.ToSByte(tb.Text);
            }
            catch
            {
                MessageBox.Show("Введите правильно UTC!");
                return;
            }

            UTC = Convert.ToSByte(tb.Text);

            FileWorker fw = new FileWorker(Global.SettingsINI);
            fw.WritePrivateString("Settings", "UTC", UTC.ToString());

            this.Close();
        }

        private void BtnResultDir_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer", System.IO.Directory.GetCurrentDirectory() + "\\Results");
        }

        private void BtnFileInPath_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer", System.IO.Directory.GetCurrentDirectory());
        }

        private void BtnOpenHomeDir_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer", System.IO.Directory.GetCurrentDirectory());
        }
    }
}
