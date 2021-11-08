using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallisticLibCore
{
    class MathModel
    {
        private readonly double deg = 180.0 / Math.PI; // Перевод радианов в градусы
        private readonly double f_tr = 0.12; // Коэффициент трения
        private readonly double g = 9.81; // Ускорение свободного падения
        // Поля класса необходимые для вычисления
        private double[] x_0_f;
        private double dt_f;
        private double tk_f;
        private double alfa_0_f;
        private double M_top_f;
        private double M_orch_f;
        private double M_gch_f;
        private double P_sr_f;
        private double t_aug_f;
        private double F_stop_f;
        private double d_f;
        private double i_f;
        private double l_nap_f;
        private int deltaAct;
        private int deltaNotAct;
        // Здесь содердится массив всех выходных данных мат.модели
        public List<double[]> Answer = null;
        // Скорость ракеты с учетом всех сил
        private readonly List<double> V = new List<double>();
        // Получить скорость
        public List<double> GetV()
        {
            return V;
        }
        // Конструктор
        public MathModel()
        {

        }
        // Функция тяги
        private double Peng(double P_sr_f, double t_f, double t_aug_f)
        {
            double t_akt = 0.02 * t_aug_f;

            if (t_f < t_akt)
            {
                return P_sr_f * t_f / t_akt;
            }
            else if (t_f < t_aug_f)
            {
                return P_sr_f;
            }
            else
            {
                return 0;
            }
        }
        // Функция ищменения массы РС
        private double Mtop(double M_top_f, double t_aug_f, double t_f)
        {
            if (Math.Round(t_f, 3) <= Math.Round(t_aug_f, 3))
            {
                return M_top_f * (1 - ((t_f * t_f) / (t_aug_f * t_aug_f)));
            }
            else
            {
                return 0;
            }
        }
        // Функция вычисления площади поперечного сечения ЛА в м*м
        private double Sf(double d_f)
        {
            return (Math.PI * (d_f * d_f)) / 4000000;
        }
        // Функция учета стопорящих устройств
        private double Xstop(double P_f, double F_stop_f, double x_f)
        {
            if (P_f > F_stop_f || Math.Round(x_f, 3) > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        // Функция преодоления сил сопротивления на начальном участке (тяжесть, трение)
        private double Xstart(double P_act_f, double P_pas_f, double x)
        {
            if (P_act_f > P_pas_f || Math.Round(x, 3) > 0)
            {

                return 1;
            }
            else
            {

                return 0;
            }
        }
        // Функция, учитывающая движение по направляющим
        private double Xnap(double x_r_f, double l_nap_f)
        {
            if (Math.Round(x_r_f, 3) < Math.Round(l_nap_f, 3))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        // # Функция, учитывающая возникновение трения при движении
        private double Xtr(double v_f)
        {
            if (Math.Round(v_f, 3) != 0.0f)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        // Функция определения текущего знака переменной
        private double Sign(double x)
        {
            if (x < 0)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        //
        // Блок функций аэродинамического воздействия
        //
        // Введение массива высот
        private readonly List<double> h_ar = (List<double>)NumpyMy.LinSpace(0, 10000, 101);
        // Введение массива значений числа Маха
        private readonly List<double> Mah_ar = (List<double>)NumpyMy.LinSpace(0, 5, 51);
        // Введение массива значений скорости звука согласно ГОСТ "Стандартная атмосфера"
        private readonly List<double> a_ar = (List<double>)NumpyMy.Array(
                340.294, 339.910, 339.526, 339.141, 338.755, 338.370, 337.983,
                337.597, 337.210, 336.822, 336.435, 336.046, 335.658, 335.268,
                334.879, 334.489, 334.098, 333.707, 333.316, 332.924, 332.532,
                332.139, 331.746, 331.352, 330.958, 330.563, 330.168, 329.773,
                329.377, 328.980, 328.584, 328.186, 327.788, 327.390, 326.991,
                326.592, 326.192, 325.792, 325.392, 324.990, 324.589, 324.187,
                323.784, 323.381, 322.977, 322.573, 322.169, 321.764, 321.358,
                320.952, 320.545, 320.138, 319.731, 319.323, 318.914, 318.505,
                318.095, 317.685, 317.275, 316.863, 316.452, 316.040, 315.627,
                315.213, 314.800, 314.385, 313.971, 313.555, 313.139, 312.723,
                312.306, 311.888, 311.470, 311.051, 310.632, 310.212, 309.792,
                309.371, 308.950, 308.528, 308.105, 307.682, 307.258, 306.834,
                306.409, 305.984, 305.558, 305.131, 304.704, 304.276, 303.848,
                303.419, 302.990, 302.559, 302.129, 301.697, 301.265, 300.833,
                300.400, 299.966, 299.532
            );
        // Введение массива значений плотности воздуха согласно ГОСТ "Стандартная атмосфера"
        private readonly List<double> ro_ar = (List<double>)NumpyMy.Array(
                1.225000, 1.213280, 1.201650, 1.190110, 1.178650, 1.167270,
                1.155980, 1.141780, 1.133660, 1.122610, 1.111660, 1.100790,
                1.089990, 1.079280, 1.068650, 1.058100, 1.047640, 1.037250,
                1.026940, 1.016710, 1.006550, 0.996470, 0.986480, 0.976560,
                0.966721, 0.956954, 0.947264, 0.937649, 0.928110, 0.918645,
                0.909254, 0.899938, 0.890694, 0.881524, 0.872427, 0.863402,
                0.854449, 0.845567, 0.836756, 0.828016, 0.819347, 0.810747,
                0.802216, 0.793755, 0.785363, 0.777038, 0.768782, 0.760593,
                0.752472, 0.744417, 0.736429, 0.728506, 0.720649, 0.712858,
                0.705131, 0.697469, 0.689871, 0.682336, 0.674865, 0.667457,
                0.660111, 0.652828, 0.645607, 0.638447, 0.631348, 0.624310,
                0.617332, 0.610415, 0.603557, 0.596758, 0.590018, 0.583337,
                0.576715, 0.570150, 0.563642, 0.557192, 0.550798, 0.544162,
                0.538181, 0.531956, 0.525786, 0.519571, 0.513612, 0.507606,
                0.501655, 0.495757, 0.489913, 0.484122, 0.478883, 0.472697,
                0.467063, 0.461481, 0.455949, 0.450469, 0.445040, 0.439661,
                0.434332, 0.429053, 0.423823, 0.418642, 0.413510
            );
        // Введение массива значений коэффициента лобового сопротивления согласно графику
        private readonly List<double> cx43_ar = (List<double>)NumpyMy.Array(
                0.1814, 0.1818, 0.1818, 0.1818, 0.1818, 0.1820, 0.1837, 0.1846,
                0.1879, 0.2072, 0.3285, 0.3706, 0.3786, 0.3730, 0.3663, 0.3566,
                0.3477, 0.3379, 0.3271, 0.3192, 0.3134, 0.3066, 0.2993, 0.2932,
                0.2883, 0.2842, 0.2801, 0.2760, 0.2719, 0.2683, 0.2658, 0.2636,
                0.2617, 0.2598, 0.2582, 0.2570, 0.2565, 0.2568, 0.2574, 0.2581,
                0.2583, 0.2581, 0.2575, 0.2569, 0.2565, 0.2564, 0.2565, 0.2567,
                0.2569, 0.2569, 0.2568
            );
        // Метод интерполяции зависимости скорости звука от высоты
        private double A_f(double h_f)
        {
            if (h_f < 0)
            {
                //return a_ar[0];
                return 0.0f;
            }
            else if (h_f < 10000)
            {
                double res;
                //res = np.interp(h_f, h_ar, a_ar, "lagrange");
                //res = np.interp(h_f, h_ar, a_ar, "newton");
                res = NumpyMy.Interp(h_f, h_ar, a_ar, "linear");
                return res;
            }
            else
            {
                return a_ar.Last();
            }
        }
        // Метод интерполяции зависимости плотности воздуха от высоты
        private double Ro_f(double h_f)
        {
            if (h_f < 0)
            {
                return ro_ar[0];
            }
            else if (h_f < 10000)
            {
                double res;
                //res = np.interp(h_f, h_ar, ro_ar, "lagrange");
                //res = np.interp(h_f, h_ar, ro_ar, "newton");
                res = NumpyMy.Interp(h_f, h_ar, ro_ar, "linear");
                return res;
            }
            else
            {
                return a_ar[ro_ar.Count - 1];
            }
        }
        // Метод интерполяции зависимости значения коэффициента лобового сопротивления от числа Маха
        private double Cx43_f(double M_f)
        {
            if (M_f < 0)
            {
                return cx43_ar[0];
            }
            else if (M_f < 5)
            {
                double res;
                //res = np.interp(M_f, Mah_ar, cx43_ar, "lagrange");
                //res = np.interp(M_f, Mah_ar, cx43_ar, "Newton");
                res = NumpyMy.Interp(M_f, Mah_ar, cx43_ar, "linear");
                return res;
            }
            else
            {
                return a_ar[cx43_ar.Count - 1];
            }
        }
        // Решение СОДУ
        private double[] SODU(double[] x, double t_f, double alfa_f, double M_top_f, double M_orch_f, double M_gch_f, double P_sr_f,
        double t_aug_f, double F_stop_f, double d_f, double i_f, double l_nap_f)
        {
            // Формирование массива уравнений СОДУ
            double[] solve = NumpyMy.Zeros(4);
            // Определение текущей массы топлива
            double M_top_tek = Mtop(M_top_f, t_aug_f, t_f);
            // Определение текущей массы ракеты
            double m_tek = M_orch_f + M_gch_f + M_top_tek;
            // Определение текущего значения силы тяги
            double P_tek = Peng(P_sr_f, t_f, t_aug_f);
            // Сравнение силы тяги с усилием стопорения
            double x_stop_tek = Xstop(P_tek, F_stop_f, x[2]);
            // Определение текущих абсолютного перемещения и скорости
            double l_tek = Math.Sqrt(x[2] * x[2] + x[3] * x[3]);
            double v_tek = Math.Sqrt(x[0] * x[0] + x[1] * x[1]);
            
            // Определение текущего значения скорости звука
            double a_tek = A_f(x[3]);

            // Определение текущего значения числа Маха
            double M_tek = v_tek / a_tek;

            // Определение значения коэффициента лобового сопротивления
            double Cx43_tek = Cx43_f(M_tek);

            // Определение скоростного напора
            double q_tek = (Ro_f(x[3]) * v_tek * v_tek) / 2;

            // Определение площади поперечного сечения
            double S_tek = Sf(d_f);

            // Определение аэродинамических сил
            double X_tek = Cx43_tek * i_f * q_tek * S_tek;

            // Определение условия нахождения реактивного снаряда в направляющей
            double x_nap_tek = Xnap(l_tek, l_nap_f);

            // Введение поправки на наличие трения
            double x_tr_tek = Xtr(v_tek);

            // Определение проекций действующих активных сил и сил сопротивления
            double P_x_act_tek = P_tek * Math.Cos(alfa_f);
            double P_y_act_tek = P_tek * Math.Sin(alfa_f);
            double P_x_pas_tek = m_tek * g * Math.Cos(alfa_f) * Math.Sin(alfa_f) * x_nap_tek + f_tr * m_tek * g * Math.Cos(alfa_f) * x_tr_tek * Math.Cos(alfa_f) * x_nap_tek + X_tek * (1 - x_nap_tek) * Math.Cos(alfa_f) * Sign(x[0]);
            double P_y_pas_tek = m_tek * g + f_tr * m_tek * g * Math.Sin(alfa_f) * Math.Cos(alfa_f) * x_nap_tek * x_tr_tek - m_tek * g * Math.Cos(alfa_f) * Math.Cos(alfa_f) * x_nap_tek + X_tek * (1 - x_nap_tek) * Math.Sin(alfa_f) * Sign(x[1]);

            // Сравнение сил активных и пассивных
            double x_start_x_tek = Xstart(P_x_act_tek, P_x_pas_tek, x[2]);
            double x_start_y_tek = Xstart(P_y_act_tek, P_y_pas_tek, x[2]);

            // Система дифференциальных уравнений полета
            solve[0] = ((P_x_act_tek - P_x_pas_tek) / m_tek) * x_stop_tek * x_start_x_tek;
            solve[1] = ((P_y_act_tek - P_y_pas_tek) / m_tek) * x_stop_tek * x_start_y_tek;
            solve[2] = x[0];
            solve[3] = x[1];

            return solve;
        }
        // Метод вычисления рунге-кутта 4 порядка
        private List<double[]> Rk4(double[] x_0_f, double dt_f, double tk_f, double alfa_0_f, double M_top_f, double M_orch_f,
        double M_gch_f, double P_sr_f, double t_aug_f, double F_stop_f, double d_f, double i_f, double l_nap_f)
        {
            // Задание массива ответа
            List<double[]> Answear = new List<double[]>()
            {
                new double[4] { 0.0, 0.0, 0.0, 0.0 }
            };
            // Задание массива предварительных вычислений
            double[] PreCal = NumpyMy.Zeros(4);
            // Создание переменной под значение угла
            List<double> Al = new List<double>();
            // Задание начальных условий
            //Answear[0] = x_0_f;
            for (int i = 0; i < x_0_f.Length; ++i)
            {
                Answear[0][i] = x_0_f[i];
            }
            // Задание начального угла
            Al.Add(alfa_0_f / deg);
            // Определение количества шагов
            int n = (int)(tk_f / dt_f);
            bool isAct = true;
            // Математические вычисления метода
            for (int i = 0; i < n; )
            {
                V.Add(Math.Sqrt(Answear[i][0] * Answear[i][0] + Answear[i][1] * Answear[i][1]));

                double[] k1 = SODU(Answear[i], dt_f * (i + 1), Al[i], M_top_f, M_orch_f, M_gch_f, P_sr_f,
                        t_aug_f, F_stop_f, d_f, i_f, l_nap_f);

                // (dt_f * k1 / 2)
                double[] temp = ArrayOperations.DivToCells(ArrayOperations.MultiplyToCells(k1, dt_f), 2);
                double[] tempX = ArrayOperations.AddToCells(Answear[i], temp);

                double[] k2 = SODU(tempX, dt_f * (i + 1) + (dt_f / 2), Al[i], M_top_f,
                        M_orch_f, M_gch_f, P_sr_f, t_aug_f, F_stop_f, d_f, i_f, l_nap_f);

                // (dt_f * k2 / 2)
                temp = ArrayOperations.DivToCells(ArrayOperations.MultiplyToCells(k2, dt_f), 2);

                double[] k3 = SODU(ArrayOperations.AddToCells(Answear[i], temp), dt_f * (i + 1) + (dt_f / 2), Al[i], M_top_f,
                        M_orch_f, M_gch_f, P_sr_f, t_aug_f, F_stop_f, d_f, i_f, l_nap_f);

                // (dt_f * k3)
                temp = ArrayOperations.MultiplyToCells(k3, dt_f);

                double[] k4 = SODU(ArrayOperations.AddToCells(Answear[i], temp), dt_f * (i + 1) + dt_f, Al[i], M_top_f, M_orch_f,
                        M_gch_f, P_sr_f, t_aug_f, F_stop_f, d_f, i_f, l_nap_f);

                // (2 * k2)
                double[] temp0 = ArrayOperations.MultiplyToCells(k2, 2);
                // (2 * k3)
                double[] temp1 = ArrayOperations.MultiplyToCells(k3, 2);
                // (k1 + temp0)
                double[] temp2 = ArrayOperations.AddToCells(k1, temp0);
                // (temp2 + temp1)
                double[] temp3 = ArrayOperations.AddToCells(temp2, temp1);
                // (temp3 + k4)
                double[] temp4 = ArrayOperations.AddToCells(temp3, k4);
                // res = (dt_f/6)*(k1+2*k2+2*k3+k4)
                double[] dy = ArrayOperations.MultiplyToCells(temp4, dt_f / 6);

                PreCal[0] = Answear[i][0] + dy[0];
                PreCal[1] = Answear[i][1] + dy[1];
                PreCal[2] = Answear[i][2] + dy[2];
                PreCal[3] = Answear[i][3] + dy[3];

                // Условие окончания вычислений (столкновение с землей)
                if (Convert.ToDouble(PreCal[2].ToString("N9")) > 0.0f && Convert.ToDouble(PreCal[3].ToString("N9")) <= 0.0f)
                {
                    break;
                }

                // Определение текущего угла
                double Alf;

                if (Math.Round(dy[2], 9) != 0 && Math.Round(dy[3], 9) != 0)
                {
                    Alf = Math.Atan(dy[3] / dy[2]);
                }
                else
                {
                    Alf = Al[i];
                }
                // Присоединение полученных результатов вычислений на данном шаге к массиву результатов
                Answear.Add(new double[4] { PreCal[0], PreCal[1], PreCal[2], PreCal[3] });
                //Answear = np.append(Answear, PreCal);
                Al.Add(Alf);

                if (isAct)
                {
                    if (i - deltaAct > 0)
                    {
                        isAct = (V[i] > V[i - deltaAct]);
                    }
                    i += deltaAct;
                }
                else
                {
                    i += deltaNotAct;
                }
            }
            return Answear;
        }
        // Вычисление результата 
        // p.s. до этого была многопоточность, но прироста в скорости не было и я убрал. Метод оставил в память
        private void Calculate()
        {
            Answer = Rk4(x_0_f, dt_f, tk_f, alfa_0_f, M_top_f,
                M_orch_f, M_gch_f, P_sr_f, t_aug_f, F_stop_f,
                d_f, i_f, l_nap_f);
        }
        // Стортум вычислять (здесь проверки проходят)
        public bool Start(Ballistic ballistic, CalcParams calcParams, double angle)
        {
            if (ballistic.CheckValues() == false)
            {
                return false;
            }

            // Start param
            x_0_f = NumpyMy.Zeros(4);
            // Ballistic params
            M_top_f = Convert.ToDouble(ballistic.FuelMass);
            M_orch_f = Convert.ToDouble(ballistic.MassPocketPath);
            M_gch_f = Convert.ToDouble(ballistic.MassHeadPath);
            d_f = Convert.ToDouble(ballistic.Calibr);
            P_sr_f = Convert.ToDouble(ballistic.AvgValFt);
            t_aug_f = Convert.ToDouble(ballistic.TimeFuelFire);
            l_nap_f = Convert.ToDouble(ballistic.DlinaNapravl);
            F_stop_f = Convert.ToDouble(ballistic.UsilieStoporen);
            i_f = Convert.ToDouble(ballistic.KoeffForm);
            // Calc params
            dt_f = Convert.ToDouble(calcParams.StepIntegr);
            tk_f = Convert.ToDouble(calcParams.TimeCalc);
            deltaAct = Convert.ToInt32(calcParams.DeltaAct);
            deltaNotAct = Convert.ToInt32(calcParams.DeltaNotAct);

            alfa_0_f = angle % 360;

            Calculate();
            return true;
        }

    }
}
