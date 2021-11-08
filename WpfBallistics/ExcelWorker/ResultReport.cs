namespace WpfBallistics.ExcelWorker
{
    // Класс для хранения всей информации
    class ResultReport
    {
        public MainResultData MainResultData { set; get; }
        public ResultItem[] ResultDatas { set; get; }
    }
    // main result data
    public class MainResultData
    {
        public int IndexExp { get; set; }
        public int IndexOfNotActDist { get; set; }
        public double Angle { get; set; }
        public double FlyTime { get; set; }
        public double FlyHeightMax { get; set; }
        public double FlyDistance { get; set; }
        public double VActMax { get; set; }
        public double VEndMax { get; set; }
        public double DeltaAct { get; set; }
        public double DeltaNotAct { get; set; }
        public double DeltaOutAct { get; set; }
        public double DeltaOutNotAct { get; set; }
    }
    // table
    public class ResultItem
    {
        public double FlyTime { get; set; }
        public double FlyHeight { get; set; }
        public double FlyDistance { get; set; }
        public double VCurrent { get; set; }

    }
    // second result data
    public class SecondResultData
    {
        public int IndexExp { get; set; }
        public string Angle { get; set; }
        public string FlyTime { get; set; }
        public string FlyHeightMax { get; set; }
        public string FlyDistance { get; set; }
        public string VActMax { get; set; }
        public string VEndMax { get; set; }
    }
}
