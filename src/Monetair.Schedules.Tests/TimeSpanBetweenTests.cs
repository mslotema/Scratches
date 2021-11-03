using System;
using System.Globalization;
using Monetair.Schedules.Internal;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable once CheckNamespace
namespace Monetair.Schedules.Tests
{
    public class TimeSpanBetweenTests
    {
        private readonly ITestOutputHelper output;

        public TimeSpanBetweenTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        // night phase (passes midnight
        [InlineData("22:00", "07:00", "23:00", true, "before midnight")]
        [InlineData("22:00", "07:00", "01:00", true, "after midnight")]
        [InlineData("22:00", "07:00", "8:00", false, "after time block")]
        [InlineData("22:00", "07:00", "21:59", false, "before time block")]
        [InlineData("22:00", "07:00", "22:00", true, "on start time")]
        [InlineData("22:00", "07:00", "06:59:59", true, "just before end of time block")]
        [InlineData("22:00", "07:00", "07:00", false, "at end of time block")]

        // night phase, midnight exactly
        [InlineData("22:00", "07:00", "00:00", true, "(exactly) at midnight")]

        // a day time phase
        [InlineData("07:00", "17:00", "09:00", true, "daytime, middle")]
        [InlineData("07:00", "17:00", "07:00", true, "daytime, on start")]
        [InlineData("07:00", "17:00", "06:59:59", false, "daytime, just before start")]
        [InlineData("07:00", "17:00", "17:00", false, "daytime, on end")]
        [InlineData("07:00", "17:00", "16:59:59", true, "daytime, just before end")]
        [InlineData("07:00", "17:00", "00:00", false, "daytime, at midnight")]
        [InlineData("07:00", "17:00", "01:00", false, "daytime, after midnight")]

        // a phase with start or end at midnight
        [InlineData("00:00", "07:00", "00:00", true, "midnight start, at midnight")]
        [InlineData("00:00", "07:00", "23:59:59", false, "midnight start, just before midnight")]
        [InlineData("00:00", "07:00", "00:00:01", true, "midnight start, just after midnight")]
        [InlineData("23:00", "00:00", "00:00", false, "midnight end, at midnight")]
        [InlineData("23:00", "00:00", "23:59:59", true, "midnight end, just before midnight")]
        [InlineData("23:00", "00:00", "0:00:01", false, "midnight end, just after midnight")]
        public void TimeSpanBetween_TestCases(string startTime, string endTime, string sampleTime, bool expected,
            string theory)
        {
            // Arrange
            var earlier = startTime.ToTime();
            var later = endTime.ToTime();
            var moment = sampleTime.ToTime();

            // Act
            output.WriteLine($"[{earlier} - {later}], time {moment} => {expected} ({theory})");
            var result = moment.Between(earlier, later);

            // Assert   
            Assert.Equal(expected, result);
        }
    }

    public static class TimeSpanTestExtensions
    {
        public static TimeSpan ToTime(this string timeString)
        {
            if (TimeSpan.TryParse(timeString, out var time)) return time;
            throw new ArgumentException($"Invalid time string: '{timeString}");
        }
    }
}