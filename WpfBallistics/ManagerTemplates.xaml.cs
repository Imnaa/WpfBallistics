using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace WpfBallistics
{
    /// <summary>
    /// Логика взаимодействия для ManagerTemplates.xaml
    /// </summary>
    public partial class ManagerTemplates : Window
    {
        /// <summary>
        /// Конструктор окна
        /// </summary>
        public ManagerTemplates()
        {
            InitializeComponent();
            CreateGUI();
            DgUpdate();

            this.Closed += ManagerTemplates_Closed;
        }
        // Событие закрытия окна
        private void ManagerTemplates_Closed(object sender, EventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            GC.Collect();
        }
        /// <summary>
        /// Обновление DataGrid'a
        /// </summary>
        private void DgUpdate()
        {
            // usage file worker
            FileWorker fw = new FileWorker(Global.FileIniPath);
            // Узнаем количество строк
            int countStr = System.IO.File.ReadAllLines(Global.FileIniPath).Length;
            // Количество секций (РСЗО)
            int count = countStr / 11;
            // get data from ini file and add in dg
            DataGrid dg = FormHelper.GetDataGrid(MainRoot, "dg");
            if (null == dg)
            {
                MessageBox.Show("Cant find dg! managerTemplates  - private void BtnSave_Click(object sender, RoutedEventArgs e)");
                return;
            }
            // создание таблицы данных
            DataTable dt = new DataTable();
            // создание колонок
            for (int i = 0; i < Ballistic.countParams; ++i)
            {
                DataColumn dc = new DataColumn()
                {
                    ColumnName = Ballistic.GetNameOfParam(i, false)
                };
                // не работает (( хз почему, нет интнернета у нас((( ПАМАГИТИиииии!!!
                //dc.Caption = b.getNameRusOfParamForHeader(dc.ColumnName);
                dt.Columns.Add(dc);
            }
            dt.TableName = "dt";
            // запись инфы из файла
            for (int i = 0; i < count; ++i)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < Ballistic.countParams; ++j)
                {
                    dr[j] = fw.GetPrivateString(i.ToString(), Ballistic.GetNameOfParam(j, false)).Replace(".", ",");
                }
                dt.Rows.Add(dr);
            }
            dg.ItemsSource = dt.DefaultView;
        }
        /// <summary>
        /// Создание интерфейса
        /// </summary>
        private void CreateGUI()
        {
            DataGrid dg = new DataGrid()
            {
                Name = "dg",
                CanUserAddRows = true,
                CanUserDeleteRows = true,
                CanUserReorderColumns = false,
                CanUserResizeColumns = true,
                CanUserSortColumns = true,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Visible,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                AutoGenerateColumns = true,
                ColumnHeaderHeight = 100
            };
            dg.AutoGeneratingColumn += Dg_AutoGeneratingColumn;
            Grid.SetRow(dg, 1); Grid.SetColumn(dg, 1);
            Grid.SetColumnSpan(dg, 3);
            MainRoot.Children.Add(dg);

            Button btnSave = new Button()
            {
                Name = "btnSave",
                Margin = new Thickness(0, 5, 0, 0),
                Content = "Сохранить"
            };
            btnSave.Click += BtnSave_Click;
            Grid.SetRow(btnSave, 2); Grid.SetColumn(btnSave, 2);
            Grid.SetColumnSpan(btnSave, 1);
            MainRoot.Children.Add(btnSave);

            Button btnClose = new Button()
            {
                Name = "btnClose",
                Margin = new Thickness(0, 5, 0, 0),
                Content = "Закрыть"
            };
            btnClose.Click += BtnClose_Click;
            Grid.SetRow(btnClose, 2); Grid.SetColumn(btnClose, 3);
            Grid.SetColumnSpan(btnClose, 1);
            MainRoot.Children.Add(btnClose);
        }
        // Событие на нажатие на выход
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        // если нажать на сохранить
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dg = FormHelper.GetDataGrid(MainRoot, "dg");
            if (null == dg)
            {
                MessageBox.Show("Cant find dg! managerTemplates  - private void BtnSave_Click(object sender, RoutedEventArgs e)");
                return;
            }
            // проверка всех ячеек
            for (int i = 0; i < dg.Items.Count - 1; ++i)
            {
                DataRowView data = dg.Items[i] as DataRowView;

                for (int j = 0; j < Ballistic.countParams; ++j)
                {
                    try
                    {
                        if (data[j].ToString().Length == 0)
                        {
                            throw new Exception();
                        }

                        if (j != 0)
                        {
                            Convert.ToDouble(data[j]);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Все ячейки должны быть заполнены и быть в правильном формате!\nОшибка: " + ex.Message);
                        return;
                    }
                }
            }
            // запись в файл
            // перезапись файла (пустой будет)
            System.IO.File.WriteAllText(Global.FileIniPath, "");
            // далее пишем
            FileWorker fw = new FileWorker(Global.FileIniPath);

            for (int i = 0; i < dg.Items.Count - 1; ++i)
            {
                DataRowView data = dg.Items[i] as DataRowView;

                for (int j = 0; j < Ballistic.countParams; ++j)
                {
                    fw.WritePrivateString(i.ToString(), Ballistic.GetNameOfParam(j, false), data[j].ToString());
                }
            }

            this.Close();
        }
        // Делаем красивые заголовки столбцов (захотелось)
        private void Dg_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            TextBlock tbl = new TextBlock()
            {
                TextAlignment = TextAlignment.Center,
                Text = Ballistic.GetNameRusOfParamForHeader(e.PropertyName)
            };

            e.Column.Header = tbl;

        }
    }
}
