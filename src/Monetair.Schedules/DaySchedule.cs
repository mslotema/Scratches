using System.Reflection.Emit;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Monetair.Schedules;
using Monetair.Schedules.Internal;

namespace Monetair.Schedules
{
    /// <summary>
    /// A day schedule, comprising of 24 hours.
    /// <para>
    /// The last phase will end when the first phase starts.
    /// All day phases are in the order of the enum, and no overlap can exist.
    /// </para>
    /// </summary>
    /// <typeparam name="TPhase"></typeparam>
    public class DaySchedule<TPhase> where TPhase : Enum
    {
        private readonly ICollection<ScheduleItem<TPhase>> dayPhases = new List<ScheduleItem<TPhase>>();

        /// <summary>
        /// 
        /// </summary>
        public DaySchedule(TimeTable<TPhase> timeTable)
        {
            CreateSchedule(timeTable);
        }

        /// <summary>
        /// Get the current day phase, given a moment in time
        /// </summary>
        public TPhase Current(TimeSpan moment)
        {
            return dayPhases
                .Where(self => moment.Between(self.StartTime, self.Next.StartTime))
                .Select(self => self.Phase)
                .Single();
        }

        /// <summary>
        /// Get the (effective) start time for a specific day phase
        /// <para>Can be used for job scheduling</para>
        /// </summary>
        public TimeSpan StartTime(TPhase phase)
        {
            return dayPhases
                .First(self => self.Phase.Equals(phase))
                .StartTime;
        }

        /// <summary>
        /// Get the original start time for a specific day phase, as input at the creation of the schedule.
        /// <para>This is the value after a <see cref="ResetSchedule"/></para>
        /// </summary>
        public TimeSpan OriginalStartTime(TPhase phase)
        {
            return dayPhases
                .First(self => self.Phase.Equals(phase))
                .OriginalStartTime;
        }

        /// <summary>
        /// Reschedule all day phases, except the first one (that is the start of day baseline)
        /// </summary>
        public void Reschedule(TimeTable<TPhase> timeTable)
        {
            CheckTimeTable(timeTable, true);
            foreach (var dayPhase in dayPhases)
                if (timeTable.StartTimes.TryGetValue(dayPhase.Phase, out var time))
                {
                    if (dayPhase.First) throw new InvalidOperationException($"start of day cannot be rescheduled");
                    dayPhase.SetStartTime(time);
                }
        }

        /// <summary>
        /// Revert all start times back to original
        /// </summary>
        public void ResetSchedule()
        {
            foreach (var dayPhase in dayPhases) dayPhase.ResetStartTime();
        }

        private void CreateSchedule(TimeTable<TPhase> timeTable)
        {
            CheckTimeTable(timeTable);
            var previousNode = default(ScheduleItem<TPhase>);
            foreach (var phase in Sorted())
            {
                var startTime = timeTable.StartTimes[phase];
                var node = ScheduleItem<TPhase>.Create(phase, startTime, !dayPhases.Any());
                previousNode?.SetNext(node);
                previousNode = node;
                dayPhases.Add(node);
            }

            dayPhases.Last().SetNext(dayPhases.First()); // full circle: last points to first again. For Between() 
        }

        private void CheckTimeTable(TimeTable<TPhase> timeTable, bool reschedule = false)
        {
            _ = timeTable ?? throw new ArgumentNullException(nameof(timeTable));
            if (timeTable.StartTimes is null || timeTable.StartTimes.Keys.Count == 0)
                throw new InvalidOperationException($"Empty {nameof(timeTable)}");

            var previousStartTime = TimeSpan.Zero;
            foreach (var phase in Sorted())
            {
                if (!timeTable.StartTimes.TryGetValue(phase, out var startTime))
                {
                    if (!reschedule)
                        throw new InvalidOperationException($"Missing start time in schedule of " +
                                                            $"'{typeof(TPhase).Name}' for phase: '{phase}'");
                    continue;
                }

                if (previousStartTime != TimeSpan.Zero && startTime <= previousStartTime)
                    throw new InvalidOperationException(
                        $"Overlapping start time in schedule of '{typeof(TPhase).Name}' " +
                        $"for phase '{phase}'");
                previousStartTime = startTime;
            }
        }

        private static IEnumerable<TPhase> Sorted()
        {
            return from value in Enum.GetValues(typeof(TPhase)).Cast<TPhase>()
                orderby value
                select value;
        }
    }
}