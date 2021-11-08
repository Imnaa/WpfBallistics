using System;
using System.Collections.Generic;

namespace WpfBallistics.ExcelWorker
{
    // Запись данных в класс для хранения
    class ResultReporter
    {
        public ResultReport GetReport(List<string>[] datas, Dictionary<string, double> calcParams, int i)
        {
            ResultReport result = new ResultReport()
            {
                MainResultData = new MainResultData()
                {
                    IndexExp = i,
                    Angle = Convert.ToDouble(datas[0][0].Split("= ")[1].Replace(".", ",")),
                    FlyTime = Convert.ToDouble(datas[0][3].Split("= ")[1].Replace(".", ",")),
                    FlyHeightMax = Convert.ToDouble(datas[0][2].Split("= ")[1].Replace(".", ",")),
                    FlyDistance = Convert.ToDouble(datas[0][1].Split("= ")[1].Replace(".", ",")),
                    VActMax = Convert.ToDouble(datas[0][4].Split("= ")[1].Replace(".", ",")),
                    VEndMax = Convert.ToDouble(datas[0][5].Split("= ")[1].Replace(".", ",")),

                    DeltaAct = calcParams["tbDeltaAct"],
                    DeltaNotAct = calcParams["tbDeltaNotAct"],
                    DeltaOutAct = calcParams["tbDeltaOutAct"],
                    DeltaOutNotAct = calcParams["tbDeltaOutNotAct"]
                }
            };

            result.ResultDatas = new ResultItem[datas[1].Count - 2];

            for (int j = 0, s = 0; j < result.ResultDatas.Length + 1; ++j)
            {
                if (!datas[1][j + 1].Contains("НЕ АКТИВНЫЙ УЧАСТОК"))
                {
                    result.ResultDatas[j - s] = new ResultItem() 
                    {
                        FlyTime = Convert.ToDouble(datas[3][j + 1].Replace(".", ",")),
                        FlyHeight = Convert.ToDouble(datas[2][j + 1].Replace(".", ",")),
                        FlyDistance = Convert.ToDouble(datas[1][j + 1].Replace(".", ",")),
                        VCurrent = Convert.ToDouble(datas[4][j + 1].Replace(".", ","))
                    };
                }
                else
                {
                    result.MainResultData.IndexOfNotActDist = j;
                    ++s;
                }
            }


            return result;
        }
    }
}