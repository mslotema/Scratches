using System;
using System.Threading;
using Microsoft.Extensions.Options;

/*
* 3 vormen:
*  - zonder "identity" (volledige timestamp + ms)
*  - met 1 letter identity (3 cijfer jaar - 900 jaar vooruit kunnen)
*  - met 2 letters identity (2 cijfers jaar - eeuw vooruit kunnen)
*
* Identity moet een "instance" zijn van een applicatie. Kan zelfde applicatie zijn met meerdere
* nodes, of verschillende applicaties
* id: A, B, C of AM (amsterdam), WS (Wassenaar), etc.
*
* Formaat (in principe): <YEAR:4-2><DAY OF YEAR:3><ID:1-2><HH><MM><SS><MS:3>
*
* _concurrent safe_; met lock() of SlimMetaphore, SpinWait.. etc. Snel, unieke waarde elke keer.
* Max 1 waarde per miliseconde. Dat is de resolutie.
*/
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
    ///         The use of an <see cref="Identity"/> (one or two characters) makes this value
    ///         unique across applications or instances.
    ///     </para>
    ///     <para>
    ///         Note: This class must be a singleton for it to work properly
    ///     </para>
    /// </summary>
    public class TransactionReference : AbstractReference, ITransactionReference
    {
        private readonly string identity;

        protected readonly object @lock = new();

        public TransactionReference(IOptions<TransactionReferenceOptions> options)
        {
            identity = options.Value.Identity;
        }

        protected override string Identity()
        {
            return identity;
        }

        /// <summary>
        /// Create a new value
        /// </summary>
        public override string Create()
        {
            lock (@lock)
            {
                Thread.Sleep(1); // rudimentair:  nooit dezelfde milliseconde opleveren als de vorige keer
                var now = DateTime.Now;

                return CreateValue(now);
            }
        }
    }
}