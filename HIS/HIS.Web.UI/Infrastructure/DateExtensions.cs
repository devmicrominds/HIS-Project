using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            DateTime result;  
            result = dt.AddDays(-1 * diff).Date;
            return result;
             
        }

        public static DateTime FromTimeSpan(this DateTime dt, string timespan) {

            var timearray = timespan.Split(':');
            if (timearray.Length > 3)
                throw new Exception("Timespan length error!");

            var hours = Convert.ToInt32(timearray[0]);
            var minutes = Convert.ToInt32(timearray[1]);
            var seconds = Convert.ToInt32(timearray[2]);

            return new DateTime(dt.Year, dt.Month, dt.Day, hours, minutes, seconds);

        }

        public static string GetTimeSpan(this DateTime dt) {

            string hours = dt.Hour.ToString("D2");
            string minutes = dt.Minute.ToString("D2");
            string seconds = dt.Second.ToString("D2");

            return string.Format("{0}:{1}:{2}",hours,minutes,seconds);
        }
    }
}