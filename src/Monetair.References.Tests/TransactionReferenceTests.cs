using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Monetair.References.Tests.Uniqueness.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace Monetair.References.Tests
{
    public class UniqueThreadedTransactionReferenceTests
    {
        private readonly ITestOutputHelper output;

        public UniqueThreadedTransactionReferenceTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void UniqueTransactionReferences_OneTask_List()
        {
            // the "singleton" instance
            var instance = new TransactionReferenceForTest();
            var runsPerTask = 100;

            var values = new List<string>();
            for (var i = 0; i < runsPerTask; i++)
            {
                values.Add(instance.Create());
            }

            Assert.Equal(runsPerTask, values.Count);
            foreach (var value in values)
            {
                output.WriteLine(value);
            }
        }

        [Fact]
        public void UniqueTransactionReferences_Returns_NoDoubles()
        {
            // the "singleton" instance
            var options = new TransactionReferenceOptions
            {
                Identity = "X"
            };
            var instance = new TransactionReference(options);
            var analysis = new ResultAnalysis();

            var totalTasks = 10;
            var runsPerTask = 100;

            var tasks = new List<Task>();
            var collectedValues = new List<ICollection<string>>();

            output.WriteLine("Create tasks");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (var tasknr = 0; tasknr < totalTasks; tasknr++)
            {
                var values = new List<string>();
                collectedValues.Add(values);
                tasks.Add(Task.Run(() => CreateValues(values, runsPerTask)));
            }

            output.WriteLine("Wait for all to end");
            Task.WaitAll(tasks.ToArray());
            stopwatch.Stop();

            output.WriteLine($"Time elapsed: {stopwatch.ElapsedMilliseconds} ms");
            output.WriteLine("Analyse...");
            var allValues = collectedValues.SelectMany(values => values).ToArray();
            analysis.Add(allValues);

            var result = analysis.DoubleValues().ToArray();
            var total = totalTasks * runsPerTask;
            output.WriteLine($"Total {total}; Doubles: {result.Length}");
            Write(result);

            Assert.Equal(totalTasks * runsPerTask, allValues.Count());
            Assert.Empty(result);

            // local function
            void CreateValues(List<string> values, int nrRuns)
            {
                for (var run = 0; run < nrRuns; run++)
                {
                    values.Add(instance.Create());
                }
            }
        }

        private void Write<T>(IEnumerable<T> list)
        {
            var txt = string.Join(", ", list.Select(x => x.ToString()));
            output.WriteLine($"{{ {txt} }}");
        }
    }
}