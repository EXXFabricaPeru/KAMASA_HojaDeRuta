using GeolocationAPI.Utilities;

namespace GeolocationAPI.Models
{
    public class TransferOrderIndicator
    {
        public int Id { get; set; }
        public string TimeWindowId { get; set; }
        public int TimeWindow { get; set; }
        public int StartTime { get; set; }
        public int TravelTime { get; set; }
        public int ArriveTime => HourHelper.SumHour(StartTime, TravelTime);
        public int ArriveTime2 { get; set; }
        public int DispatchTime { get; set; }
        public int LeaveTime => HourHelper.SumHour(ArriveTime, DispatchTime);
        public int LeaveTime2 => HourHelper.SumHour(ArriveTime2, DispatchTime);
        public int OptimizationTime { get; set; }
        public decimal Weight { get; set; }
        public decimal Volume { get; set; }
        public decimal FreezeWeight { get; set; }
        public decimal FreezeVolume { get; set; }
        public decimal TotalIndicatorValue { get; set; }
        public decimal Distance { get; set; }
        public string DeliveryRestriction { get; set; }
        public bool IsPossibleToDispatch { get; set; }
    }
}