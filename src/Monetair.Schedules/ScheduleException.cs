using System;
using System.Runtime.Serialization;

namespace Monetair.Schedules
{
    [Serializable]
    public class TradingDayScheduleException : Exception
    {
        public TradingDayScheduleException()
        {
        }

        protected TradingDayScheduleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public TradingDayScheduleException(string message) : base(message)
        {
        }

        public TradingDayScheduleException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}