using System.Collections.Generic;

namespace WpfBallistics
{
    /// <summary>
    /// Класс для работы с поиском в массивах
    /// </summary>
    public static class FindOperations
    {
        /// <summary>
        /// Поиск максимального элемента в 
        /// </summary>
        /// <param name="x">Лист из массива</param>
        /// <param name="index">Индекс эклемента из массива внутри листа по которому поиск будет идти</param>
        /// <returns>Возврат максимального элемента из массива по выбранному индекса</returns>
        public static double FindMax(List<double[]> x, int index)
        {
            double res = x[0][0];

            foreach (double[] d in x)
            {
                if (res < d[index]) res = d[index];
            }

            return res;
        }
        /// <summary>
        /// Поиск максимального по листу
        /// </summary>
        /// <param name="x">Лист чисел</param>
        /// <returns>Возврат максимального числа из листа</returns>
        public static double FindMax(List<double> x)
        {
            double res = x[0];

            foreach (double d in x)
            {
                if (res < d) res = d;
            }

            return res;
        }
    }
}
