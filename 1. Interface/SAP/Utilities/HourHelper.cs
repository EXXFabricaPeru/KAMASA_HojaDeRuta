using System;
using System.Web.Management;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public static class HourHelper
    {
        /// <summary>
        /// Validate if input is a valid time
        /// </summary>
        /// <param name="time">a numeric or string in format [HH:mm]</param>
        /// <returns></returns>
        public static bool IsHourInput(string time)
        {
            int timeHour = 0, hour = 0, minutes = 0;
            if (time.Contains(":"))
            {
                string[] split = time.Split(':');
                if (split.Length > 2 || !int.TryParse(split[0], out hour) || !int.TryParse(split[1], out minutes))
                    return false;
            }
            else if (int.TryParse(time, out timeHour))
            {
                hour = timeHour / 100;
                minutes = timeHour % 100;
            }

            return hour >= 0 && hour < 24 && minutes >= 0 && minutes < 60;
        }

        /// <summary>
        /// Parse time input to valid string hour.
        /// </summary>
        /// <param name="time">a numeric or string in format [HH:mm]</param>
        /// <returns>string hour into format in [HH:mm]. ie: 15:30</returns>
        public static string ParseToTime(string time)
        {
            int timeHour;
            if (time.Contains(":"))
                return time;

            if (int.TryParse(time, out timeHour))
                return $"{timeHour / 100:00}:{timeHour % 100:00}";

            return string.Empty;
        }

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