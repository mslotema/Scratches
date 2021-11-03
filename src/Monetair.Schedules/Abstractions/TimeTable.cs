using System;
using System.Collections.Generic;
using Monetair.Schedules.Internal;

// ReSharper disable once CheckNamespace
namespace Monetair.Schedules
{
    /// <summary>
    /// Time Table for type <param name="TPhase"/>.
    /// It contains the start times for each phase of the schedule, in order
    /// <para>
    ///   The order is determined by the TPhase enum
    /// </para>
    /// </summary>
    /// <typeparam name="TPhase"></typeparam>
    public class TimeTable<TPhase>
        where TPhase : Enum
    {
        public Dictionary<TPhase, TimeSpan> StartTimes { get; set; } = new Dictionary<TPhase, TimeSpan>();

        // fluent
        public TimeTable<TPhase> Add(TPhase phase, string startTime)
        {
            return Add(phase, startTime.ToTime());
        }

        public TimeTable<TPhase> Add(TPhase phase, TimeSpan startTime)
        {
            StartTimes.Add(phase, startTime);

            return this;
        }
    }
}