using System;

namespace Monetair.Schedules.Internal
{
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Returns true when <param name="moment"/> is between <param name="earlier"/>
        /// and (not including) <param name="later"></param>.
        /// <para>
        /// It works across midnight, <param name="later"></param> is assumed to be later
        /// in date and time (e.g. the next morning)
        /// </para>
        /// </summary>
        public static bool Between(this TimeSpan moment, TimeSpan earlier, TimeSpan later)
        {
            if (earlier < later)
            {
                // same day
                return moment >= earlier && moment < later;
            }
            else
            {
                // across midnight (two days) (e.g. earlier=22:00, later=07:00)
                if (moment < later)
                    // moment after midnight (e.g. moment=01:00, later=07:00)
                    return later >= moment && moment < earlier;
                else
                    // moment before midnight (e.g. moment=23:30, later=07:00)
                    return moment > later && earlier <= moment;
            }
        }

        public static TimeSpan ToTime(this string timeString)
        {
            if (TimeSpan.TryParse(timeString, out var timeSpan))
            {
                return timeSpan;
            }

            throw new FormatException($"value cannot be converted to a TimeSpan");
        }
    }
}