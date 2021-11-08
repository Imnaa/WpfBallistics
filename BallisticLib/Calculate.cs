using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallisticLib
{
    public class Calculate
    {
        public struct Result
        {
            /// <summary>
            /// Все Х
            /// </summary>
            public double[] X;
            /// <summary>
            /// Все Y
            /// </summary>
            public double[] Y;
            /// <summary>
            /// Время полета
            /// </summary>
            public double[] FlyTimes;
            /// <summary>
            /// Скорость ракеты
            /// </summary>
            public double[] FlyV;
            /// <summary>
            /// Поведение снаряда по оси Х
            /// </summary>
            public double[] a_x;
            /// <summary>
            /// Поведение снаряда по оси У
            /// </summary>
            public double[] a_y;
        }
        /// <summary>
        /// Метод для вычисления
        /// </summary>
        /// <param name="ballistic">Параметры снаряда</param>
        /// <param name="angle">Угол стрельбы</param>
        /// <param name="calcParams">Параметры вычисления</param>
        /// <returns>Возврат массив типа Result</returns>
        public Result Start(Ballistic ballistic, double angle, CalcParams calcParams)
        {
            if (ballistic is null && !ballistic.CheckValues())
            {
                throw new ArgumentNullException(nameof(ballistic));
            }

            if (calcParams is null)
            {
                throw new ArgumentNullException(nameof(calcParams));
            }

            // init mathmodel
            MathModel mm = new MathModel();
            bool isGood = mm.Start(ballistic, calcParams, angle);

            if (isGood == false)
            {
                throw new ArgumentException("При вычислении MathModel возникла проблема!");
            }

            Result result = new Result()
            {
                a_x = new double[mm.Answer.Count],
                a_y = new double[mm.Answer.Count],
                X = new double[mm.Answer.Count],
                Y = new double[mm.Answer.Count],
                FlyTimes = new double[mm.Answer.Count],
                FlyV = mm.GetV().ToArray()
            };

            for (int i = 0; i < mm.Answer.Count; ++i)
            {
                result.a_x[i] = mm.Answer[i][0];
                result.a_y[i] = mm.Answer[i][1];
                result.X[i] = mm.Answer[i][2];
                result.Y[i] = mm.Answer[i][3];
                result.FlyTimes[i] = calcParams.StepIntegr * i;
            }

            return result;

        }
    }
}
