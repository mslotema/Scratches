using System;

namespace Monetair.Schedules
{
    public enum TradingDayPhase
    {
        Day1 = 0,

        Day2 = 1,

        End1 = 2,

        End2 = 3,

        Night = 4,
    }

    public class DayPhase
    {
        public TradingDayPhase Phase { get; }

        protected readonly TimeSpan defaultStartTime;

        public TimeSpan StartTime { get; }

        /// <summary>
        /// This indicates the first, start, phase.
        /// <para>
        /// The start time cannot be changed
        /// </para>
        /// </summary>
        public bool Fixed => Phase == TradingDayPhase.Day1;

        public DayPhase(TradingDayPhase phase, TimeSpan startTime)
        {
            Phase = phase;
            defaultStartTime = StartTime = startTime;
        }
    }

    // /// <summary>
    // /// Abstract trading day phase
    // /// <para>
    // /// The phases of a trading day are deliberatly ordered, and the first one is fixed
    // /// in time through configuration. It signals the start of the day, and will reset the schecule if necessary.
    // /// </para>
    // /// </summary>
    // public abstract class DayPhase
    // {
    //     private TimeSpan startTime;
    //
    //     public virtual string Name => GetType().Name;
    //
    //
    //     protected abstract int Order { get; }
    //
    //     /// <summary>
    //     /// The start time of this <see cref="DayPhase"/> cannot be changed.
    //     /// <para>
    //     /// It is the start of a new trading day
    //     /// </para>
    //     /// </summary>
    //     protected virtual bool Fixed => Order == 1;
    //
    //     public virtual TimeSpan StartTime
    //     {
    //         get => startTime;
    //         set
    //         {
    //             if (Fixed)
    //             {
    //                 throw new TradingDayScheduleException();
    //             }
    //
    //             startTime = value;
    //         }
    //     }
    // }
    //
    // public class DayPhase1 : DayPhase
    // {
    //     protected override int Order => 1;
    // }
    //
    // public class DayPhase2 : DayPhase
    // {
    //     protected override int Order => 2;
    // }
    //
    // public class EndPhase1 : DayPhase
    // {
    //     protected override int Order => 3;
    // }
    //
    // public class EndPhase2 : DayPhase
    // {
    //     protected override int Order => 4;
    // }
    //
    // public class NightPhase : DayPhase
    // {
    //     protected override int Order => 5;
    // }
}