using System;

namespace Monetair
{
    public class TimeProvider : ITimeProvider
    {
        public TimeSpan CurrentTime => DateTime.Now.TimeOfDay;

        public DateTime ValueDate => DateTime.Today;
    }
}