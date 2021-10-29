using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Monetair.References.Tests.Uniqueness.Helpers
{
    public class HelperTests
    {
        private readonly ITestOutputHelper output;

        public HelperTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void HelperTest_SelectMany()
        {
            var list = new List<List<int>>();

            list.Add(new List<int> { 1, 3, 7, 8 });
            list.Add(new List<int> { 1, 2, 3, 9 });

            var combined = list.SelectMany(v => v);
            Write(combined);

            Assert.Equal(8, combined.Count());
        }

        [Fact]
        public void HelperTest_GetDoubles_ViaGroupBySelectManySkip1()
        {
            var list = new List<List<int>>();

            list.Add(new List<int> { 1, 3, 7, 8 });
            list.Add(new List<int> { 1, 2, 3, 9 });

            var combined = list.SelectMany(v => v);
            var result = combined
                .GroupBy(s => s)
                .SelectMany(grp => grp.Skip(1));

            Write(result);

            Assert.Equal(2, result.Count());
        }

        // Deze ziet er minder efficiënt uit.
        [Fact]
        public void HelperTest_GetDoubles_ViaGroupByWhereSkipSelectMany()
        {
            var list = new List<List<int>>();

            list.Add(new List<int> { 1, 3, 7, 8 });
            list.Add(new List<int> { 1, 2, 3, 9 });

            var combined = list.SelectMany(v => v);
            var result = combined
                .GroupBy(x => x) // group matching items
                .Where(g => g.Skip(1).Any()) // where the group contains more than one item
                .SelectMany(g => g) // re-expand the groups with more than one item
                // note:
                .Distinct(); // needs distinct!

            Write(result);

            Assert.Equal(2, result.Count());
        }

        private void Write<T>(IEnumerable<T> list)
        {
            var txt = string.Join(", ", list.Select(x => x.ToString()));
            output.WriteLine($"{{ {txt} }}");
        }
    }
}