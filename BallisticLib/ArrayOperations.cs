using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallisticLib
{
    /// <summary>
    /// Класс для выполнения операций над массивами
    /// </summary>
    static class ArrayOperations
    {
        /// <summary>
        /// Метод для перемножения элементов массива на число
        /// </summary>
        /// <param name="a">Исходный массив</param>
        /// <param name="val">Число на которое умножаем</param>
        /// <returns>Перемноженный массив</returns>
        public static double[] MultiplyToCells(double[] a, double val)
        {
            return a.Select(x => x * val).ToArray();
        }
        /// <summary>
        /// Метод для сложения элементов массива на число
        /// </summary>
        /// <param name="a">Исходный массив</param>
        /// <param name="val">Число на которое складываем</param>
        /// <returns>Результационный массив</returns>
        public static double[] AddToCells(double[] a, double val)
        {
            return a.Select(x => x + val).ToArray();
        }
        /// <summary>
        /// Метод для деления элементов массива на число
        /// </summary>
        /// <param name="a">Исходный массив</param>
        /// <param name="val">Число на которое делим</param>
        /// <returns>Результационный массив</returns>
        public static double[] DivToCells(double[] a, double val)
        {
            return a.Select(x => x / val).ToArray();
        }
        /// <summary>
        /// Метод для вычитания элементов массива на число
        /// </summary>
        /// <param name="a">Исходный массив</param>
        /// <param name="val">Число на которое вычитаем</param>
        /// <returns>Результационный массив</returns>
        public static double[] SubToCells(double[] a, double val)
        {
            return a.Select(x => x - val).ToArray();
        }
        /// <summary>
        /// Метод для сложения элементов массива на элементы другого массива
        /// </summary>
        /// <param name="a">Исходный массив</param>
        /// <param name="b">Второй массив который складываем</param>
        /// <returns>Массив из сложенных элементов двух массивов</returns>
        public static double[] AddToCells(double[] a, double[] b)
        {
            if (a.Length != b.Length)
            {
                throw new ArgumentException("Размерность массивов должна быть одинаковая!");
            }
            double[] c = new double[a.Length];
            for (int i = 0; i < c.Length; ++i)
            {
                c[i] = a[i] + b[i];
            }
            return c;
        }
        /// <summary>
        /// Метод для сложения элементов двухмерного массива на элементы другого двухмерного массива
        /// </summary>
        /// <param name="a">Исходный двухмерный массив</param>
        /// <param name="b">Второй двухмерный массив который складываем</param>
        /// <returns>Двухмерный массив из сложенных элементов двух массивов</returns>
        public static double[,] AddToCells(double[,] a, double[,] b)
        {
            int arows = a.GetLength(0);
            int acols = a.GetLength(1);
            int brows = b.GetLength(0);
            int bcols = b.GetLength(1);

            if (arows != brows && acols != bcols)
            {
                throw new ArgumentException("Размерность массивов должна быть одинаковая!");
            }

            double[,] c = new double[arows, acols];
            for (int i = 0; i < arows; ++i)
            {
                for (int j = 0; i < acols; ++j)
                {
                    c[i, j] = a[i, j] + b[i, j];
                }
            }
            return c;
        }
        // !!!
        //private static float[] ToFloat(IEnumerable<double> array)
        //{
        //    return array.Select(x => (float)x).ToArray();
        //}
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
