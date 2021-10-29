using System;

// ReSharper disable once CheckNamespace
namespace Monetair.References
{
    internal static class TransactionReferenceExtensions
    {
        public static string Year2(this DateTime moment)
        {
            return moment.ToString("yy");
        }

        public static string Year3(this DateTime moment)
        {
            return moment.ToString("yyyy").Substring(1, 3);
        }

        public static string Year4(this DateTime moment)
        {
            return moment.ToString("yyyy");
        }

        public static string TotalTime(this DateTime moment)
        {
            return $"{moment:HH}{moment:mm}{moment:ss}{moment:fff}";
        }

        public static string DayOfYear(this DateTime moment)
        {
            return moment.DayOfYear.ToString("D3");
        }
    }
}