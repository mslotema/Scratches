using System;

// ReSharper disable once CheckNamespace
namespace Monetair.Schedules
{
    public class TimeTable
    {
        public TimeSpan DayPhase1 { get; set; }

        public TimeSpan DayPhase2 { get; set; }

        public TimeSpan EndPhase1 { get; set; }

        public TimeSpan EndPhase2 { get; set; }

        public TimeSpan NightPhase { get; set; }
    }
}