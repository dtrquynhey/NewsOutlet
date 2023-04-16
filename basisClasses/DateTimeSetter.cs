using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NewsOutlet.basisClasses
{
    internal class DateTimeSetter
    {
        //public static DateTime CurrentTime { get; set; } = DateTime.Now;

        //public static long convertToUnixEpoch(DateTime dateTimeToBeConverted)
        //{
        //    TimeSpan timeSpan = dateTimeToBeConverted - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //    return (long)timeSpan.TotalSeconds;
        //}

        public static void SetTime(DateTime dateTimeToSet)
        {
            [DllImport("kernel32.dll")]
            static extern bool SetSystemTime(ref SYSTEMTIME st);

            SYSTEMTIME st = new SYSTEMTIME();
            st.wYear = (short)dateTimeToSet.Year;
            st.wMonth = (short)dateTimeToSet.Month;
            st.wDay = (short)dateTimeToSet.Day;
            st.wHour = (short)dateTimeToSet.Hour;
            st.wMinute = (short)dateTimeToSet.Minute;
            st.wSecond = (short)dateTimeToSet.Second;
            SetSystemTime(ref st);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;
        }
    }
}
