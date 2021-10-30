namespace Monetair.Schedules
{
    public class TradingDaySchedule
    {
        private readonly ITimeProvider timeProvider;

        public TradingDaySchedule(string name, ITimeProvider timeProvider, TimeTable timeTable)
        {
            Name = name;
            this.timeProvider = timeProvider;
            ConvertToSchedule(timeTable);
        }


        public string Name { get; }

        public DayPhase Current()
        {
            var moment = timeProvider.CurrentTime;
            // where are we

            return null;
        }

        private void ConvertToSchedule(TimeTable timeTable)
        {
        }
    }
}