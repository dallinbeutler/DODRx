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

   [System.Composition.Export(typeof(DOD.IDataStream<long>)), System.Composition.Shared]
   public class SysTime : DataStream<long, SysTime.DilOption>
   {

      public class DilOption
      {
         public delegate void DilationChangedEventHandler();//double old, double newVal);
         event DilationChangedEventHandler DilChanged;
         private double _Dilation;

         public double Dilation
         {
            get
            {
               return _Dilation;
            }
            set
            {
               _Dilation = value;
               DilChanged.Invoke();
            }
         }

         public double FinalDilation { get; set; }
         //amount it affects this entity, and the Dilation to affect with
         public Tuple<double, DilOption>[] AffectedBy { get; set; }
         public DilOption(double dilation, params Tuple<double, DilOption>[] dilOptions)
         {
            _Dilation = dilation;
            FinalDilation = Dilation;
            AffectedBy = dilOptions;
            foreach (var d in dilOptions)
               DilChanged += d.Item2.DilChanged;
            DilChanged += Recalculate;
         }

         private void Recalculate()
         {
            FinalDilation = Dilation;
            foreach (var ab in AffectedBy)
            {
               FinalDilation *= System.Math.Pow(ab.Item2.Dilation, ab.Item1); 
            }
         }
         //private double lerp(double value1, double value2, double percent)
         //{
         //   return (value1 * (1 - percent)) + (value2 * percent);
         //}
      }

      public DilOption Global = new DilOption(1.0);
      public DilOption Room;
      Stopwatch stopwatch;

      public SysTime() : base("SysTime", null, null)
      {
         stopwatch = new Stopwatch();
         stopwatch.Start();
         this[-1] = Global;
         this[-2] = Room = new DilOption(1.0, Tuple.Create(1.0, Global));
         DefaultVal = Global;
      }

      public IObservable<long> DilatedTimer(long Entity, TimeSpan duration)
      {

         return Observable.Create((IObserver<long> observer) =>
         {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            DilOption info;
            if (this.HasEntity(Entity))
            {
               info = this[Entity];
            }
            info = this.Room;
            var LastTick = stopwatch.Elapsed;
            var elapsed = new TimeSpan();
            var ticker = Observable.Interval(new TimeSpan(0, 0, 0, 0, 1 / 30000))
            .Subscribe(x =>
            {
               //if (!info.GloballyAffected)
               //{
               var timeBetween = stopwatch.Elapsed - LastTick;

               elapsed += TimeSpan.FromTicks((long)(timeBetween.Ticks * info.FinalDilation));
               if (elapsed > duration)
               {
                  observer.OnNext(elapsed.Ticks);
                  observer.OnCompleted();
                  
               }
               //}
               LastTick = stopwatch.Elapsed;
            });

            return ticker;
         });
      }

      
   }
}
