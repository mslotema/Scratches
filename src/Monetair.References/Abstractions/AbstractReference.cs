using System;

// ReSharper disable once CheckNamespace
namespace Monetair.References
{
    /// <summary>
    ///     Creates the format of a Transaction Reference, based on date and time
    ///     <para>
    ///         The value is exactly 16 characters wide, always
    ///     </para>
    /// </summary>
    public abstract class AbstractReference
    {
        /// <summary>
        ///     Returns the identity to inject in the TransactionReference value
        ///     <para>
        ///         Each application, and each instance of an application should have a unique
        ///         <see cref="Identity" /> to inject in the value.
        ///     </para>
        ///     <para>
        ///         Can be empty (not recommended), one character or two characters.
        ///         Preferably [A-Z0-9].
        ///     </para>
        /// </summary>
        protected abstract string Identity();

        /// <summary>
        ///     Craetes a new TransactionReference value, based on <see cref="DateTime" />
        ///     <para>
        ///         The resolution is 1 millisecond
        ///     </para>
        ///     <para>
        ///         The goal is to make unique values for each call, so 1) thread safety needs to be implemented,
        ///         and 2) repeated calls should always return a new value (max. speed is 1 value per ms)
        ///     </para>
        /// </summary>
        public abstract string Create();


        /// <summary>
        /// </summary>
        /// <param name="moment"></param>
        /// <returns></returns>
        protected virtual string CreateValue(DateTime moment)
        {
            var id = Identity();

            return id.Length switch
            {
                // tot het Einde Der Tijden (not recommended: geen verschil in applicaties mogelijk):
                0 => $"{moment.Year4()}{moment.DayOfYear()}{moment.TotalTime()}",
                // dit millenium zijn we ok:
                1 => $"{moment.Year3()}{moment.DayOfYear()}{id}{moment.TotalTime()}",
                // deze eeuw:
                _ => $"{moment.Year2()}{moment.DayOfYear()}{id}{moment.TotalTime()}"
            };
        }
    }
}