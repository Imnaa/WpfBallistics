using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WpfBallistics
{
    /// <summary>
    /// Класс для работы с файлами
    /// </summary>
    internal class FileWorker
    {
        //Поля класса
        private const int SIZE = 10240; //Максимальный размер (для чтения значения из файла)
        private string path = null; //Для хранения пути к INI-файлу

        //Конструктор без аргументов (путь к INI-файлу нужно будет задать отдельно)
        public FileWorker() : this("") { }
        //Конструктор, принимающий путь к INI-файлу
        public FileWorker(string _path)
        {
            path = _path;
        }
        // РАБОТА С INI файлами
        //Возвращает значение из INI-файла (по указанным секции и ключу) 
        public string GetPrivateString(string aSection, string aKey)
        {
            //Для получения значения
            StringBuilder buffer = new StringBuilder(SIZE);

            //Получить значение в buffer
            GetPrivateString(aSection, aKey, null, buffer, SIZE, path);

            //Вернуть полученное значение
            return buffer.ToString();
        }
        //Пишет значение в INI-файл (по указанным секции и ключу) 
        public void WritePrivateString(string aSection, string aKey, string aValue)
        {
            //Записать значение в INI-файл
            WritePrivateString(aSection, aKey, aValue, path);
        }
        //Возвращает или устанавливает путь к INI файлу
        public string Path { get { return path; } set { path = value; } }
        //Импорт функции GetPrivateProfileString (для чтения значений) из библиотеки kernel32.dll
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateString(string section, string key, string def, StringBuilder buffer, int size, string path);
        //Импорт функции WritePrivateProfileString (для записи значений) из библиотеки kernel32.dll
        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        private static extern int WritePrivateString(string section, string key, string str, string path);
        // КОНЕЦ INI

        // Логика получения данных из INI файла
        public Dictionary<string, string[]> GetValuesForLbTemplates()
        {
            // Узнаем количество строк
            int countStr = System.IO.File.ReadAllLines(path).Length;
            // Количество секций (РСЗО)
            int count = countStr / 11;
            // Массив для значений
            Dictionary<string, string[]> result = new Dictionary<string, string[]>()
            {
                { "name", new string[count] },
                { "tbFuelMass", new string[count] },
                {"tbMassPocketPath", new string[count]},
                {"tbMassHeadPath", new string[count]},
                {"tbCalibr", new string[count]},
                {"tbAvgValFt", new string[count]},
                {"tbTimeFuelFire", new string[count]},
                {"tbDlinaNapravl", new string[count]},
                {"tbUsilieStoporen", new string[count]},
                {"tbKoeffForm", new string[count]}
            };

            string[] names = new string[count];
            string[] tbFuelMass = new string[count];
            string[] tbMassPocketPath = new string[count];
            string[] tbMassHeadPath = new string[count];
            string[] tbCalibr = new string[count];
            string[] tbAvgValFt = new string[count];
            string[] tbTimeFuelFire = new string[count];
            string[] tbDlinaNapravl = new string[count];
            string[] tbUsilieStoporen = new string[count];
            string[] tbKoeffForm = new string[count];

            // Добавляем в массив значения из INI
            for (int i = 0; i < count; ++i)
            {
                //Получить значение по ключу name из секции main
                names[i] = GetPrivateString(i.ToString(), "name");
                tbFuelMass[i] = GetPrivateString(i.ToString(), "tbFuelMass");
                tbMassPocketPath[i] = GetPrivateString(i.ToString(), "tbMassPocketPath");
                tbMassHeadPath[i] = GetPrivateString(i.ToString(), "tbMassHeadPath");
                tbCalibr[i] = GetPrivateString(i.ToString(), "tbCalibr");
                tbAvgValFt[i] = GetPrivateString(i.ToString(), "tbAvgValFt");
                tbTimeFuelFire[i] = GetPrivateString(i.ToString(), "tbTimeFuelFire");
                tbDlinaNapravl[i] = GetPrivateString(i.ToString(), "tbDlinaNapravl");
                tbUsilieStoporen[i] = GetPrivateString(i.ToString(), "tbUsilieStoporen");
                tbKoeffForm[i] = GetPrivateString(i.ToString(), "tbKoeffForm");
            }

            result["name"] = names;
            result["tbFuelMass"] = tbFuelMass;
            result["tbMassPocketPath"] = tbMassPocketPath;
            result["tbMassHeadPath"] = tbMassHeadPath;
            result["tbCalibr"] = tbCalibr;
            result["tbAvgValFt"] = tbAvgValFt;
            result["tbTimeFuelFire"] = tbTimeFuelFire;
            result["tbDlinaNapravl"] = tbDlinaNapravl;
            result["tbUsilieStoporen"] = tbUsilieStoporen;
            result["tbKoeffForm"] = tbKoeffForm;

            return result;
        }
    }
}