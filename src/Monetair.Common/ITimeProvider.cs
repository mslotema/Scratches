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
}