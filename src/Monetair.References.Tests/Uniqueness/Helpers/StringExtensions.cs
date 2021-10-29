namespace Monetair.References.Tests.Uniqueness.Helpers
{
    public static class StringExtensions
    {
        public static string AsMaxLength(this string source, int maxLength)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }

            var len = source.Length <= maxLength ? source.Length : maxLength;

            return source.Substring(0, len);
        }
    }
}