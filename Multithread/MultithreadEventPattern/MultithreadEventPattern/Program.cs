using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Timers;

namespace MultithreadEventPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            var progress = new Progress<int>();
            IObservable<EventPattern<int>> progressReports =
                Observable.FromEventPattern<int>(
                    handler => progress.ProgressChanged += handler,
                    handler => progress.ProgressChanged -= handler);
            progressReports.Subscribe(data => Console.WriteLine($"OnNext: {data.EventArgs}"));
            Trace.WriteLine("Error in Widget 42");
            Trace.WriteLine("Hello World!");

            var timer = new Timer(1) { Enabled = true };
            IObservable<EventPattern<ElapsedEventArgs>> ticks = Observable.FromEventPattern<ElapsedEventHandler, ElapsedEventArgs>(
                handler => (s, a) => handler(s, a),
                handler => timer.Elapsed += handler,
                handler => timer.Elapsed -= handler);

            ticks.Subscribe(data => Console.WriteLine($"OnNext: {data.EventArgs.SignalTime}"));

        }
    }
}
