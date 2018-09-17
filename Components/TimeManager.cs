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
   class DilationSystem:DataStream<long,Tuple<bool,double>>
   {
      double GlobalDilation;
      Stopwatch stopwatch;

      public DilationSystem()
      {
         stopwatch.Start();
      }

      public IObservable<long> DilatedTimer(long Entity, TimeSpan duration)
      {

         return Observable.Create((IObserver<long> observer) =>
         {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            var info = this[Entity];
            var LastTick = stopwatch.Elapsed;
            var elapsed = new TimeSpan();
            var ticker = Observable.Interval(new TimeSpan(0, 0, 0, 0, 1 / 30000))
            .Subscribe(x =>
            {
               if (!info.Item1)
               {
                  var timeBetween = LastTick - stopwatch.Elapsed;
                  elapsed += TimeSpan.FromTicks((long)(timeBetween.Ticks * info.Item2));
                  if (elapsed > duration)
                  {

                     observer.OnCompleted();
                  }
               }
               LastTick = stopwatch.Elapsed;

            });

            return Disposable.Empty;
         });
      }

   }

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

      //float GlobalDilation { get; set; } = 1.0f;

      //public bool Paused { get; set; }

      //public double Dilation = 1.0f;
      //Stopwatch Timer;

      


   }


}
