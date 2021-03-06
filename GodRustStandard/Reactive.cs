﻿using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodRustShared
{
   public static class ReactiveExt
   {

      public static IObservable<T> RepeatLastValueDuringSilence<T>(this IObservable<T> inner, TimeSpan maxQuietPeriod)
      {
         return inner.Select(x =>
             Observable.Interval(maxQuietPeriod)
                       .Select(_ => x)
                       .StartWith(x)
         ).Switch();
      }
   }

}
