using System;
using RefitExample.Interfaces;

namespace RefitExample.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public void Write(string message) => Console.WriteLine(message);
    }
}
