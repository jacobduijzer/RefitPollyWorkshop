using RefitExample.Interfaces;
using System;
using Xunit.Abstractions;

namespace RefitExample.Tests
{
    public class TestLogger : ILogger
    {
        private readonly ITestOutputHelper _output;

        public TestLogger(ITestOutputHelper output) =>
            _output = output ?? throw new ArgumentNullException(nameof(output));

        public void Write(string message) => _output.WriteLine(message);
    }
}
