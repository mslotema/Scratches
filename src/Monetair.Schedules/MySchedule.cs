using System;

namespace Monetair.Schedules
{
    public class MySchedule
    {
        
    }

    public class ScheduleItem
    {
        public DayPhases DayPhase { get; }
        public TimeSpan DefaultStartTime { get; }
        public TimeSpan StartTime { get; protected set; }
        
        public ScheduleItem(DayPhases dayPhase, TimeSpan startTime)
        {
            DayPhase = dayPhase;
            DefaultStartTime = StartTime = startTime;
        }
    }
    public enum DayPhases
    {
        Day1 = 0,

        Day2 = 1,

        End1 = 2,

        End2 = 3,

        Night = 4,
    }
}