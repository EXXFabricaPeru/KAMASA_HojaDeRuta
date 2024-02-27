using System;

namespace GeolocationAPI.Utilities
{
    public class HourHelper
    {
        /// <summary>
        /// Convert string hour to int hour
        /// </summary>
        /// <param name="hour">format in [HH:mm]. ie: 15:30</param>
        /// <returns></returns>
        public static int ParseToHour(string hour)
        {
            var split = hour.Split(':');
            return Convert.ToInt32(split[0] + split[1]);
        }

        public static int ConvertToHourFormat(decimal minutes)
        {
            minutes = Math.Round(minutes, 0);
            if (minutes < 60)
                return Convert.ToInt32(minutes);

            var aHour = (int) minutes / 60;
            var aMinutes = (int) minutes % 60;

            return aHour * 100 + aMinutes;
        }

        public static int SumHour(decimal a, decimal b)
        {
            var aHour = (int) a / 100;
            var aMinute = (int) a % 100;
            var bHour = (int) b / 100;
            var bMinute = (int) b % 100;
            aMinute += bMinute;
            aHour += bHour;

            if (aMinute < 60)
                return Convert.ToInt32(aHour * 100 + aMinute);

            aHour++;
            aMinute -= 60;
            return Convert.ToInt32(aHour * 100 + aMinute);
        }

        public static int SubtractHour(decimal a, decimal b)
        {
            var aHour = (int) a / 100;
            var aMinute = (int) a % 100;
            var bHour = (int) b / 100;
            var bMinute = (int) b % 100;

            aMinute -= bMinute;
            aHour -= bHour;

            if (aMinute >= 0)
                return Convert.ToInt32(aHour * 100 + aMinute);

            aMinute += 60;
            aHour--;
            return Convert.ToInt32(aHour * 100 + aMinute);
        }
    }
}