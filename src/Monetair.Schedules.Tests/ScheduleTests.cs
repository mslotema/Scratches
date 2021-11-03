using System;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable once CheckNamespace
namespace Monetair.Schedules.Tests
{
    public class ScheduleTests
    {
        private readonly ITestOutputHelper output;

        public ScheduleTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        // ==== create schedule and reschedule ==== 
        [Fact]
        public void Schedule_CreateSchedule_ReturnsValidSchedule()
        {
            var timeTable = new TimeTable<DayPhases>()
                .Add(DayPhases.BusinessDay, "07:00")
                .Add(DayPhases.EndOfDay, "17:00")
                .Add(DayPhases.Night, "19:00");

            var schedule = new DaySchedule<DayPhases>(timeTable);

            Assert.Equal("07:00".ToTime(), schedule.StartTime(DayPhases.BusinessDay));
            Assert.Equal("17:00".ToTime(), schedule.StartTime(DayPhases.EndOfDay));
            Assert.Equal("19:00".ToTime(), schedule.StartTime(DayPhases.Night));
        }

        [Fact]
        public void Schedule_Reschedule_ReturnsNewStartTimes()
        {
            var timeTable = new TimeTable<DayPhases>()
                .Add(DayPhases.BusinessDay, "07:00")
                .Add(DayPhases.EndOfDay, "17:00")
                .Add(DayPhases.Night, "19:00");

            var schedule = new DaySchedule<DayPhases>(timeTable);

            var newTimeTable = new TimeTable<DayPhases>()
                .Add(DayPhases.EndOfDay, "18:30")
                .Add(DayPhases.Night, "21:00");
            schedule.Reschedule(newTimeTable);

            Assert.Equal("07:00".ToTime(), schedule.StartTime(DayPhases.BusinessDay));
            Assert.Equal("18:30".ToTime(), schedule.StartTime(DayPhases.EndOfDay));
            Assert.Equal("21:00".ToTime(), schedule.StartTime(DayPhases.Night));
        }

        [Fact]
        public void Schedule_Reschedule_UnchangedOriginalStartTimes()
        {
            var timeTable = new TimeTable<DayPhases>()
                .Add(DayPhases.BusinessDay, "07:00")
                .Add(DayPhases.EndOfDay, "17:00")
                .Add(DayPhases.Night, "19:00");

            var schedule = new DaySchedule<DayPhases>(timeTable);

            var newTimeTable = new TimeTable<DayPhases>()
                .Add(DayPhases.EndOfDay, "18:30")
                .Add(DayPhases.Night, "21:00");
            schedule.Reschedule(newTimeTable);

            Assert.Equal("07:00".ToTime(), schedule.OriginalStartTime(DayPhases.BusinessDay));
            Assert.Equal("17:00".ToTime(), schedule.OriginalStartTime(DayPhases.EndOfDay));
            Assert.Equal("19:00".ToTime(), schedule.OriginalStartTime(DayPhases.Night));
        }

        [Fact]
        public void Schedule_Reschedule_Reset_ReturnsOriginalStartTimes()
        {
            var timeTable = new TimeTable<DayPhases>()
                .Add(DayPhases.BusinessDay, "07:00")
                .Add(DayPhases.EndOfDay, "17:00")
                .Add(DayPhases.Night, "19:00");

            var schedule = new DaySchedule<DayPhases>(timeTable);

            var newTimeTable = new TimeTable<DayPhases>()
                .Add(DayPhases.EndOfDay, "18:30")
                .Add(DayPhases.Night, "21:00");
            schedule.Reschedule(newTimeTable);

            Assert.Equal("07:00".ToTime(), schedule.StartTime(DayPhases.BusinessDay));
            Assert.Equal("18:30".ToTime(), schedule.StartTime(DayPhases.EndOfDay));
            Assert.Equal("21:00".ToTime(), schedule.StartTime(DayPhases.Night));

            schedule.ResetSchedule();

            Assert.Equal("07:00".ToTime(), schedule.StartTime(DayPhases.BusinessDay));
            Assert.Equal("17:00".ToTime(), schedule.StartTime(DayPhases.EndOfDay));
            Assert.Equal("19:00".ToTime(), schedule.StartTime(DayPhases.Night));
        }

