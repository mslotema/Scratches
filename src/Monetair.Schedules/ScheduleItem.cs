using System;

namespace Monetair.Schedules
{
    internal class ScheduleItem<TPhase> where TPhase : Enum
    {
        public TPhase Phase { get; private set; }

        /// <summary>
        /// Immutable default start time (for reset)
        /// </summary>
        public TimeSpan OriginalStartTime { get; private set; }

        /// <summary>
        /// Effective start time
        /// </summary>
        public TimeSpan StartTime { get; private set; }

        public ScheduleItem<TPhase> Next { get; private set; }

        public bool First { get; private set; }

        private ScheduleItem()
        {
        }

        internal void SetStartTime(TimeSpan newTime)
        {
            StartTime = newTime;
        }

        internal void ResetStartTime()
        {
            StartTime = OriginalStartTime;
        }

        internal static ScheduleItem<TPhase> Create(TPhase phase, TimeSpan startTime, bool first = false)
        {
            return new ScheduleItem<TPhase>
            {
                Phase = phase,
                OriginalStartTime = startTime,
                StartTime = startTime,
                First = first
            };
        }

        internal void SetNext(ScheduleItem<TPhase> next)
        {
            Next = next;
        }
    }
}