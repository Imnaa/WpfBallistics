using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallisticLib
{
    public class Ballistic
    {
        /// <summary>
        /// Название эксперимента
        /// </summary>
        private string name;
        /// <summary>
        /// Масса топлива
        /// </summary>
        private double fuelMass;
        /// <summary>
        /// Масса обечайки ракетной части
        /// </summary>
        private double massPocketPath;
        /// <summary>
        /// Масса головной части
        /// </summary>
        private double massHeadPath;
        /// <summary>
        /// Калибр
        /// </summary>
        private double calibr;
        /// <summary>
        /// Среднее значение F тяги
        /// </summary>
        private double avgValFt;
        /// <summary>
        /// Время горения топлива
        /// </summary>
        private double timeFuelFire;
        /// <summary>
        /// Длина направляющей
        /// </summary>
        private double dlinaNapravl;
        /// <summary>
        /// Усилие стопорения
        /// </summary>
        private double usilieStoporen;
        /// <summary>
        /// Коеффициент формы
        /// </summary>
        private double koeffForm;
        /// <summary>
        /// Название эксперимента
        /// </summary>
        public string Name { get => name; set => name = value; }
        /// <summary>
        /// Масса топлива
        /// </summary>
        public double FuelMass { get => fuelMass; set => fuelMass = value; }
        /// <summary>
        /// Масса обечайки ракетной части
        /// </summary>
        public double MassPocketPath { get => massPocketPath; set => massPocketPath = value; }
        /// <summary>
        /// Масса головной части
        /// </summary>
        public double MassHeadPath { get => massHeadPath; set => massHeadPath = value; }
        /// <summary>
        /// Калибр
        /// </summary>
        public double Calibr { get => calibr; set => calibr = value; }
        /// <summary>
        /// Среднее значение F тяги
        /// </summary>
        public double AvgValFt { get => avgValFt; set => avgValFt = value; }
        /// <summary>
        /// Время горения топлива
        /// </summary>
        public double TimeFuelFire { get => timeFuelFire; set => timeFuelFire = value; }
        /// <summary>
        /// Длина направляющей
        /// </summary>
        public double DlinaNapravl { get => dlinaNapravl; set => dlinaNapravl = value; }
        /// <summary>
        /// Усилие стопорения
        /// </summary>
        public double UsilieStoporen { get => usilieStoporen; set => usilieStoporen = value; }
        /// <summary>
        /// Коеффициент формы
        /// </summary>
        public double KoeffForm { get => koeffForm; set => koeffForm = value; }
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
    }
}