        // ==== get current phases ====
        [Theory]
        [InlineData("07:00", DayPhases.BusinessDay)]
        [InlineData("07:01", DayPhases.BusinessDay)]
        [InlineData("16:59:59", DayPhases.BusinessDay)]
        [InlineData("17:00", DayPhases.EndOfDay)]
        [InlineData("18:59:59", DayPhases.EndOfDay)]
        [InlineData("19:00", DayPhases.Night)]
        [InlineData("00:00", DayPhases.Night)]
        [InlineData("06:59:00", DayPhases.Night)]
        public void Schedule_GetPhaseOnTime_ReturnsCorrectPhase(string time, DayPhases expected)
        {
            var timeTable = new TimeTable<DayPhases>()
                .Add(DayPhases.BusinessDay, "07:00")
                .Add(DayPhases.EndOfDay, "17:00")
                .Add(DayPhases.Night, "19:00");

            var schedule = new DaySchedule<DayPhases>(timeTable);
            var result = schedule.Current(time.ToTime());
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Schedule_CorrectPhase_AfterReschedule_ReturnsExpectedPhase()
        {
            var timeTable = new TimeTable<DayPhases>()
                .Add(DayPhases.BusinessDay, "07:00")
                .Add(DayPhases.EndOfDay, "17:00")
                .Add(DayPhases.Night, "19:00");

            var schedule = new DaySchedule<DayPhases>(timeTable);

            var sampleTime = "20:00".ToTime();
            output.WriteLine($"Schedule.{DayPhases.EndOfDay}: {schedule.StartTime(DayPhases.EndOfDay)}");
            output.WriteLine($"Schedule.{DayPhases.Night}: {schedule.StartTime(DayPhases.Night)}");
            output.WriteLine($"Sample time: {sampleTime}\n");

            var result = schedule.Current(sampleTime);
            Assert.Equal(DayPhases.Night, result);
            output.WriteLine($"Returned phase on {sampleTime}: {result}\n");

            var newTimeTable = new TimeTable<DayPhases>()
                .Add(DayPhases.Night, "21:00");
            schedule.Reschedule(newTimeTable);

            output.WriteLine($"Schedule.{DayPhases.Night}: {schedule.StartTime(DayPhases.Night)} (rescheduled)\n");

            result = schedule.Current(sampleTime);
            Assert.Equal(DayPhases.EndOfDay, result);
            output.WriteLine($"Returned phase on {sampleTime}: {result}");
        }

        // ==== unhappy flows ====

        [Fact]
        public void Schedule_IncompleteTimeTable_ThrowsException()
        {
            var timeTable = new TimeTable<DayPhases>()
                .Add(DayPhases.BusinessDay, "07:00")
                // MISSING DayPhases.EndOfDay
                .Add(DayPhases.Night, "19:00");

            var ex = Assert.Throws<InvalidOperationException>(() => new DaySchedule<DayPhases>(timeTable));
            output.WriteLine($"Exception.Message: {ex.Message}");
        }

        [Fact]
        public void Schedule_OverlappingTimeTable_ThrowsException()
        {
            var timeTable = new TimeTable<DayPhases>()
                .Add(DayPhases.BusinessDay, "07:00")
                .Add(DayPhases.EndOfDay, "17:00")
                .Add(DayPhases.Night, "17:00");

            var ex = Assert.Throws<InvalidOperationException>(() => new DaySchedule<DayPhases>(timeTable));
            output.WriteLine($"Exception.Message: {ex.Message}");
        }

        [Fact]
        public void Schedule_EmptyTimeTable_ThrowsException()
        {
            var timeTable = new TimeTable<DayPhases>();

            var ex = Assert.Throws<InvalidOperationException>(() => new DaySchedule<DayPhases>(timeTable));
            output.WriteLine($"Exception.Message: {ex.Message}");
        }

        [Fact]
        public void Schedule_TryRescheduleFirstPhase_ThrowsException()
        {
            var timeTable = new TimeTable<DayPhases>()
                .Add(DayPhases.BusinessDay, "07:00")
                .Add(DayPhases.EndOfDay, "17:00")
                .Add(DayPhases.Night, "19:00");

            var schedule = new DaySchedule<DayPhases>(timeTable);

            var newTimeTable = new TimeTable<DayPhases>()
                .Add(DayPhases.BusinessDay, "07:00") // value is not relevant
                .Add(DayPhases.EndOfDay, "18:30")
                .Add(DayPhases.Night, "21:00");

            var ex = Assert.Throws<InvalidOperationException>(() => schedule.Reschedule(newTimeTable));
            output.WriteLine($"Exception.Message: {ex.Message}");
        }
    }


    /// <summary>
    /// Fake day schedule phases
    /// </summary>
    public enum DayPhases
    {
        BusinessDay,
        EndOfDay,
        Night
    }
}