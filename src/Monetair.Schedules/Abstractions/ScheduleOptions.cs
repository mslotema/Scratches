using System.Collections.Generic;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Monetair.Schedules
{
    public class ScheduleOptions : IOptions<ScheduleOptions>
    {
        public ScheduleOptions Value => this;

        public Dictionary<string, TimeTable> Schedules { get; set; } = new Dictionary<string, TimeTable>();

        
    }
}