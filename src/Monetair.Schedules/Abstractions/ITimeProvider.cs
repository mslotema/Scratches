using System;

// ReSharper disable once CheckNamespace
namespace Monetair
{
    /// <summary>
    /// Stub TimeProvider interface
    /// </summary>
    public interface ITimeProvider
    {
        TimeSpan CurrentTime { get; }

        DateTime ValueDate { get; }
    }

    public class TimeProvider : ITimeProvider
    {
        public TimeSpan CurrentTime => DateTime.Now.TimeOfDay;

        public DateTime ValueDate => DateTime.Today;
    }
}