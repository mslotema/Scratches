using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Monetair.References.Tests.Uniqueness.Helpers
{
    public class ResultAnalysisTests
    {
        private readonly ITestOutputHelper output;

        public ResultAnalysisTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ResultAnalysis_TwoCollections_NoDoubles()
        {
            var analysis = new ResultAnalysis();

            analysis.Add(new List<string> { "aap", "noot", "mies" });
            analysis.Add(new List<string> { "wim", "zus", "jet" });

            var result = analysis.DoubleValues();

            Assert.Empty(result);
        }

        [Fact]
        public void ResultAnalysis_TwoCollections_WithDoubles()
        {
            var analysis = new ResultAnalysis();

            analysis.Add(new List<string> { "aap", "noot", "mies" });
            analysis.Add(new List<string> { "wim", "zus", "jet", "mies", "aap" });

            var result = analysis.DoubleValues();

            Assert.Equal(2, result.Count());
        }

        private void Write<T>(IEnumerable<T> list)
        {
            var txt = string.Join(", ", list.Select(x => x.ToString()));
            output.WriteLine($"{{ {txt} }}");
        }
    }
}