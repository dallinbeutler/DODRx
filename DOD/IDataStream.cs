using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DOD
{
   public interface IDataStream<Key> //: Enumerable
   {
      string Name { get; }
      int Count { get; }
      bool HasEntity(Key ID);
      void Clear();
      bool RemoveAt(Key ID);
      void Set(Key ID, object o);
      object Get(Key ID);
      object GetObjOrDefault(Key ID);
      event NotifyDataStreamChangedEventHandler<Key> DataStreamChanged;
      IObservable<EntityChangedArgs<Key>> AsObservable { get; }
   }
}
