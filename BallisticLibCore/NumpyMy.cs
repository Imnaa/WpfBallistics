using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallisticLibCore
{
    static class NumpyMy
    {
        /// <summary>
        /// Функция Arange() возвращает одномерный массив с равномерно разнесенными значениями внутри заданного интервала
        /// </summary>
        /// <param name="start">число, которое является началом интервала</param>
        /// <param name="count">количество элементов</param>
        /// <returns></returns>
        public static IEnumerable<double> Arange(double start, int count)
        {
            return Enumerable.Range((int)start, count).Select(v => (double)v);
        }
        /// <summary>
        /// Функция Power() выполняет возведение элментов массива в степень
        /// </summary>
        /// <param name="exponents">одномерный массив, значения которых необходимо возвести в степень</param>
        /// <param name="baseValue">основание логарифмической шкалы</param>
        /// <returns></returns>
        public static IEnumerable<double> Power(IEnumerable<double> exponents, double baseValue = 10.0d)
        {
            return exponents.Select(v => Math.Pow(baseValue, v));
        }
        /// <summary>
        /// функция LinSpace() возвращает одномерный массив из указанного количества элементов, значения которых равномерно
        /// распределены внутри заданного интервала
        /// </summary>
        /// <param name="start">число которое является началом последовательности</param>
        /// <param name="stop">число которое является концом последовательности </param>
        /// <param name="num">определяет количество элементов последовательности</param>
        /// <param name="endpoint">если true - то значение stop входит в последовательность, иначе нет</param>
        /// <returns>одномерный массив из указанного количества элементов</returns>
        public static IEnumerable<double> LinSpace(double start, double stop, int num, bool endpoint = true)
        {
            var result = new List<double>();
            if (num <= 0)
            {
                return result;
            }
            if (endpoint)
            {
                if (num == 1)
                {
                    return new List<double>() { start };
                }
                var step = (stop - start) / ((double)num - 1.0d);
                result = Arange(0, num).Select(v => (v * step) + start).ToList();
            }
            else
            {
                var step = (stop - start) / (double)num;
                result = Arange(0, num).Select(v => (v * step) + start).ToList();
            }
            return result;
        }
        /// <summary>
        /// Функция LogSpace() возвращает одномерный массив из указанного количества элементов, значения которых равномерно
        /// распределны по логарифмической шкале внутри заданного интервала.
        /// </summary>
        /// <param name="start">число которое является началом последовательности</param>
        /// <param name="stop">число которое является концом последовательности</param>
        /// <param name="num">определяет количество элементов последовательности</param>
        /// <param name="endpoint">если true - то значение stop входит в последовательность, иначе нет</param>
        /// <param name="numericBase">основание логарифмической шкалы</param>
        /// <returns></returns>
        public static IEnumerable<double> LogSpace(double start, double stop, int num, bool endpoint = true, double numericBase = 10.0d)
        {
            var y = LinSpace(start, stop, num: num, endpoint: endpoint);
            return Power(y, numericBase);
        }
        /// <summary>
        /// Возвращает массив
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IEnumerable<double> Array(params double[] values)
        {
            return new List<double>(values.OfType<double>().ToList());
        }
        /// <summary>
        /// Одномерная линейная интерполяция
        /// </summary>
        /// <param name="h_f">координата(ы) по которым вычисляется интерполированные значения</param>
        /// <param name="h_ar">x координаты интерполируемой функции</param>
        /// <param name="ro_ar">y координаты интерполируемой функции</param>
        /// <param name="algoritm">lagrange, newton, linear (simple)</param>
        /// <returns>интерполируемые значения той же формы, что и h_f</returns>
        public static double Interp(double h_f, List<double> h_ar, List<double> ro_ar, string algoritm)
        {
            return algoritm.ToLower() switch
            {
                "lagrange" => Lagrange(h_f, h_ar.ToArray(), ro_ar.ToArray()),
                "newton" => Newton(h_f, h_ar.ToArray(), ro_ar.ToArray()),
                "linear" => Linear(h_f, h_ar.ToArray(), ro_ar.ToArray()),
                _ => throw new ArgumentException("Метод выбран неверный"),
            };
        }
        private static int LeftBorder(double x, double[] array)
        {
            if (array.Length == 0)
            {
                throw new ArgumentException("Массив должен содержать элементы!");
            }

            for (int i = 0; i < array.Length - 1; ++i)
            {
                if (array[i] <= x && x <= array[i + 1])
                {
                    return i;
                }
            }

            throw new ArgumentException("Неправильно отсортирован массив!");
        }
        private static int RightBorder(double x, double[] array)
        {
            if (array.Length == 0)
            {
                throw new ArgumentException("Массив должен содержать элементы!");
            }

            for (int i = 0; i < array.Length - 1; ++i)
            {
                if (array[i] <= x && x <= array[i + 1])
                {
                    return i + 1;
                }
            }

            throw new ArgumentException("Неправильно отсортирован массив!");
        }
        // Метод для вычисления линейной интерполяции
        private static double Linear(double x, double[] xd, double[] yd)
        {
            int left = LeftBorder(x, xd);
            int right = RightBorder(x, xd);

            return (x - xd[right]) / (xd[left] - xd[right]) * yd[left]
                + (x - xd[left]) / (xd[right] - xd[left]) * yd[right];

        }
        // Фукция для вычисления интерполяции методом лагранджа
        private static double Lagrange(double x, double[] xd, double[] yd)
        {
            if (xd.Length != yd.Length)
            {
                throw new ArgumentException("Разный размер массивов");
            }

            double result = 0.0d;

            for (int i = 0, n = xd.Length; i < n; ++i)
            {
                if (Math.Round(x - xd[i], 9) == 0)
                {
                    return yd[i];
                }
                double product = yd[i];
                for (int j = 0; j < n; ++j)
                {
                    if ((i == j) || (Math.Round(xd[i] - xd[j], 9) == 0))
                    {
                        continue;
                    }
                    product *= (x - xd[i]) / (xd[i] - xd[j]);
                }
                result += product;
            }

            return result;
        }
        // Функция ньютона
        private static double Newton(double x, double[] xd, double[] yd)
        {
            if (xd.Length != yd.Length)
            {
                throw new ArgumentException("Разный размер массивов");
            }

            int n = xd.Length;

            double[,] dy = new double[n, n];


            // подсчитываем dy
            for (int i = 0; i < n; ++i)
            {
                dy[0, i] = yd[i];
            }

            for (int i = 1; i < n; ++i)
            {
                for (int j = 0; j < n - i; ++j)
                {
                    dy[i, j] = dy[i - 1, j + 1] - dy[i - 1, j];
                }
            }

            // вычисляем результирующий y
            double step = 0.05;
            double q = (x - xd[0]) / step; // см. формулу
            double result = yd[0]; // результат (y) 

            double mult_q = 1; // произведение из q*(q-1)*(q-2)*(q-n)
            double fact = 1;  // факториал

            for (int i = 1; i < n; ++i)
            {
                fact *= i;
                mult_q *= (q - i + 1);

                result += mult_q / fact * dy[i, 0];
            }

            return result;
        }


        public static double[,] Zeros(int rows, int cols)
        {
            double[,] result = new double[rows, cols];

            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    result[i, j] = 0.0;
                }
            }
            return result;
        }
        public static double[] Zeros(int cell)
        {
            double[] result = new double[cell];

            for (int i = 0; i < cell; ++i)
            {
                result[i] = 0.0;
            }
            return result;
        }
        /// <summary>
        /// Добавление в конец массива данных из другого массива
        /// </summary>
        /// <param name="a">Исходный массив</param>
        /// <param name="values">Масив из которого необходимо добавить элементы в исходный массив</param>
        /// <param name="axis"></param>
        /// <returns>Массив, содержащий исходный и добавленные элементы</returns>
        public static double[,] Append(double[,] a, double[,] values, int axis)
        {
            int arows = a.GetLength(0);
            int acols = a.GetLength(1);
            int newrows = arows + values.GetLength(0);
            int newcols = values.GetLength(1);

            if (acols != newcols)
            {
                throw new ArgumentException("Количество колонок в массивах должна быть одинакова!");
            }

            double[,] result = new double[newrows, acols];

            switch (axis)
            {
                case 0:
                    // copy a to result
                    for (int i = 0; i < arows; ++i)
                    {
                        for (int j = 0; j < acols; ++j)
                        {
                            result[i, j] = a[i, j];
                        }
                    }
                    // copy values to result
                    for (int i = arows; i < newrows; ++i)
                    {
                        for (int j = 0; j < acols; ++j)
                        {
                            result[i, j] = values[i, j];
                        }
                    }
                    break;
                case 1:
                    throw new ArgumentException("Axis == 1 не реализован!");
                default:
                    throw new ArgumentException("Неправильно выбран параметр axis!");
            }
            return result;
        }
        /// <summary>
        /// Добавление в исходный одномерный массив элемента
        /// </summary>
        /// <param name="a">Исходный массив</param>
        /// <param name="value">Значение, которое необходимо добавить</param>
        /// <returns>Массив, содержащий исходный массив и добавленный элемент</returns>
        public static double[] Append(double[] a, double value)
        {
            double[] result = new double[a.Length + 1];
            // copy to result
            for (int i = 0; i < result.Length - 1; ++i)
            {
                result[i] = a[i];
            }
            // add new value
            result[^1] = value;

            return result;
        }
        /// <summary>
        /// Добавление в исходный одномерный массив элемента
        /// </summary>
        /// <param name="a">Исходный массив</param>
        /// <param name="value">Значение, которое необходимо добавить</param>
        /// <returns>Массив, содержащий исходный массив и добавленный элемент</returns>
        public static double[] Append(double[] a, double[] value)
        {
            double[] result = new double[a.Length + value.Length];
            // copy to result
            for (int i = 0; i < a.Length; ++i)
            {
                result[i] = a[i];
            }
            // copy to result
            for (int i = a.Length; i < value.Length; ++i)
            {
                result[i] = value[i];
            }

            return result;
        }
    }
}
