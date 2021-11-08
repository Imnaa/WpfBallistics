using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;

namespace WpfBallistics.ExcelWorker
{
    // Класс генерации Excel файла
    class ResultExcelGenerator
    {
        public byte[] Generate(ResultReport report)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();

            var sheet = package.Workbook.Worksheets
                .Add(report.MainResultData.IndexExp.ToString());

            // start
            // main data
            sheet.Cells[2, 2].Value = "Эксперимент";
            sheet.Cells[2, 2, 2, 3].Merge = true;
            sheet.Cells[2, 4].Value = report.MainResultData.IndexExp+1;
            sheet.Cells[3, 2].Value = "Заданный угол";
            sheet.Cells[3, 2, 3, 3].Merge = true;
            sheet.Cells[3, 4].Value = report.MainResultData.Angle;
            sheet.Cells[4, 2].Value = "Дальность полета";
            sheet.Cells[4, 2, 4, 3].Merge = true;
            sheet.Cells[4, 4].Value = report.MainResultData.FlyDistance;
            sheet.Cells[5, 2].Value = "Высота максимальная";
            sheet.Cells[5, 2, 5, 3].Merge = true;
            sheet.Cells[5, 4].Value = report.MainResultData.FlyHeightMax;
            sheet.Cells[6, 2].Value = "Время полета";
            sheet.Cells[6, 2, 6, 3].Merge = true;
            sheet.Cells[6, 4].Value = report.MainResultData.FlyTime;
            sheet.Cells[7, 2].Value = "Скорость на активном участке";
            sheet.Cells[7, 2, 7, 3].Merge = true;
            sheet.Cells[7, 4].Value = report.MainResultData.VActMax;
            sheet.Cells[8, 2].Value = "Скорость конечная";
            sheet.Cells[8, 2, 8, 3].Merge = true;
            sheet.Cells[8, 4].Value = report.MainResultData.VEndMax;

            sheet.Cells[2, 5].Value = "Пропуск на акт. участке";
            sheet.Cells[2, 5, 2, 6].Merge = true;
            sheet.Cells[2, 7].Value = report.MainResultData.DeltaAct;
            sheet.Cells[3, 5].Value = "Пропуск на не акт. участке";
            sheet.Cells[3, 5, 3, 6].Merge = true;
            sheet.Cells[3, 7].Value = report.MainResultData.DeltaNotAct;
            sheet.Cells[4, 5].Value = "Пропуск для вывода акт. участка";
            sheet.Cells[4, 5, 4, 6].Merge = true;
            sheet.Cells[4, 7].Value = report.MainResultData.DeltaOutAct;
            sheet.Cells[5, 5].Value = "Пропуск для вывода не акт. участка";
            sheet.Cells[5, 5, 5, 6].Merge = true;
            sheet.Cells[5, 7].Value = report.MainResultData.DeltaOutNotAct;

            // table data
            sheet.Cells[11, 2, 11, 5].LoadFromArrays(new object[][] { new[] { "Время полета", "Дальность полета", "Высота полета", "Скорость текущая" } });
            var row = 12;
            var column = 2;
            foreach (var item in report.ResultDatas)
            {
                sheet.Cells[row, column].Value = item.FlyTime;
                sheet.Cells[row, column + 1].Value = item.FlyDistance;
                sheet.Cells[row, column + 2].Value = item.FlyHeight;
                sheet.Cells[row, column + 3].Value = item.VCurrent;
                ++row;
            }
            // Форматирование ячеек
            // Размеры
            sheet.Cells[1, 1, row, column + 3].AutoFitColumns();
            sheet.Column(1).Width = 4;
            sheet.Column(2).Width = 14;
            sheet.Column(3).Width = 18;
            sheet.Column(4).Width = 18;
            sheet.Column(5).Width = 18;
            sheet.Column(6).Width = 18;
            sheet.Column(7).Width = 8;
            // Стили
            sheet.Column(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            sheet.Cells[11, 2, 11 + report.ResultDatas.Length, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            sheet.Column(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            // Выделение ячеек
            sheet.Cells[11, 2, 11, 5].Style.Font.Bold = true;
            sheet.Cells[2, 2, 8, 3].Style.Font.Bold = true;
            sheet.Cells[2, 5, 5, 5].Style.Font.Bold = true;
            sheet.Cells[11, 2, 11 + report.ResultDatas.Length, 5].Style.Border.BorderAround(ExcelBorderStyle.Double);
            sheet.Cells[11, 2, 11, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Форматироние ячейки для акт уч
            sheet.Cells[12, 1].Value = "Активный участок";
            sheet.Cells[12, 1, report.MainResultData.IndexOfNotActDist + 11, 1].Merge = true;
            sheet.Cells[12, 1, report.MainResultData.IndexOfNotActDist + 11, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            sheet.Cells[12, 1, report.MainResultData.IndexOfNotActDist + 11, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[12, 1, report.MainResultData.IndexOfNotActDist + 11, 1].Style.TextRotation = 90;
            sheet.Cells[12, 1, report.MainResultData.IndexOfNotActDist + 11, 1].Style.WrapText = true;
            // Форматироние ячейки для не акт уч
            sheet.Cells[report.MainResultData.IndexOfNotActDist + 12, 1].Value = "Не активный участок";
            try
            {
                sheet.Cells[report.MainResultData.IndexOfNotActDist + 12, 1, report.ResultDatas.Length + 11, 1].Merge = true;
                sheet.Cells[report.MainResultData.IndexOfNotActDist + 12, 1, report.ResultDatas.Length + 11, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                sheet.Cells[report.MainResultData.IndexOfNotActDist + 12, 1, report.ResultDatas.Length + 11, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[report.MainResultData.IndexOfNotActDist + 12, 1, report.ResultDatas.Length + 11, 1].Style.TextRotation = 90;
                sheet.Cells[report.MainResultData.IndexOfNotActDist + 12, 1, report.ResultDatas.Length + 11, 1].Style.WrapText = true;
            } catch //(Exception ex)
            {
                // Проблема с количеством выводимых данных. Ничего страшного....наверное...
                System.Windows.MessageBox.Show("Необходимо больше точек для вывода графика!\nP.S. но график постараюсь нарисовать :)");
            }
            // chart
            var dataChart = sheet.Drawings.AddChart("FindingsChart", OfficeOpenXml.Drawing.Chart.eChartType.Line);
            dataChart.Title.Text = "Траектория полета";
            dataChart.SetPosition(8, 0, 5, 0);
            dataChart.SetSize(800, 400);
            var allData = (ExcelChartSerie)(dataChart.Series.Add(sheet.Cells[11, 4, 11 + report.ResultDatas.Length, 4], sheet.Cells[11, 3, 11 + report.ResultDatas.Length, 3]));
            allData.Header = "Эксперимент = " + report.MainResultData.IndexExp.ToString();
            // end
            sheet.Protection.IsProtected = true;
            return package.GetAsByteArray();
        }
    }
}
