using System;
using System.Collections.Generic;
using System.Text;
using DynamicData;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveUI;


namespace Testing
{
   class Testing
   {
      //Node<>
      SourceCache<KeyValuePair<long, TimeSpan>, long> source = new SourceCache<KeyValuePair<long,TimeSpan>, long>(x=> { return 1 ; });
      public Testing()
      {
         //source.AsObservableCache().ObservableForProperty(x=>x.)
         source.Lookup(2);
         //source.Watch(0).Subscribe(x => { x.c = 2; });
         //var c = source.Connect().Filter(x=>x.Value.Days == 3).Sort(SortExpre)
         //c.ToObservableChangeSet(expireAfter: x=>new TimeSpan(0,1,0)).Subscribe(x=>x.i);

      }
      
   }
}
