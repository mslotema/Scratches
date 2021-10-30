// ReSharper disable once CheckNamespace

namespace Monetair.References
{
    /// <summary>
    ///     Creates a thread safe, unique transaction reference, based on the current
    ///     time and date.
    ///     <para>
    ///         The value is exactly 16 characters wide, always.
    ///         The meaning of the given time and date is not important. It is
    ///         purely a unique value per call.
    ///     </para>
    ///     <para>
    ///         The use of an "Identity"(one or two characters) makes this value
    ///         unique across applications or instances. (e.g. "A", or "XY")
    ///     </para>
    ///     <para>
    ///         Note: The implementing class must be a singleton for it to work properly
    ///     </para>
    /// </summary>
    public interface ITransactionReference
    {
        /// <summary>
        /// Create a new value
        /// <para>
        /// 16 characters long, a timestamp with a resolution of 1 ms.
        /// Thread safe and unique, with a fixed value in the middle (for uniqueness across applications).
        /// </para>
        /// </summary>
        string Create();
    }
}