using System;

namespace WpfBallistics
{
    /// <summary>
    /// Статический класс для объявление "ГЛОБАЛЬНЫХ" переменных
    /// </summary>
    static class Global
    {
        // Home dir
        public static readonly string HOMEDIR = System.IO.Directory.GetCurrentDirectory() + "\\";
        // ini file with settings
        public static readonly string SettingsINI = HOMEDIR + "Settings.ini";
        // ini file with templates
        public static readonly string FileIniPath = HOMEDIR + "Templates.ini";
        // название папки с результатами
        public static readonly string ResultDirName = "Results";
        // полный путь до результирующих файлов файлов
        public static readonly string ResultDir = HOMEDIR + ResultDirName + "\\";
        // UTC который нужен
        public static sbyte UTC;
        // метод для обновления нужных значений
        public static void UpdateSettings()
        {
            // usage file worker
            FileWorker fw = new FileWorker(Global.SettingsINI);
            // Узнаем количество строк
            UTC = Convert.ToSByte(fw.GetPrivateString("Settings", "UTC"));
        }
    }
}
