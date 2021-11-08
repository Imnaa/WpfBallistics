using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallisticLib
{
    public class CalcParams
    {
        /// <summary>
        /// Шаг интегрирования
        /// </summary>
        private double stepIntegr = 0;
        /// <summary>
        /// Время расчета
        /// </summary>
        private double timeCalc = 0;
        /// <summary>
        /// Прокуск шагов на акт. участке
        /// </summary>
        private int deltaAct = 0;
        /// <summary>
        /// Прокуск шагов на не акт. участке
        /// </summary>
        private int deltaNotAct = 0;

        public CalcParams(double stepIntegr, double timeCalc, int deltaAct, int deltaNotAct)
        {
            this.stepIntegr = stepIntegr;
            this.timeCalc = timeCalc;
            this.deltaAct = deltaAct;
            this.deltaNotAct = deltaNotAct;
        }

        /// <summary>
        /// Шаг интегрирования
        /// </summary>
        public double StepIntegr { get => stepIntegr; set => stepIntegr = value; }
        /// <summary>
        /// Время расчета
        /// </summary>
        public double TimeCalc { get => timeCalc; set => timeCalc = value; }
        /// <summary>
        /// Прокуск шагов на акт. участке
        /// </summary>
        public int DeltaAct { get => deltaAct; set => deltaAct = value; }
        /// <summary>
        /// Прокуск шагов на не акт. участке
        /// </summary>
        public int DeltaNotAct { get => deltaNotAct; set => deltaNotAct = value; }
    }
}
