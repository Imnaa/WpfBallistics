using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfBallistics
{
    public partial class MainWindow : Window
    {
        // max count exp
        private readonly int rowsExp = 20;
        private readonly int colsExp = 2;
        // class' constructor 
        public MainWindow()
        {
            // Обновление глобальных переменных
            Global.UpdateSettings();

            InitializeComponent();
            // Событие на отслеживание изменений в tbCountExp
            tbCountExp.TextChanged += TbCountExp_TextChanged;
            // Загрузка данных
            LbTemplatesLoad();
            SetDefaultValues();
            // Выбор первого элемента в листе с шаблонами
            if (lv_Templates.Items.Count > 0)
            {
                lv_Templates.SelectedIndex = 0;
            }
            // событие для отслеживание закрытия формы
            this.Closed += MainWindow_Closed;
        }
        // Метод для обработки события изменения данных в текстбоксе кол-ва экспериментов
        // генерит текстбокси с углами
        private void TbCountExp_TextChanged(object sender, TextChangedEventArgs e)
        {
            int value = 0;

            try
            {
                value = Convert.ToInt32(((TextBox)e.Source).Text);

                if (value > rowsExp * colsExp)
                {
                    throw new Exception($"Введено число больше {rowsExp * colsExp}!");
                }
            } catch (Exception ex)
            {
                if (value > 40)
                {
                    MessageBox.Show("Ошибка при вводе количества экспериментов!\nОшибка: " + ex.Message);
                }
                ((TextBox)e.Source).Text = "";
                return;
            }

            // delete controls
            while (FormHelper.DeleteTextBlock(MainRoot, "tblAngle")) ;
            while (FormHelper.DeleteTextBox(MainRoot, "tbAngle")) ;
            while (FormHelper.DeleteRectangle(MainRoot, "rectAngle")) ;
            // create controls
            for (int j = 0, cur = 0; j < colsExp && cur < value; ++j)
            {
                for (int i = 0; cur < value && i < rowsExp; ++i, ++cur)
                {
                    TextBlock tbl = new TextBlock()
                    {
                        Name = $"tblAngle{cur}",
                    Text = $"Угол {cur + 1}",
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Height = 16
                    };
                    Grid.SetRow(tbl, i + 2); Grid.SetColumn(tbl, 7 + j * 2);
                    MainRoot.Children.Add(tbl);

                    Rectangle rect = new Rectangle()
                    {
                        Name = $"rectAngle{cur}",
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Height = 2,
                        Fill = Brushes.Black,
                        Stroke = Brushes.Black
                    };
                    Grid.SetRow(rect, i + 2); Grid.SetColumn(rect, 7 + j * 2);
                    Grid.SetColumnSpan(rect, 2);
                    MainRoot.Children.Add(rect);

                    TextBox tb = new TextBox
                    {
                        Name = $"tbAngle{cur}",
                        Text = "",
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Height = 18
                    };

                    Grid.SetRow(tb, i + 2); Grid.SetColumn(tb, 8 + j * 2);
                    MainRoot.Children.Add(tb);
                }
            }
        }
        // Обработка закрытия формы
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
        // storage with params for calcing 
        private Ballistic[] ballistic;
        private sbyte indexBallistic = 0;
        // get params for storage
        private Ballistic[] GetBallisticData(string filepath)
        {
            Ballistic[] result;
            // Узнаем количество строк
            int countStr;
            try
            {
                countStr = System.IO.File.ReadAllLines(filepath).Length;
            } catch (Exception e)
            {
                MessageBox.Show("Нет файла с шаблонами\nОшибка: " + e.Message);
                return null;
            }
            // Количество секций (РСЗО)
            int count = countStr / 11;
            // Инициализация объекта для хранения данных
            result = new Ballistic[count];
            // Получаем данные из файла
            FileWorker fw = new FileWorker(filepath);
            // Добавляем в массив значения из INI
            for (int i = 0; i < count; ++i)
            {
                result[i] = new Ballistic();

                try
                {
                    //Получить значение по ключу name из секции main
                    result[i].Name = fw.GetPrivateString(i.ToString(), Ballistic.GetNameOfParam(0, false));
                    result[i].FuelMass = Convert.ToDouble(fw.GetPrivateString(i.ToString(), Ballistic.GetNameOfParam(1, false)).Replace(".", ","));
                    result[i].MassPocketPath = Convert.ToDouble(fw.GetPrivateString(i.ToString(), Ballistic.GetNameOfParam(2, false)).Replace(".", ","));
                    result[i].MassHeadPath = Convert.ToDouble(fw.GetPrivateString(i.ToString(), Ballistic.GetNameOfParam(3, false)).Replace(".", ","));
                    result[i].Calibr = Convert.ToDouble(fw.GetPrivateString(i.ToString(), Ballistic.GetNameOfParam(4, false)).Replace(".", ","));
                    result[i].AvgValFt = Convert.ToDouble(fw.GetPrivateString(i.ToString(), Ballistic.GetNameOfParam(5, false)).Replace(".", ","));
                    result[i].TimeFuelFire = Convert.ToDouble(fw.GetPrivateString(i.ToString(), Ballistic.GetNameOfParam(6, false)).Replace(".", ","));
                    result[i].DlinaNapravl = Convert.ToDouble(fw.GetPrivateString(i.ToString(), Ballistic.GetNameOfParam(7, false)).Replace(".", ","));
                    result[i].UsilieStoporen = Convert.ToDouble(fw.GetPrivateString(i.ToString(), Ballistic.GetNameOfParam(8, false)).Replace(".", ","));
                    result[i].KoeffForm = Convert.ToDouble(fw.GetPrivateString(i.ToString(), Ballistic.GetNameOfParam(9, false)).Replace(".", ","));
                } catch (Exception e)
                {
                    MessageBox.Show("Неправильные данные в ini файле\nИсправьте данные!\nОшибка: " + e.Message);
                    return null;
                }

                
            }
            return result;
        }
        // update listbox with templates
        private void LbTemplatesLoad()
        {
            lv_Templates.Items.Clear();
            ballistic = GetBallisticData(Global.FileIniPath);
            if (ballistic == null)
            {
                return;
            }
            for (int i = 0; i < ballistic.Length; ++i)
            {
                lv_Templates.Items.Add(ballistic[i].Name);
            }
        }
        // event for button close
        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        // event for button results
        private void Btn_Results_Click(object sender, RoutedEventArgs e)
        {
            // ЗДесь новое окно с результатами!
            ResultWindow rw = new ResultWindow();
            rw.Show();
            this.Hide();
        }
        // event for button templates
        private void Btn_Templates_Click(object sender, RoutedEventArgs e)
        {
            ManagerTemplates mt = new ManagerTemplates();
            this.Hide();
            mt.Show();
        }
        // set default params
        private void SetDefaultValues()
        {
            tbStepIntegr.Text = "0.005";
            tbTimeCalc.Text = "200";
            tbCountExp.Text = "1";

            tbDeltaAct.Text = "1";
            tbDeltaNotAct.Text = "1";
            tbDeltaOutAct.Text = "30";
            tbDeltaOutNotAct.Text = "60";
        }
        // event for button clear
        private void Btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            foreach(UIElement el in MainRoot.Children)
            {
                if (el is TextBox box)
                {
                    box.Text = "";
                }
            }
            SetDefaultValues();
        }
        // event for button apply
        private void Btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            if (lv_Templates.SelectedValue == null || (string)lv_Templates.SelectedValue == "")
            {
                MessageBox.Show("Выберете из списка шаблон!");
                return;
            }
            // 
            for (int i = 0; i < ballistic.Length; ++i)
            {
                if ((string)lv_Templates.SelectedValue == ballistic[i].Name)
                {
                    tbFuelMass.Text = ballistic[i].FuelMass.ToString();
                    tbMassPocketPath.Text = ballistic[i].MassPocketPath.ToString();
                    tbMassHeadPath.Text = ballistic[i].MassHeadPath.ToString();
                    tbCalibr.Text = ballistic[i].Calibr.ToString();
                    tbAvgValFt.Text = ballistic[i].AvgValFt.ToString();
                    tbTimeFuelFire.Text = ballistic[i].TimeFuelFire.ToString();
                    tbDlinaNapravl.Text = ballistic[i].DlinaNapravl.ToString();
                    tbUsilieStoporen.Text = ballistic[i].UsilieStoporen.ToString();
                    tbKoeffForm.Text = ballistic[i].KoeffForm.ToString();
                    indexBallistic = (sbyte)i;
                    break;
                }
            }
            
        }
        // event for button calc
        private void Btn_Calc_Click(object sender, RoutedEventArgs e)
        {
            List<double> angles = new List<double>();
            Dictionary<string, double> calcParams = new Dictionary<string, double>();
            // проверка полей ввода
            foreach (UIElement el in MainRoot.Children)
            {
                if (el is TextBox box)
                {
                    // замена .  на , для метода Convert.To*....
                    box.Text = box.Text.Trim().Replace(".", ",");

                    double value = 0.0;

                    try
                    {
                        value = Convert.ToDouble(box.Text.Trim());

                        if (box.Text == "" || value <= 0)
                        {
                            throw new Exception("Заполните все поля правильно!");
                        }
                    } catch (Exception ex)
                    {
                        MessageBox.Show("Неправильные данные введены в тексовых полях ввода!\nОшибка: " + ex.Message);
                        return;
                    }

                    if (box.Name.Contains("tbAngle"))
                    {
                        angles.Add(value);
                    }
                    else if (box.Name.Contains("tbStepIntegr"))
                    {
                        calcParams.Add("tbStepIntegr", value);
                    }
                    else if (box.Name.Contains("tbTimeCalc"))
                    {
                        calcParams.Add("tbTimeCalc", value);
                    }
                    else if (box.Name.Contains("tbDeltaAct"))
                    {
                        calcParams.Add("tbDeltaAct", value);
                    }
                    else if (box.Name.Contains("tbDeltaNotAct"))
                    {
                        calcParams.Add("tbDeltaNotAct", value);
                    }
                    else if (box.Name.Contains("tbDeltaOutAct"))
                    {
                        calcParams.Add("tbDeltaOutAct", value);
                    }
                    else if (box.Name.Contains("tbDeltaOutNotAct"))
                    {
                        calcParams.Add("tbDeltaOutNotAct", value);
                    }

                }
            }
            // запись данных в объект с параметрами эксперимента
            if (ballistic == null)
            {
                ballistic = new Ballistic[1];
                ballistic[0] = new Ballistic()
                {
                    Name = "Без имени",
                    FuelMass = Convert.ToDouble(tbFuelMass.Text.Replace(".", ",")),
                    MassPocketPath = Convert.ToDouble(tbMassPocketPath.Text.Replace(".", ",")),
                    MassHeadPath = Convert.ToDouble(tbMassHeadPath.Text.Replace(".", ",")),
                    Calibr = Convert.ToDouble(tbCalibr.Text.Replace(".", ",")),
                    AvgValFt = Convert.ToDouble(tbAvgValFt.Text.Replace(".", ",")),
                    TimeFuelFire = Convert.ToDouble(tbTimeFuelFire.Text.Replace(".", ",")),
                    DlinaNapravl = Convert.ToDouble(tbDlinaNapravl.Text.Replace(".", ",")),
                    UsilieStoporen = Convert.ToDouble(tbUsilieStoporen.Text.Replace(".", ",")),
                    KoeffForm = Convert.ToDouble(tbKoeffForm.Text.Replace(".", ","))
                };
                indexBallistic = 0;

            }

            if (tbCountExp.Text == "")
            {
                MessageBox.Show("Введите количество углов!");
                return;
            }

            if (angles.Count == 0)
            {
                MessageBox.Show("Нет введенных углов! Введите!");
                return;
            }
            // вызов результирующего окна
            CalcedWindow cw = new CalcedWindow(ballistic[indexBallistic], calcParams, angles);
            cw.Show();
            this.Hide();
            
        }
        // Обработка нажатия на кнопку "Настройки"
        private void Btn_Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();
            sw.Show();
            this.Hide();
        }
    }
}
