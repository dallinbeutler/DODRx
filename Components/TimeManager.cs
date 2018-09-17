using DOD;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;

namespace Components
{
   //struct TimedObservable
   //{
   //   long EntityID;
   //   TimeSpan Timeleft;
   //}

   class TimeManager
   {
      //public TimeManager()
      //{
      //   globalstopwatch = new Stopwatch();
      //   globalstopwatch.Start();
      //}
      //Stopwatch globalstopwatch;

      //List<Tuple<long, TimeSpan>> timers;
      //TimeSpan FPS = new TimeSpan(0, 0, 0, 0, 1 / 60000);

      float GlobalDilation { get; set; } = 1.0f;

      public bool Paused
      {
         get
         {
            return Timer.IsRunning;
         }
         set
         {
            if (value)
               Timer.Start();
            else
               Timer.Stop();
         }
      }
      public double Dilation = 1.0f;
      Stopwatch Timer;

      public IObservable<long> DilatedTimer(TimeSpan duration)
      {
         return Observable.Create((IObserver<long> observer) =>
         {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            TimeSpan elapsed = new TimeSpan();
            var ticker = Observable.Interval(new TimeSpan(0, 0, 0, 0, 1 / 30000))
            .Subscribe(x =>
                  {
                     if (!Paused)
                     {
                        elapsed += new TimeSpan((long)(stopwatch.Elapsed.Ticks * Dilation));
                        if (elapsed > duration)
                        {
                           observer.OnCompleted();
                        }
                     }
                     stopwatch.Restart();
                  });


            return Disposable.Empty;
         });
      }

   }


}
