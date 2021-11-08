using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;

using WpfBallistics.ExcelWorker;

using LiveCharts;
using LiveCharts.Wpf;

namespace WpfBallistics
{
    /// <summary>
    /// Логика взаимодействия для CalcedWindow.xaml
    /// </summary>
    public partial class CalcedWindow : Window
    {
        // Объект для хранения параметров вычислений
        private readonly Ballistic ballistic;
        // Лист с углами
        private readonly List<double> Angles;
        // Словарь с настройками вычислений
        private readonly Dictionary<string, double> CalcParams;
        // Путь до результата
        private string PathDefault = "";
        // Дата вычисления экспериментов
        private readonly DateTime Date;
        // Название формы
        private readonly string WindowName = "CalcedWindow";
        /// <summary>
        /// Конструктор формы
        /// </summary>
        /// <param name="_ballistic">Объект с параметрами вычислений</param>
        /// <param name="_calcParams">Словарь с настройками вычислений</param>
        /// <param name="_angles">Лист с углами</param>
        public CalcedWindow(object _ballistic, Dictionary<string, double> _calcParams, List<double> _angles)
        {
            InitializeComponent();
            // Настройка формы
            this.Name = WindowName;
            this.Title = "Результат вычисления траекторий полета";
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            // События
            this.Closed += CalcedWindow_Closed;
            // Получение данных
            ballistic = (Ballistic)_ballistic;
            Angles = _angles;
            CalcParams = _calcParams;
            // Создание GUI
            CreateGUI();
            // Инициализация нужных переменных
            Date = DateTime.UtcNow.AddHours(Global.UTC);
            PathDefault = Global.ResultDir + ConvertDateToFormat(Date) + "\\";
            // Создание директории для результатов
            Directory.CreateDirectory(PathDefault);
            // Вычисление результата
            Calc();
        }
        /// <summary>
        /// Создание GUI
        /// </summary>
        private void CreateGUI()
        {
            Button btnResult = new Button()
            {
                Name = "btnResult",
                Content = "Результат",
            };
            btnResult.Click += BtnResult_Click;
            Grid.SetRow(btnResult, 2);
            Grid.SetColumn(btnResult, 2);
            MainRoot.Children.Add(btnResult);

            Button btnSave = new Button()
            {
                Name = "btnSave",
                Content = "Сохранить"
            };
            btnSave.Click += BtnSave_Click; ;
            Grid.SetRow(btnSave, 3);
            Grid.SetColumn(btnSave, 2);
            MainRoot.Children.Add(btnSave);

            Button btnClose = new Button()
            {
                Name = "btnClose",
                Content = "Закрыть"
            };
            btnClose.Click += BtnClose_Click; ;
            Grid.SetRow(btnClose, 4);
            Grid.SetColumn(btnClose, 2);
            MainRoot.Children.Add(btnClose);
        }
        // Обработка события кнопки "Закрыть" при нажатии
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            // Закрытие формы
            Close();
        }
        // Обработка события кнопки "Сохранить" при нажатии
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Получение папки
            PathDefault = Global.ResultDir + ConvertDateToFormat(Date) + "\\";
            // Получение позиции окна
            int left = 0;
            int top = 0;
            foreach (Window w in Application.Current.Windows)
            {
                if (w.Name == this.WindowName)
                {
                    left = (int)w.Left;
                    top = (int)w.Top;
                    break;
                }
            }
            // Созздание скриншота
            System.Drawing.Rectangle bounds = new System.Drawing.Rectangle(left, top, (int)this.Width, (int)this.Height);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new System.Drawing.Point(bounds.Left, bounds.Top), System.Drawing.Point.Empty, bounds.Size);
                }
                bitmap.Save(PathDefault + "result.jpg", ImageFormat.Jpeg);
            }
            // выкл кнопки
            ((Button)e.Source).IsEnabled = false;
            // открытие папки
            System.Diagnostics.Process.Start("explorer", PathDefault);
        }
        // Обработка события кнопки "Результат" при нажатии
        private void BtnResult_Click(object sender, RoutedEventArgs e)
        {
            // Открытие папки
            string[] dirs = Directory.GetDirectories(Global.ResultDir);
            System.Diagnostics.Process.Start("explorer", dirs.Last());
        }
        // Обработка события закрытия формы
        private void CalcedWindow_Closed(object sender, EventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            GC.Collect();
        }
        /// <summary>
        /// Сохранение вычисленных результатов
        /// </summary>
        /// <param name="mm">Математическая модель</param>
        /// <param name="angle">Угол</param>
        /// <param name="date">Дата вычисления</param>
        /// <param name="i">Номер эксперимента</param>
        private async void SaveResults(MathModel mm, double angle, DateTime date, int i)
        {
            // Получение скоростей
            List<double> V = mm.GetV();
            //// async write to files
            //int deltaAct = (int)calcParams["tbDeltaOutAct"];
            //int deltaNotAct = (int)calcParams["tbDeltaOutNotAct"];
            int deltaAct = (int)CalcParams["tbDeltaAct"];
            int deltaNotAct = (int)CalcParams["tbDeltaNotAct"];
            // Инициализация пути до результата
            PathDefault = Global.ResultDir + ConvertDateToFormat(date) + "\\" + (i + 1) + "\\";
            Directory.CreateDirectory(PathDefault);
            // Инициалиция имен файлов
            string[] fileNames = new string[] { PathDefault + "Result.txt", PathDefault + "Дальность полета.txt", PathDefault + "Высота полета.txt", PathDefault + "Время полета.txt", PathDefault + "Скорость текущая.txt" };
            // Инициализация заданий
            Task[] writingTasks = new Task[fileNames.Length];
            // Сбор всей необходимой информации
            List<string>[] lstr = new List<string>[fileNames.Length];
            // save output
            {
                // 0 file (итоговые значения)
                lstr[0] = new List<string>()
                {
                    "Заданный угол                   = " + Math.Round((double)angle, 3), // ugol +
                    "Дальность полета                = " + FindOperations.FindMax(mm.Answer, 2).ToString("N4").Replace(',', '.').Trim(), // dalnost' +
                    "Высота максимальная             = " + FindOperations.FindMax(mm.Answer, 3).ToString("N4").Replace(',', '.').Trim(), // h_max +
                    "Время полета                    = " + (CalcParams["tbStepIntegr"] * mm.Answer.Count).ToString("N4").Replace(',', '.').Trim(), // t_poleta +
                    "Скорость на активном участке    = " + FindOperations.FindMax(V).ToString("N4").Replace(',', '.').Trim(), // V_akt_uchastok +
                    "Скорость конечная               = " + V.Last().ToString("N4").Replace(',', '.').Trim() // V_konec +
                };
                // 1 file
                lstr[1] = new List<string>();
                // 2 file
                lstr[2] = new List<string>();
                // 3 file
                lstr[3] = new List<string>();
                // 4 file
                lstr[4] = new List<string>();
                // 
                int j = 0;
                if (V.Count < deltaAct)
                {
                    throw new ArgumentException("Проблема с вычислениями CalcedWindow.Calc()!");
                }
                lstr[1].Add("=============== АКТИВНЫЙ УЧАСТОК ===============");
                lstr[2].Add("=============== АКТИВНЫЙ УЧАСТОК ===============");
                lstr[3].Add("=============== АКТИВНЫЙ УЧАСТОК ===============");
                lstr[4].Add("=============== АКТИВНЫЙ УЧАСТОК ===============");
                while (j < mm.Answer.Count - deltaAct && Math.Round(V[j + deltaAct], 1) >= Math.Round(V[j],1))
                {
                    lstr[1].Add(mm.Answer[j][2].ToString("N4").Replace(',', '.').Trim()); // dalnost' +
                    lstr[2].Add(mm.Answer[j][3].ToString("N4").Replace(',', '.').Trim()); // h_max +
                    lstr[3].Add((CalcParams["tbStepIntegr"] * j).ToString("N4").Replace(',', '.').Trim()); // t_poleta +
                    lstr[4].Add(V[j].ToString("N4").Replace(',', '.').Trim()); // V_akt_uchastok +

                    j += deltaAct;
                }
                lstr[1].Add("=============== НЕ АКТИВНЫЙ УЧАСТОК ============");
                lstr[2].Add("=============== НЕ АКТИВНЫЙ УЧАСТОК ============");
                lstr[3].Add("=============== НЕ АКТИВНЫЙ УЧАСТОК ============");
                lstr[4].Add("=============== НЕ АКТИВНЫЙ УЧАСТОК ============");
                while (j < mm.Answer.Count - deltaNotAct)
                {
                    lstr[1].Add(mm.Answer[j][2].ToString("N4").Replace(',', '.').Trim()); // dalnost' +
                    lstr[2].Add(mm.Answer[j][3].ToString("N4").Replace(',', '.').Trim()); // h_max +
                    lstr[3].Add((CalcParams["tbStepIntegr"] * j).ToString("N4").Replace(',', '.').Trim()); // t_poleta +
                    lstr[4].Add(V[j].ToString("N4").Replace(',', '.').Trim()); // V_akt_uchastok +

                    j += deltaNotAct;
                }
            }
            // Вывод информации в Excel:
            var reportData = new ResultReporter()
                .GetReport(lstr, CalcParams, i);
            var reportExcel = new ResultExcelGenerator()
                .Generate(reportData);
            File.WriteAllBytes(Global.ResultDir + ConvertDateToFormat(date) + $"\\Report{i+1}.xlsx", reportExcel);
            // Запись данных в txt файлы
            for (int j = 0; j < fileNames.Length; ++j)
            {
                writingTasks[j] = DoWriteAsync(String.Join("\n", lstr[j].ToArray()), fileNames[j]);
            }
            await Task.WhenAll(writingTasks);
        }
        /// <summary>
        /// Метод для вычисления результатов (всех)
        /// </summary>
        private async void Calc()
        {
            // init mathmodel
            MathModel[] mm = new MathModel[Angles.Count];
            // init tasks
            Task<bool>[] tasks = new Task<bool>[Angles.Count];
            // doing tasks
            for (int i = 0; i < Angles.Count; ++i)
            {
                mm[i] = new MathModel();
                tasks[i] = Task.FromResult(mm[i].Start(ballistic, CalcParams, Angles[i]));
            }
            bool[] res_task = await Task.WhenAll(tasks);
            // для datagrid коллекция
            ObservableCollection<WpfBallistics.ExcelWorker.SecondResultData> collection = null;
            if (collection == null)
            {
                collection = new ObservableCollection<SecondResultData>();
                dgMain.ItemsSource = collection;
            }
            // Перебор всех экспериментов
            for (int i = 0; i < Angles.Count; ++i)
            {
                if (res_task[i] == false)
                {
                    throw new ArgumentException($"При вычислении MathModel i={i} возникла проблема!");
                }

                List<double> V = mm[i].GetV();

                collection.Add(
                    new SecondResultData()
                    {
                        IndexExp = i + 1,
                        Angle = Angles[i].ToString().Replace(".", ","),
                        FlyDistance = FindOperations.FindMax(mm[i].Answer, 2).ToString("N4").Replace(',', '.'),
                        FlyHeightMax = FindOperations.FindMax(mm[i].Answer, 3).ToString("N4").Replace(',', '.'),
                        FlyTime = (CalcParams["tbStepIntegr"] * mm[i].Answer.Count).ToString("N4").Replace(',', '.'),
                        VActMax = FindOperations.FindMax(V).ToString("N4").Replace(',', '.'),
                        VEndMax = V.Last().ToString("N4").Replace(',', '.')
                    }
                    );
                SaveResults(mm[i], Angles[i], Date, i);
            }

            DrawGrafic(mm, Angles);
        }
        /// <summary>
        /// Метод для записи данных в файл
        /// </summary>
        /// <param name="text">Текст который надо вывести</param>
        /// <param name="file">Путь до файла</param>
        /// <returns>return Task</returns>
        private async Task DoWriteAsync(string text, string file)
        {
            using StreamWriter sw = new StreamWriter(file, true, Encoding.Unicode);
            await sw.WriteAsync(text);
        }
        /// <summary>
        /// Метод преобразования даты в нужную строку
        /// </summary>
        /// <param name="date">Дата</param>
        /// <returns>Строка в преобразованном виде</returns>
        private string ConvertDateToFormat(DateTime date)
        {
            string result = date.Year + ".";

            if (date.Month < 10)
            {
                result += "0" + date.Month;
            }
            else
            {
                result += date.Month;
            }
            result += ".";
            if (date.Day < 10)
            {
                result += "0" + date.Day;

            }
            else
            {
                result += date.Day;
            }
            result += "_";
            if (date.Hour < 10)
            {
                result += "0" + date.Hour;

            }
            else
            {
                result += date.Hour;
            }
            result += "-";
            if (date.Minute < 10)
            {
                result += "0" + date.Minute;

            }
            else
            {
                result += date.Minute;
            }
            result += "-";
            if (date.Second < 10)
            {
                result += "0" + date.Second;

            }
            else
            {
                result += date.Second;
            }

            return result;
        }
        // Коллекция для Графика
        public SeriesCollection SeriesCollection { get; set; }
        // Заголовки для графика
        public string[] Labels { get; set; }
        /// <summary>
        /// Отрисовка графика
        /// </summary>
        /// <param name="mm">Массив математических моделей</param>
        /// <param name="angles">Углы</param>
        private void DrawGrafic(MathModel[] mm, List<double> angles)
        {
            SeriesCollection = new SeriesCollection();

            List<string> osX = new List<string>();

            int indexMaxDist = 0;
            double dMaxDist = mm[indexMaxDist].Answer[^1][2];

            for (int i = 0; i < angles.Count; ++i)
            {
                SeriesCollection.Add(new LineSeries
                {
                    Title = "Угол = " + angles[i],
                    LineSmoothness = 0, //0: straight lines, 1: really smooth lines

                    //PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                    //PointGeometrySize = 50,

                    PointForeground = System.Windows.Media.Brushes.Gray
                });

                SeriesCollection[i].Values = new ChartValues<double>();

                List<string> arr = new List<string>();

                int j = 0;
                int deltaAct = (int)CalcParams["tbDeltaOutAct"];
                int deltaNotAct = (int)CalcParams["tbDeltaOutNotAct"];
                // act path
                while (j < mm[i].Answer.Count - deltaAct && mm[i].Answer[j + deltaAct][3] > mm[i].Answer[j][3])
                {
                    SeriesCollection[i].Values.Add(Math.Round(mm[i].Answer[j][3], 1));
                    arr.Add(mm[i].Answer[j][2].ToString("N1"));

                    j += deltaAct;
                }
                // not act path
                while (j < mm[i].Answer.Count - deltaNotAct)
                {
                    SeriesCollection[i].Values.Add(Math.Round(mm[i].Answer[j][3], 1));
                    arr.Add(mm[i].Answer[j][2].ToString("N1"));

                    j += deltaNotAct;
                }

                if (dMaxDist <= mm[i].Answer[^1][2])
                {
                    indexMaxDist = i;
                    dMaxDist = mm[indexMaxDist].Answer[^1][2];
                    osX = arr;
                }
            }
            Labels = osX.ToArray<string>();

            DataContext = this;
        }
    }
}
