using System;

namespace Monetair.References.Tests.Uniqueness.Helpers
{
    public class TransactionReferenceForTest : TransactionReference
    {
        // Normaal gesproken zou dit een singleton moeten zijn!
        // Dat kan met DI, of met een static "instance" 
        // Dit is omdat waarden altijd uniek moeten zijn over de hele applicatie!

        public TransactionReferenceForTest() : base(new TransactionReferenceOptions { Identity = "A" })
        {
        }

        private string effectiveIdentity = "A";

        /// <summary>
        ///     Gets the value
        ///     <param name="fromMoment" />
        ///     <para>
        ///         This is for testing only! it will not produce unique values.
        ///     </para>
        /// </summary>
        public string GetValue(DateTime fromMoment)
        {
            lock (@lock)
            {
                return CreateValue(fromMoment);
            }
        }

        public void SetIdentity(string newValue)
        {
            lock (@lock)
            {
                effectiveIdentity = newValue.AsMaxLength(2);
            }
        }

        protected override string Identity()
        {
            return effectiveIdentity;
        }
    }
}