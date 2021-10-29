using System;
using System.Runtime.Serialization;

// ReSharper disable once CheckNamespace
namespace Monetair.References
{
    [Serializable]
    public class TransactionReferenceException : Exception
    {
        public TransactionReferenceException()
        {
        }

        protected TransactionReferenceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public TransactionReferenceException(string message) : base(message)
        {
        }

        public TransactionReferenceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}