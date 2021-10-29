using System;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable once CheckNamespace
namespace Monetair.References.Tests
{
    public class OtherFormatsTests
    {
        private readonly ITestOutputHelper output;

        private readonly DateTime d1 = new(2021, 10, 27, 17, 0, 20, 200);

        private DateTime d2 = new(2021, 8, 4, 17, 15, 20, 200);

        public OtherFormatsTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void OtherRef_HexDate()
        {
            output.WriteLine($"date: {d1:yyyy-MM-dd}, day of year: {d1.DayOfYear}");
            var partString = $"{d1.Year}{d1.DayOfYear:D3}";
            var partNum = d1.Year * 1000 + d1.DayOfYear;

            output.WriteLine($"as string: {partString}");
            output.WriteLine($"as num: {partNum}");
            output.WriteLine($"as hex: {partNum:X}");
        }

        [Fact]
        public void Ticks_Hex()
        {
            output.WriteLine($"date: {d1:yyyy-MM-dd}, day of year: {d1.DayOfYear}");
            var ticks = d1.Ticks;
            output.WriteLine($"ticks: {ticks}");
            output.WriteLine($"ticks, hex: {ticks:X}, length: {ticks.ToString("X").Length}");
        }

        [Fact]
        public void AsFileTime()
        {
            var result = "initial value";
            var ftime = d1; //DateTime.Now;
            output.WriteLine($"date: {ftime:yyyy-MM-dd} {ftime:HH:mm:ss:fff}, day of year: {d1.DayOfYear}");

            var x = Interlocked.Exchange(ref result, CreateValue(ftime));

            output.WriteLine($"result: {result}");
            output.WriteLine($"x: {x}");
        }


        private string CreateValue(DateTime moment)
        {
            var identity = "A";

            return $"{moment.ToString("yyyy").Substring(1, 3)}{moment.DayOfYear:D3}" +
                   $"{identity}" +
                   $"{moment:HH}{moment:mm}{moment:ss}{moment:fff}";
        }
    }
}