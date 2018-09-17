using System;
using System.Collections.Generic;
using System.Text;

namespace Components
{
   using DOD;
   using System;
   using System.Collections.Generic;
   using System.Reactive.Subjects;


   public struct Dil
   {
      public bool GlobalAffects;
      public float IndividualAmount;

   }
   [System.Composition.Export(typeof(DOD.IDataStream<long>)), System.Composition.Shared]
   public class SysDilation : DataStream<long, Dil>
   {
      public float GlobalDilation = 1.0f;
      public SysDilation()
      {
         this[-1] = new Dil() { GlobalAffects = true, IndividualAmount = 1.0f };
         this[-2] = new Dil() { GlobalAffects = false, IndividualAmount = 1.0f };
      }
      public float GetDilation(long entity, bool DilationFallback = true)
      {
         Dil e;
         if (!this.DataSet.ContainsKey(entity))
         {
            e = this[DilationFallback ? -1 : -2];
         }
         else
         {
            e = this[entity];
         }
         return e.GlobalAffects ? (GlobalDilation * e.IndividualAmount) : e.IndividualAmount;

      }

      public List<TimedEvent> timedEvents = new List<TimedEvent>();
      public void Update(TimeSpan delta)
      {
         foreach (var i in timedEvents)
         {
            i.Update(new TimeSpan( (long)(delta.Ticks * GetDilation(i.Entity)) ));
         }
      }
   }
   public enum TimerState
   {
      Paused,
      Completed,
      Restarted,
      Resumed,
   }
   public interface ITimer : IDisposable
   {
      TimeSpan MSStart { get; }
      TimeSpan MSLeft { get; }
      Subject<TimerState> OnCompleted { get; }
      bool IsPaused { get; }
      void Pause();
      void Resume();
      void Toggle();
      void Restart();
   }

   public struct TimedEvent : ITimer
   {
      public TimeSpan MSStart { get; private set; }
      public TimeSpan MSLeft { get; private set; }
      public Subject<TimerState> OnCompleted { get; private set; }
      public bool IsPaused { get; private set; }
      public long Entity { get; private set; }
      public TimedEvent(long Entity, TimeSpan timeSpan)
      {
         MSLeft = timeSpan;
         MSStart = MSLeft;
         OnCompleted = new Subject<TimerState>();
         IsPaused = false;
         this.Entity = Entity;
      }

      public void Dispose()
      {
         OnCompleted.Dispose();
      }
      public void Pause()
      {
         if (IsPaused == false)
         {
            IsPaused = true;
            OnCompleted.OnNext(TimerState.Paused);
         }
      }
      public void Resume()
      {
         if (IsPaused == true)
         {
            IsPaused = false;
            OnCompleted.OnNext(TimerState.Paused);
         }
      }
      public void Toggle()
      {
         if (IsPaused == true)
         {
            IsPaused = false;
            OnCompleted.OnNext(TimerState.Paused);
         }
         else
         {
            IsPaused = true;
            OnCompleted.OnNext(TimerState.Resumed);
         }
      }
      public void Restart()
      {
         MSLeft = MSStart;
         OnCompleted.OnNext(TimerState.Restarted);
      }
      public void Update(TimeSpan delta)
      {
         if (MSLeft.TotalMilliseconds > 0)
         {
            MSLeft -= delta;
            if (MSLeft.TotalMilliseconds <= 0)
            {
               OnCompleted.OnNext(TimerState.Completed);
            }
         }
      }
      //public enum TimerState
      //{
      //   Paused,
      //   Completed,
      //   Restarted,
      //   Resumed,
      //}

   }
}
