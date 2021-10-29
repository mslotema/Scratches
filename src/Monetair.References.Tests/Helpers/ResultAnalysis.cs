using System.Collections.Generic;
using System.Linq;

namespace Monetair.References.Tests.Uniqueness.Helpers
{
    /// <summary>
    ///     Analyses the results of multi threaded tests
    /// </summary>
    public class ResultAnalysis
    {
        public List<string> Results { get; } = new();

        public ResultAnalysis Add(IEnumerable<string> items)
        {
            Results.AddRange(items);

            return this;
        }

        public IEnumerable<string> DoubleValues()
        {
            return Results
                .GroupBy(s => s)
                .SelectMany(grp => grp.Skip(1))
                .Distinct()
                .ToArray();
        }
    }
}