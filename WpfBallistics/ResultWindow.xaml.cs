using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfBallistics
{
    /// <summary>
    /// Логика взаимодействия для ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public ResultWindow()
        {
            InitializeComponent();

            this.Title = "Результаты предыдущих вычислений";
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Height = 450;
            this.Width = 400;

            this.Closed += ResultWindow_Closed;

            CreateGUI();
            GetResultFiles();
        }
        // Вывод в листвью списка результаттов
        private void GetResultFiles()
        {
            // получаем инфу о папке
            DirectoryInfo RootDir = new DirectoryInfo(Global.ResultDir);
            DirectoryInfo[] dirs = RootDir.GetDirectories("*");
            // получаем lv и чистим
            ListView lv = FormHelper.GetListView(MainRoot, "lv_files");
            lv.Items.Clear();
            // добавляем в lv
            foreach (DirectoryInfo dir in dirs)
            {
                lv.Items.Add(dir.Name);
            }
            // далее выбираем элемент из lv
            Button btn_Open = FormHelper.GetButton(MainRoot, "btn_open");
            Button btn_Del = FormHelper.GetButton(MainRoot, "btn_del");

            if (dirs.Length > 0)
            {
                lv.SelectedItem = dirs[0].Name;
                btn_Del.IsEnabled = true;
                btn_Open.IsEnabled = true;
            }
            else
            {
                btn_Del.IsEnabled = false;
                btn_Open.IsEnabled = false;
            }
        }

        private void CreateGUI()
        {
            ListView lv = new ListView()
            {
                Name = "lv_files"
            };
            lv.MouseDoubleClick += Lv_MouseDoubleClick;
            lv.Items.Clear();
            Grid.SetRow(lv, 1); Grid.SetColumn(lv, 1);
            Grid.SetRowSpan(lv, 6);
            MainRoot.Children.Add(lv);

            Button btn_open = new Button() 
            {
                Name = "btn_open",
                Content = "Открыть"
            };
            btn_open.Click += Btn_open_Click;
            Grid.SetRow(btn_open, 1); Grid.SetColumn(btn_open, 2);
            Grid.SetRowSpan(btn_open, 2);
            MainRoot.Children.Add(btn_open);

            Button btn_del = new Button()
            {
                Name = "btn_del",
            Content = "Удалить"
            };
            btn_del.Click += Btn_del_Click;
            Grid.SetRow(btn_del, 3); Grid.SetColumn(btn_del, 2);
            Grid.SetRowSpan(btn_del, 2);
            MainRoot.Children.Add(btn_del);

            Button btn_close = new Button()
            {
                Name = "btn_close",
                Content = "Закрыть"
            };
            btn_close.Click += Btn_close_Click;
            Grid.SetRow(btn_close, 5); Grid.SetColumn(btn_close, 2);
            Grid.SetRowSpan(btn_close, 2);
            MainRoot.Children.Add(btn_close);

        }

        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Lv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer", Directory.GetCurrentDirectory() + "\\" + Global.ResultDir.Replace("/", "\\") + ((ListView)e.Source).SelectedItem.ToString());
        }

        private void Btn_del_Click(object sender, RoutedEventArgs e)
        {
            ListView lv = FormHelper.GetListView(MainRoot, "lv_files");
            Directory.Delete(Global.ResultDir + lv.SelectedItem.ToString(), true);
            GetResultFiles();
        }

        private void Btn_open_Click(object sender, RoutedEventArgs e)
        {
            ListView lv = FormHelper.GetListView(MainRoot, "lv_files");
            System.Diagnostics.Process.Start("explorer", Global.ResultDir + lv.SelectedItem.ToString());
        }

        private void ResultWindow_Closed(object sender, EventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            GC.Collect();
        }
    }
}
