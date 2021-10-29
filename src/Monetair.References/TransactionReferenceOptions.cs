using System;
using Microsoft.Extensions.Options;

namespace Monetair.References
{
    /// <summary>
    /// Options for <see cref="TransactionReference"/>, setting the identity injected in the values.
    /// <para>
    /// The class <see cref="TransactionReference"/> must be a singleton within the application
    /// for it to work properly. The goal is to create unique values.
    /// </para>
    /// </summary>
    public class TransactionReferenceOptions : IOptions<TransactionReferenceOptions>
    {
        private string identity = string.Empty;

        public TransactionReferenceOptions Value => this;

        /// <summary>
        /// The identity (token) that is inserted in every <see cref="TransactionReference"/> value
        /// that is created.
        /// <para>
        /// It can be empty (not recommended), 1 character, or 2 characters.
        /// The goal is to create unique values across applications and instances.
        /// </para>
        /// <para>
        /// It needs to be limited, because a <see cref="TransactionReference"/> cannot be longer
        /// than 16 characters.
        /// The year is shortened to accomodate this requirement (4 digits for no <see cref="Identity"/>,
        /// 3 for a single character and 2 digits for a double character version)
        /// </para>
        /// <para>
        ///  It is wise to limit the characterset to [A-Z0-9] for readability.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">If the Identity is more than 2 characters</exception>
        public string Identity
        {
            get => identity;
            set
            {
                if (value is not null && value.Length > 2)
                {
                    throw new TransactionReferenceException("Limit value to two characters maximum");
                }

                identity = value ?? string.Empty;
            }
        }
    }
}