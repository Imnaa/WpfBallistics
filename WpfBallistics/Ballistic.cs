using System;
using System.Collections.Generic;
using System.Text;

namespace WpfBallistics
{
    /// <summary>
    /// Массив для описания параметров баллистической информации
    /// </summary>
    class Ballistic
    {
        private string name;
        private double fuelMass;
        private double massPocketPath;
        private double massHeadPath;
        private double calibr;
        private double avgValFt;
        private double timeFuelFire;
        private double dlinaNapravl;
        private double usilieStoporen;
        private double koeffForm;
        public static readonly int countParams = 10;
        public string Name { get => name;  set => name = value; }
        public double FuelMass { get => fuelMass; set => fuelMass = value; }
        public double MassPocketPath { get => massPocketPath; set => massPocketPath = value; }
        public double MassHeadPath { get => massHeadPath; set => massHeadPath = value; }
        public double Calibr { get => calibr; set => calibr = value; }
        public double AvgValFt { get => avgValFt; set => avgValFt = value; }
        public double TimeFuelFire { get => timeFuelFire; set => timeFuelFire = value; }
        public double DlinaNapravl { get => dlinaNapravl; set => dlinaNapravl = value; }
        public double UsilieStoporen { get => usilieStoporen; set => usilieStoporen = value; }
        public double KoeffForm { get => koeffForm; set => koeffForm = value; }
        /// <summary>
        /// Конструктор класса Ballistic
        /// </summary>
        public Ballistic()
        {
            name = null;
            fuelMass = 0.0;
            massPocketPath = 0.0;
            massHeadPath = 0.0;
            calibr = 0.0;
            avgValFt = 0.0;
            timeFuelFire = 0.0;
            dlinaNapravl = 0.0;
            usilieStoporen = 0.0;
            koeffForm = 0.0;
        }
        /// <summary>
        /// Конструктор класса Ballistic
        /// </summary>
        /// <param name="name">Название экспериментальных данных</param>
        /// <param name="fuelMass">Масса топлива</param>
        /// <param name="massPocketPath">Масса обечайки ракетной части</param>
        /// <param name="massHeadPath">Масса головной части</param>
        /// <param name="calibr">Калибр</param>
        /// <param name="avgValFt">Среднее значение F тяги</param>
        /// <param name="timeFuelFire">Время горения топлива</param>
        /// <param name="dlinaNapravl">Длина направляющей</param>
        /// <param name="usilieStoporen">Усиление стопорения</param>
        /// <param name="koeffForm">Коэффициент формы</param>
        public Ballistic(string name = null, double fuelMass = 0.0, double massPocketPath = 0.0, double massHeadPath = 0.0, 
            double calibr = 0.0, double avgValFt = 0.0, double timeFuelFire = 0.0, double dlinaNapravl = 0.0, 
            double usilieStoporen = 0.0, double koeffForm = 0.0)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.fuelMass = fuelMass;
            this.massPocketPath = massPocketPath;
            this.massHeadPath = massHeadPath;
            this.calibr = calibr;
            this.avgValFt = avgValFt;
            this.timeFuelFire = timeFuelFire;
            this.dlinaNapravl = dlinaNapravl;
            this.usilieStoporen = usilieStoporen;
            this.koeffForm = koeffForm;
        }
        /// <summary>
        /// Проверка значений класса на инициализацию
        /// </summary>
        /// <returns>true - все хорошо</returns>
        public bool CheckValues()
        {
            return !(name == null || name == ""
                || fuelMass == 0
                || massPocketPath == 0
                || massHeadPath == 0
                || calibr == 0
                || avgValFt == 0
                || timeFuelFire == 0
                || dlinaNapravl == 0
                || usilieStoporen == 0
                || koeffForm == 0);
            //{
            //    throw new ArgumentNullException("Входные данные баллистики не инициализированы!");
            //}
        }
        /// <summary>
        /// Метод для получения рус или инг названия поля
        /// </summary>
        /// <param name="id">индекс поля</param>
        /// <param name="isRus">рус или анг нужен</param>
        /// <returns>Возврат рус или инг названия поля</returns>
        public static string GetNameOfParam(int id, bool isRus)
        {
            return id switch
            {
                0 => isRus ? "Название" : "Name",
                1 => isRus ? "Масса топлива" : "FuelMass",
                2 => isRus ? "Масса обечайки ракетной части" : "MassPocketPath",
                3 => isRus ? "Масса головной части" : "MassHeadPath",
                4 => isRus ? "Калибр" : "Calibr",
                5 => isRus ? "Ср.знач. F тяги" : "AvgValFt",
                6 => isRus ? "Время горения топлива" : "TimeFuelFire",
                7 => isRus ? "Длина направляющей" : "DlinaNapravl",
                8 => isRus ? "Усилие спорения" : "UsilieStoporen",
                9 => isRus ? "Коэффициент формы" : "KoeffForm",
                _ => throw new ArgumentException("id error"),
            };
        }
        /// <summary>
        /// Получить по названию поля рус описание
        /// </summary>
        /// <param name="header">Название поля</param>
        /// <returns>Русс название</returns>
        public static string GetNameRusOfParamForHeader(string header)
        {
            return header switch
            {
                "Name" => "Название",
                "FuelMass" => "Масса\nтоплива",
                "MassPocketPath" => "Масса\nобечайки\nракетной\nчасти",
                "MassHeadPath" => "Масса\nголовной\nчасти",
                "Calibr" => "Калибр",
                "AvgValFt" => "Ср.знач.\nF тяги",
                "TimeFuelFire" => "Время\nгорения\nтоплива",
                "DlinaNapravl" => "Длина\nнаправляющей",
                "UsilieStoporen" => "Усилие\nспорения",
                "KoeffForm" => "Коэффициент\nформы",
                _ => throw new ArgumentException("id error"),
            };
        }

    }
}
